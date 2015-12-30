using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BillPath
{
    public class ModelState
        : INotifyPropertyChanged
    {
        public static ModelState GetFor(object model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return ModelStateProviders.GetFor(model.GetType()).GetForRoot(model);
        }

        private static IReadOnlyDictionary<string, PropertyInfo> _GetRuntimePropertiesByNamesFor(Type type)
            => (from runtimeProperty in type.GetRuntimeProperties()
                let hasPublicGetter = runtimeProperty.GetMethod?.IsPublic ?? false
                let hasPublicSetter = runtimeProperty.SetMethod?.IsPublic ?? false
                let hasParameters = runtimeProperty.GetIndexParameters().Length > 0
                let isStatic = runtimeProperty.GetMethod?.IsStatic ?? runtimeProperty.SetMethod?.IsStatic ?? false
                where (hasPublicGetter || hasPublicSetter)
                    && !hasParameters
                    && !isStatic
                select runtimeProperty)
            .ToDictionary(
                runtimeProperty => runtimeProperty.Name,
                StringComparer.OrdinalIgnoreCase);

        private object _model;
        private ModelStateCache _modelStateCache;
        private readonly ModelErrors _errors;
        private readonly IReadOnlyDictionary<string, PropertyInfo> _runtimePropertiesByNames;
        private readonly Lazy<IDictionary<PropertyInfo, ModelState>> _modelPropertyStates;

        internal ModelState(object model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _model = model;
            _errors = new ModelErrors(model);

            _modelStateCache = new ModelStateCache(this);
            _runtimePropertiesByNames = _GetRuntimePropertiesByNamesFor(Model.GetType());
            _modelPropertyStates = new Lazy<IDictionary<PropertyInfo, ModelState>>(_GetModelStateProperties);
        }

        private IDictionary<PropertyInfo, ModelState> _GetModelStateProperties()
            => (from publicRuntimeProperty in _runtimePropertiesByNames.Values
                let hasPublicGetter = publicRuntimeProperty.GetMethod?.IsPublic ?? false
                where hasPublicGetter && !publicRuntimeProperty.PropertyType.IsPrimitive()
                select publicRuntimeProperty)
            .ToDictionary(
                runtimeProperty => runtimeProperty,
                runtimeProperty =>
                {
                    var propertyValue = runtimeProperty.GetValue(Model);
                    if (propertyValue == null)
                        return null;
                    else
                    {
                        var propertyValueModelState = _modelStateCache.GetFor(Model, propertyValue);
                        propertyValueModelState._modelStateCache = _modelStateCache;
                        propertyValueModelState.PropertyChanged += delegate { _RefreshErrors(); };

                        return propertyValueModelState;
                    }
                });

        public object Model
        {
            get
            {
                return _model;
            }
            private set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Model));

                _model = value;
                foreach (var propertyName in _runtimePropertiesByNames.Keys)
                    OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            }
        }

        public IModelErrors Errors
        {
            get
            {
                return _errors;
            }
        }

        public bool IsValid
            => !Errors.EnumerateAll().Any();
        private void _RefreshErrors()
        {
            _errors.Refresh();
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsValid)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
            => PropertyChanged?.Invoke(this, propertyChangedEventArgs);

        public object this[string propertyName]
        {
            get
            {
                PropertyInfo runtimeProperty;
                if (_TryGetRuntimeProperty(propertyName, false, out runtimeProperty)
                    && runtimeProperty.CanRead)
                {
                    ModelState modelPropertyState;
                    if (_modelPropertyStates.Value.TryGetValue(runtimeProperty, out modelPropertyState))
                        return modelPropertyState;
                    else
                        return runtimeProperty.GetValue(Model);
                }
                else
                    throw new ArgumentException(nameof(propertyName));
            }
            set
            {
                PropertyInfo runtimeProperty;
                if (_TryGetRuntimeProperty(propertyName, false, out runtimeProperty)
                    && (runtimeProperty.SetMethod?.IsPublic ?? false))
                {
                    _SetPropertyValue(runtimeProperty, value);
                    OnPropertyChanged(new PropertyChangedEventArgs($"Item[{runtimeProperty.Name}]"));
                    _errors.Refresh();
                    _RefreshErrors();
                }
                else
                    throw new ArgumentException(nameof(propertyName));
            }
        }

        private void _SetPropertyValue(PropertyInfo runtimeProperty, object value)
        {
            runtimeProperty.SetValue(Model, value);

            if (_modelPropertyStates.Value.ContainsKey(runtimeProperty))
                if (value == null)
                    _modelPropertyStates.Value[runtimeProperty] = null;
                else
                {
                    ModelState aggregateModelState;

                    if (_modelPropertyStates.Value.TryGetValue(runtimeProperty, out aggregateModelState))
                        aggregateModelState.Model =
                            ModelStateProviders
                                .GetFor(value.GetType(), Model.GetType())
                                .GetForAggregate(Model, value)
                                .Model;
                    else
                    {
                        aggregateModelState = _modelStateCache.GetFor(Model, value);
                        aggregateModelState._modelStateCache = _modelStateCache;
                        _modelPropertyStates.Value.Add(
                            runtimeProperty,
                            aggregateModelState);
                    }
                }
        }

        private bool _TryGetRuntimeProperty(string propertyName, bool ignoreCase, out PropertyInfo runtimeProperty)
            => (_runtimePropertiesByNames.TryGetValue(propertyName, out runtimeProperty)
                && (ignoreCase || propertyName.Equals(runtimeProperty.Name, StringComparison.Ordinal)));
    }

    public class ModelState<TModel>
        : ModelState
    {
        protected ModelState(TModel model)
            : base(model)
        {
        }

        new public TModel Model
            => (TModel)base.Model;
    }
}