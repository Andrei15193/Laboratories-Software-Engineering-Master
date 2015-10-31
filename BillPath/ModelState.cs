using System;
using System.Collections.Concurrent;
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
        private static ConcurrentDictionary<Type, Lazy<IReadOnlyDictionary<string, PropertyInfo>>> _runtimePropertiesTypeCahce =
            new ConcurrentDictionary<Type, Lazy<IReadOnlyDictionary<string, PropertyInfo>>>();
        private static Lazy<IReadOnlyDictionary<string, PropertyInfo>> _GetRuntimePropertiesByNamesFor(Type type)
            => _runtimePropertiesTypeCahce.GetOrAdd(type, _GetRuntimePropertiesByNamesFrom);
        private static Lazy<IReadOnlyDictionary<string, PropertyInfo>> _GetRuntimePropertiesByNamesFrom(Type type)
            => new Lazy<IReadOnlyDictionary<string, PropertyInfo>>(
                () => type
                    .GetRuntimeProperties()
                    .ToDictionary(
                        runtimeProperty => runtimeProperty.Name,
                        StringComparer.OrdinalIgnoreCase));

        private object _model;
        private readonly Lazy<IReadOnlyDictionary<string, PropertyInfo>> _runtimePropertiesByNames;

        public ModelState(object model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _model = model;
            _runtimePropertiesByNames = _GetRuntimePropertiesByNamesFor(model.GetType());
            Errors = new ModelErrors(this);
        }

        public object Model
        {
            get
            {
                return _model;
            }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Model));

                _model = value;
            }
        }

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
                && runtimeProperty.CanRead
                && binder.ReturnType.GetTypeInfo().IsAssignableFrom(runtimeProperty.PropertyType.GetTypeInfo()))
            {
                result = runtimeProperty.GetValue(_model);
                return true;
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

                runtimeProperty.SetValue(_model, value);
                OnPropertyChanged(new PropertyChangedEventArgs(runtimeProperty.Name));

                if (wasValid != IsValid)
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsValid)));
                return true;
            }

            return false;
        }

        private bool _TryGetRuntimeProperty(string propertyName, bool ignoreCase, out PropertyInfo runtimeProperty)
            => (_runtimePropertiesByNames.Value.TryGetValue(propertyName, out runtimeProperty)
                && (ignoreCase || propertyName.Equals(runtimeProperty.Name, StringComparison.Ordinal)));
    }

    public class ModelState<TModel>
        : ModelState
    {
        public ModelState(TModel model)
            : base(model)
        {
        }

        new public TModel Model
        {
            get
            {
                return (TModel)base.Model;
            }
            protected set
            {
                base.Model = value;
            }
        }
    }
}