using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using BillPath.Models;

namespace BillPath.UserInterface.ViewModels
{
    public class ModelState
        : ModelContext
    {
        private static readonly ModelValidator _modelValidator = new ModelValidator();
        private readonly IReadOnlyDictionary<string, ObservableCollection<string>> _errorsByPropertyNames;

        public ModelState(object model)
            : base(model)
        {
            PropertyChanged +=
                (sender, e) =>
                {
                    if (!nameof(IsValid).Equals(e.PropertyName, StringComparison.OrdinalIgnoreCase))
                        _ValidateModel();
                };

            _errorsByPropertyNames = model
                .GetType()
                .GetRuntimeProperties()
                .Select(runtimeProperty => runtimeProperty.Name)
                .Concat(new[] { string.Empty })
                .ToDictionary(
                    runtimePropertyName => runtimePropertyName,
                    runtimePropertyName => new ObservableCollection<string>(),
                    StringComparer.OrdinalIgnoreCase);
            Errors = _errorsByPropertyNames.ToDictionary(
                errorsByPropertyName => errorsByPropertyName.Key,
                errorsByPropertyName => new ReadOnlyObservableCollection<string>(errorsByPropertyName.Value),
                StringComparer.OrdinalIgnoreCase);

            _ValidateModel();
        }

        public IReadOnlyDictionary<string, ReadOnlyObservableCollection<string>> Errors
        {
            get;
        }

        public bool IsValid
            => Errors.Values.All(errors => !errors.Any());

        private void _ValidateModel()
        {
            _ClearAllErrors();
            foreach (var validationResultsByMemberName in _GetValidationResultsByMemberName())
                _AddRange(
                    _errorsByPropertyNames[validationResultsByMemberName.Key],
                    validationResultsByMemberName);

            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsValid)));
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
    }
}