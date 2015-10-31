﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace BillPath
{
    public class ModelErrors
        : DynamicObject, IModelErrors
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

        private readonly ModelState _modelState;
        private readonly ObservableCollection<string> _modelErrors;
        private readonly IDictionary<string, PropertyErrors> _propertyErrorsByNames;

        public ModelErrors(ModelState modelState)
        {
            if (modelState == null)
                throw new ArgumentNullException(nameof(modelState));

            _modelState = modelState;
            _modelErrors = new ObservableCollection<string>();
            _propertyErrorsByNames = new Dictionary<string, PropertyErrors>(StringComparer.OrdinalIgnoreCase);

            _FillErrors();
            _modelState.PropertyChanged +=
                delegate
                {
                    _ClearAllErrors();
                    _FillErrors();
                };
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            if (binder.ReturnType.GetTypeInfo().IsAssignableFrom(typeof(ReadOnlyObservableCollection<string>).GetTypeInfo()))
            {
                var propertyErrors = _GetOrAddPropertyErrors(binder.Name);

                if (binder.IgnoreCase || binder.Name.Equals(propertyErrors.PropertyName, StringComparison.Ordinal))
                    result = propertyErrors.AsReadOnly();
            }

            return result != null;
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
        public string this[int index]
            => _modelErrors[index];

        public IEnumerator<string> GetEnumerator()
            => _modelErrors.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public IEnumerable<string> EnumerateAll()
            => _modelErrors.Concat(
                _propertyErrorsByNames.Values.SelectMany(propertyErrorsByName => propertyErrorsByName));

        private void _FillErrors()
        {
            foreach (var errorsByMemberName in _GetErrorsByMemberName())
                if (string.IsNullOrWhiteSpace(errorsByMemberName.Key))
                    _AddRange(_modelErrors, errorsByMemberName);
                else
                    _GetOrAddPropertyErrors(errorsByMemberName.Key).AddRange(errorsByMemberName);
        }
        private IEnumerable<IGrouping<string, string>> _GetErrorsByMemberName()
            => from validationResult in new ModelValidator().Validate(_modelState.Model)
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