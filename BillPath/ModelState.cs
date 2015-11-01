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
        private static IEnumerable<Type> _primitiveTypes = new HashSet<Type>
        {
            typeof(object),

            typeof(byte),
            typeof(byte?),
            typeof(sbyte),
            typeof(sbyte?),

            typeof(short),
            typeof(short?),
            typeof(ushort),
            typeof(ushort?),

            typeof(int),
            typeof(int?),
            typeof(uint),
            typeof(uint?),

            typeof(long),
            typeof(long?),
            typeof(ulong),
            typeof(ulong?),

            typeof(float),
            typeof(float?),
            typeof(double),
            typeof(double?),
            typeof(decimal),
            typeof(decimal?),

            typeof(char),
            typeof(char?),
            typeof(string),

            typeof(DateTime),
            typeof(DateTime?),
            typeof(DateTimeOffset),
            typeof(DateTimeOffset?)
        };
        internal static bool IsPrimitive(Type type)
            => _primitiveTypes.Contains(type)
            || typeof(Enum).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo())
            || typeof(Delegate).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());

        private static ConcurrentDictionary<TypeInfo, IReadOnlyDictionary<string, PropertyInfo>> _runtimePropertiesTypeCahce =
            new ConcurrentDictionary<TypeInfo, IReadOnlyDictionary<string, PropertyInfo>>();
        private static IReadOnlyDictionary<string, PropertyInfo> _GetRuntimePropertiesByNamesFor(TypeInfo typeInfo)
            => _runtimePropertiesTypeCahce.GetOrAdd(typeInfo, _GetRuntimePropertiesByNamesFrom);
        private static IReadOnlyDictionary<string, PropertyInfo> _GetRuntimePropertiesByNamesFrom(TypeInfo typeInfo)
            => typeInfo
            .AsType()
            .GetRuntimeProperties()
            .ToDictionary(
                runtimeProperty => runtimeProperty.Name,
                StringComparer.OrdinalIgnoreCase);

        private readonly object _model;
        private readonly IDictionary<object, ModelState> _modelStatesByValue;
        private readonly IReadOnlyDictionary<string, PropertyInfo> _runtimePropertiesByNames;
        private readonly IDictionary<PropertyInfo, ModelState> _modelPropertyStates;

        public ModelState(object model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _model = model;
            _modelStatesByValue = new Dictionary<object, ModelState> { { model, this } };
            _runtimePropertiesByNames = _GetRuntimePropertiesByNamesFor(_model.GetType().GetTypeInfo());
            _modelPropertyStates = _GetModelStateProperties();
            Errors = new ModelErrors(this);
        }
        private ModelState(object model, IDictionary<object, ModelState> modelStatesByValue)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _model = model;
            _modelStatesByValue = modelStatesByValue;
            _runtimePropertiesByNames = _GetRuntimePropertiesByNamesFor(_model.GetType().GetTypeInfo());
            _modelPropertyStates = _GetModelStateProperties();
            Errors = new ModelErrors(this);
        }

        private IDictionary<PropertyInfo, ModelState> _GetModelStateProperties()
            => _runtimePropertiesByNames
            .Values
            .Where(runtimeProperty => runtimeProperty.CanRead
                && runtimeProperty.GetIndexParameters().Length == 0
                && !IsPrimitive(runtimeProperty.PropertyType))
            .ToDictionary(
                runtimeProperty => runtimeProperty,
                runtimeProperty =>
                {
                    var propertyValue = runtimeProperty.GetValue(_model);
                    return propertyValue == null ? null : _GetOrAddModelState(propertyValue);
                });

        public object Model
            => _model;

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
                if (_modelPropertyStates.TryGetValue(runtimeProperty, out modelPropertyState)
                    && binder.ReturnType.GetTypeInfo().IsAssignableFrom(modelPropertyState.GetType().GetTypeInfo()))
                {
                    result = modelPropertyState;
                    return true;
                }
                if (binder.ReturnType.GetTypeInfo().IsAssignableFrom(runtimeProperty.PropertyType.GetTypeInfo()))
                {
                    result = runtimeProperty.GetValue(_model);
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

                _SetPropertyValue(_model, runtimeProperty, value);

                if (wasValid != IsValid)
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsValid)));
                return true;
            }

            return false;
        }

        private void _SetPropertyValue(object model, PropertyInfo runtimeProperty, object value)
        {
            runtimeProperty.SetValue(model, value);

            if (_modelPropertyStates.ContainsKey(runtimeProperty))
                if (value == null)
                    _modelPropertyStates[runtimeProperty] = null;
                else
                    _modelPropertyStates[runtimeProperty] = _GetOrAddModelState(value);

            OnPropertyChanged(new PropertyChangedEventArgs(runtimeProperty.Name));
        }

        private ModelState _GetOrAddModelState(object value)
        {
            ModelState modelState;
            if (!_modelStatesByValue.TryGetValue(value, out modelState))
            {
                modelState = new ModelState(value, _modelStatesByValue);
                _modelStatesByValue.Add(value, modelState);
            }

            return modelState;
        }

        private bool _TryGetRuntimeProperty(string propertyName, bool ignoreCase, out PropertyInfo runtimeProperty)
            => (_runtimePropertiesByNames.TryGetValue(propertyName, out runtimeProperty)
                && (ignoreCase || propertyName.Equals(runtimeProperty.Name, StringComparison.Ordinal)));
    }

    public class ModelState<TModel>
        : ModelState
    {
        public ModelState()
            : base(typeof(TModel))
        {
        }
        public ModelState(TModel model)
            : base(model)
        {
        }

        new public TModel Model
            => (TModel)base.Model;
    }
}