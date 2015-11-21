using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BillPath
{
    public sealed class ModelErrors
        : IModelErrors
    {
        private struct PropertyErrors
            : IEnumerable<string>
        {
            public readonly ObservableCollection<string> _errors;
            public readonly ReadOnlyObservableCollection<string> _readOnlyErrors;

            public PropertyErrors(string propertyName, ObservableCollection<string> errors)
            {
                _errors = errors;
                _readOnlyErrors = new ReadOnlyObservableCollection<string>(errors);

                PropertyName = propertyName;
            }

            public string PropertyName { get; }

            public void AddRange(IEnumerable<string> errors)
            {
                foreach (var error in errors)
                    _errors.Add(error);
            }
            public void Clear()
                => _errors.Clear();

            public ReadOnlyObservableCollection<string> AsReadOnly()
                => _readOnlyErrors;

            public IEnumerator<string> GetEnumerator()
                => _errors.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();
        }

        private readonly object _model;
        private readonly ObservableCollection<string> _modelErrors;
        private readonly IDictionary<string, PropertyErrors> _propertyErrorsByNames;

        public ModelErrors(object model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _model = model;
            _modelErrors = new ObservableCollection<string>();
            _propertyErrorsByNames = new Dictionary<string, PropertyErrors>(StringComparer.OrdinalIgnoreCase);

            _FillErrors();
        }

        internal void Refresh()
        {
            _ClearAllErrors();
            _FillErrors();
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                ((INotifyPropertyChanged)_modelErrors).PropertyChanged += value;
            }
            remove
            {
                ((INotifyPropertyChanged)_modelErrors).PropertyChanged -= value;
            }
        }
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                _modelErrors.CollectionChanged += value;
            }
            remove
            {
                _modelErrors.CollectionChanged -= value;
            }
        }

        public int Count
            => _modelErrors.Count;

        IEnumerable<string> IReadOnlyDictionary<string, ReadOnlyObservableCollection<string>>.Keys
            => _propertyErrorsByNames.Keys;

        IEnumerable<ReadOnlyObservableCollection<string>> IReadOnlyDictionary<string, ReadOnlyObservableCollection<string>>.Values
            => _propertyErrorsByNames.Values.Select(propertyErrors => propertyErrors.AsReadOnly());

        public ReadOnlyObservableCollection<string> this[string propertyName]
            => _GetOrAddPropertyErrors(propertyName).AsReadOnly();

        bool IReadOnlyDictionary<string, ReadOnlyObservableCollection<string>>.ContainsKey(string key)
            => _propertyErrorsByNames.ContainsKey(key);

        bool IReadOnlyDictionary<string, ReadOnlyObservableCollection<string>>.TryGetValue(string key, out ReadOnlyObservableCollection<string> value)
        {
            PropertyErrors propertyErrors;

            value = null;
            if (_propertyErrorsByNames.TryGetValue(key, out propertyErrors))
                value = propertyErrors.AsReadOnly();

            return value != null;
        }

        public IEnumerator<string> GetEnumerator()
            => _modelErrors.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
        IEnumerator<KeyValuePair<string, ReadOnlyObservableCollection<string>>> IEnumerable<KeyValuePair<string, ReadOnlyObservableCollection<string>>>.GetEnumerator()
            => _propertyErrorsByNames.Select(propertyErrors => new KeyValuePair<string, ReadOnlyObservableCollection<string>>(
                propertyErrors.Key,
                propertyErrors.Value.AsReadOnly()))
            .GetEnumerator();

        public IEnumerable<string> EnumerateAll()
        {
            foreach (var error in _modelErrors)
                yield return error;
            foreach (var propertyErrors in _propertyErrorsByNames.Values)
                foreach (var propertyError in propertyErrors)
                    yield return propertyError;

            var modelValidator = new ModelValidator();
            var validated = new HashSet<object> { this };
            var toValidate = new Queue<object>(_GetPropertyValuesFrom(_model));

            while (toValidate.Any())
            {
                var objectToValidate = toValidate.Dequeue();

                foreach (var error in modelValidator.Validate(objectToValidate))
                    yield return error.ErrorMessage;

                validated.Add(objectToValidate);
                foreach (var objectPropertyValuesToValidate in _GetPropertyValuesFrom(objectToValidate))
                    if (!validated.Contains(objectPropertyValuesToValidate))
                        toValidate.Enqueue(objectPropertyValuesToValidate);
            }
        }

        private IEnumerable<object> _GetPropertyValuesFrom(object @object)
            => from runtimeProperty in @object.GetType().GetRuntimeProperties()
               let hasPublicGetter = runtimeProperty.GetMethod?.IsPublic ?? false
               let hasParameters = runtimeProperty.GetIndexParameters().Length > 0
               let isStatuc = runtimeProperty?.GetMethod?.IsStatic ?? false
               where hasPublicGetter && !hasParameters && !isStatuc
               let runtimePropertyValue = runtimeProperty.GetValue(@object)
               where runtimePropertyValue != null
               select runtimePropertyValue;

        private void _FillErrors()
        {
            foreach (var errorsByMemberName in _GetErrorsByMemberName())
                if (string.IsNullOrWhiteSpace(errorsByMemberName.Key))
                    _AddRange(_modelErrors, errorsByMemberName);
                else
                    _GetOrAddPropertyErrors(errorsByMemberName.Key).AddRange(errorsByMemberName);
        }
        private IEnumerable<IGrouping<string, string>> _GetErrorsByMemberName()
            => from validationResult in new ModelValidator().Validate(_model)
               let memberNames = from memberName in (validationResult.MemberNames ?? Enumerable.Empty<string>())
                                 select string.IsNullOrWhiteSpace(memberName) ? string.Empty : memberName
               from memberName in memberNames.DefaultIfEmpty(string.Empty)
               group validationResult.ErrorMessage by memberName;

        private PropertyErrors _GetOrAddPropertyErrors(string propertyName)
        {
            PropertyErrors propertyErrors;
            if (!_propertyErrorsByNames.TryGetValue(propertyName, out propertyErrors))
            {
                propertyErrors = new PropertyErrors(propertyName, new ObservableCollection<string>());
                _propertyErrorsByNames.Add(propertyName, propertyErrors);
            }

            return propertyErrors;
        }

        private void _ClearAllErrors()
        {
            _modelErrors.Clear();
            foreach (var propertyErrors in _propertyErrorsByNames.Values)
                propertyErrors.Clear();
        }
        private static void _AddRange<T>(ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
                collection.Add(item);
        }
    }
}