using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace BillPath
{
    public class ModelState
        : DynamicObject, INotifyPropertyChanged
    {
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

        private readonly IReadOnlyDictionary<string, PropertyInfo> _runtimePropertiesByNames;
        private readonly Lazy<IDictionary<PropertyInfo, ModelState>> _modelPropertyStates;

        internal ModelState(object model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Model = model;
            _runtimePropertiesByNames = _GetRuntimePropertiesByNamesFor(Model.GetType());
            _modelPropertyStates = new Lazy<IDictionary<PropertyInfo, ModelState>>(_GetModelStateProperties);
            Errors = new ModelErrors(this);
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
                    return propertyValue == null ? null : ModelStates.GetFor(propertyValue);
                });

        public object Model { get; }

        public IModelErrors Errors
        {
            get;
        }

        public bool IsValid
            => !Errors.EnumerateAll().Any();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
            => PropertyChanged?.Invoke(this, propertyChangedEventArgs);

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            PropertyInfo runtimeProperty;
            if (_TryGetRuntimeProperty(binder.Name, binder.IgnoreCase, out runtimeProperty)
                && runtimeProperty.CanRead)
            {
                ModelState modelPropertyState;
                if (_modelPropertyStates.Value.TryGetValue(runtimeProperty, out modelPropertyState)
                    && binder.ReturnType.GetTypeInfo().IsAssignableFrom(modelPropertyState.GetType().GetTypeInfo()))
                {
                    result = modelPropertyState;
                    return true;
                }
                if (binder.ReturnType.GetTypeInfo().IsAssignableFrom(runtimeProperty.PropertyType.GetTypeInfo()))
                {
                    result = runtimeProperty.GetValue(Model);
                    return true;
                }
            }

            result = null;
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            PropertyInfo runtimeProperty;
            if (_TryGetRuntimeProperty(binder.Name, binder.IgnoreCase, out runtimeProperty)
                && runtimeProperty.CanWrite
                && runtimeProperty.PropertyType.GetTypeInfo().IsAssignableFrom(binder.ReturnType.GetTypeInfo()))
            {
                var wasValid = IsValid;

                _SetPropertyValue(runtimeProperty, value);

                if (wasValid != IsValid)
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsValid)));
                return true;
            }

            return false;
        }
        private void _SetPropertyValue(PropertyInfo runtimeProperty, object value)
        {
            runtimeProperty.SetValue(Model, value);

            if (_modelPropertyStates.Value.ContainsKey(runtimeProperty))
                if (value == null)
                    _modelPropertyStates.Value[runtimeProperty] = null;
                else
                    _modelPropertyStates.Value[runtimeProperty] = ModelStates.GetFor(value);

            OnPropertyChanged(new PropertyChangedEventArgs(runtimeProperty.Name));
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