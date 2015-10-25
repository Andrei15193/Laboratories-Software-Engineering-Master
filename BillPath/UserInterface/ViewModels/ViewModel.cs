using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using BillPath.Models;

namespace BillPath.UserInterface.ViewModels
{
    public class ViewModel
        : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                if (propertyName == null)
                    throw new ArgumentNullException(nameof(propertyName));
                else
                    throw new ArgumentException("Cannot be empty or white space!", nameof(propertyName));

            _OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
        }
        private void _OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs == null)
                throw new ArgumentNullException(nameof(propertyChangedEventArgs));

            PropertyChanged?.Invoke(this, propertyChangedEventArgs);
            OnPropertyChanged(propertyChangedEventArgs);
        }
    }

    public class ViewModel<TModel>
        : ViewModel
    {
        private static readonly ModelValidator _modelValidator = new ModelValidator();
        private readonly IReadOnlyDictionary<string, ObservableCollection<string>> _errorsByPropertyNames;
        private readonly IReadOnlyDictionary<string, ReadOnlyObservableCollection<string>> _readonlyErrorsByPropertyNames;

        public ViewModel(TModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Model = model;
            _errorsByPropertyNames = typeof(TModel)
                .GetRuntimeProperties()
                .Select(propertyInfo => propertyInfo.Name)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Concat(Enumerable.Repeat(string.Empty, 1))
                .ToDictionary(
                    propertyName => propertyName,
                    delegate { return new ObservableCollection<string>(); },
                    StringComparer.OrdinalIgnoreCase);
            _readonlyErrorsByPropertyNames = _errorsByPropertyNames
                .ToDictionary(
                    errorsByPropertyName => errorsByPropertyName.Key,
                    errorsByPropertyName => new ReadOnlyObservableCollection<string>(errorsByPropertyName.Value),
                    StringComparer.OrdinalIgnoreCase);
            ValidateModel();

            PropertyChanged +=
                (sender, e) =>
                {
                    if (!nameof(IsValid).Equals(
                        e.PropertyName,
                        StringComparison.OrdinalIgnoreCase))
                        ValidateModel();
                };
        }

        protected void ValidateModel()
        {
            _ClearAllErrors();
            foreach (var validationResultsByMemberName in _GetValidationResultsByMemberName())
                _AddRange(
                    _errorsByPropertyNames[validationResultsByMemberName.Key],
                    validationResultsByMemberName);
            OnPropertyChanged(nameof(IsValid));
        }

        private IEnumerable<IGrouping<string, string>> _GetValidationResultsByMemberName()
        {
            return from validationResult in _modelValidator.Validate(Model)
                   let memberNames = from memberName in (validationResult.MemberNames ?? Enumerable.Empty<string>())
                                     select string.IsNullOrWhiteSpace(memberName) ? string.Empty : memberName
                   from memberName in memberNames.DefaultIfEmpty(string.Empty)
                   group validationResult.ErrorMessage by memberName.ToLowerInvariant();
        }

        private void _ClearAllErrors()
        {
            foreach (var errors in _errorsByPropertyNames.Values)
                errors.Clear();
        }
        private static void _AddRange<T>(ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
                collection.Add(item);
        }

        public TModel Model
        {
            get;
            protected set;
        }

        public bool IsValid
        {
            get
            {
                return !_errorsByPropertyNames.Values.Any(Enumerable.Any);
            }
        }
        public IReadOnlyDictionary<string, ReadOnlyObservableCollection<string>> Errors
            => _readonlyErrorsByPropertyNames;
    }
}