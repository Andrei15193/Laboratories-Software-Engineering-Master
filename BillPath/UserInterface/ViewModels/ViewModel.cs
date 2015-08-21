using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs == null)
                throw new ArgumentNullException(nameof(propertyChangedEventArgs));

            PropertyChanged?.Invoke(this, propertyChangedEventArgs);
        }
    }

    public class ViewModel<TModel>
        : ViewModel, INotifyDataErrorInfo
    {
        private class PropertyValidator
        {
            private readonly PropertyInfo _propertyInfo;
            private readonly string _displayName;
            private readonly IEnumerable<ValidationAttribute> _validationAttributes;

            public PropertyValidator(PropertyInfo propertyInfo)
            {
                if (propertyInfo == null)
                    throw new ArgumentNullException(nameof(propertyInfo));

                _propertyInfo = propertyInfo;
                _displayName = propertyInfo.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? propertyInfo.Name;
                _validationAttributes = propertyInfo.GetCustomAttributes<ValidationAttribute>(true).ToList();
            }

            public IEnumerable<ValidationResult> Validate(object instance)
            {
                if (instance == null)
                    throw new ArgumentNullException(nameof(instance));

                var propertyValue = _propertyInfo.GetValue(instance);
                var validationContext = new ValidationContext(instance)
                {
                    DisplayName = _displayName,
                    MemberName = _propertyInfo.Name
                };

                var validationResults = ValidateProperty(propertyValue, validationContext);
                if (validationResults.Any())
                    return validationResults;

                return ValidateInstance(instance, validationContext);
            }

            private IEnumerable<ValidationResult> ValidateProperty(object propertyValue, ValidationContext validationContext)
            {
                return from validationAttribute in _validationAttributes
                       let validationResult = validationAttribute.GetValidationResult(propertyValue, validationContext)
                       where validationResult != null
                       select validationResult;
            }

            private IEnumerable<ValidationResult> ValidateInstance(object instance, ValidationContext validationContext)
            {
                var validatableObject = instance as IValidatableObject;
                if (validatableObject == null)
                    return Enumerable.Empty<ValidationResult>();

                return from validationResult in validatableObject.Validate(validationContext)
                       where validationResult.MemberNames.Contains(_propertyInfo.Name, StringComparer.OrdinalIgnoreCase)
                       select validationResult;
            }
        }

        private static readonly IReadOnlyDictionary<string, PropertyValidator> _propertyValidatorsByPropertyName =
            typeof(TModel).GetRuntimeProperties().ToDictionary(
                property => property.Name,
                property => new PropertyValidator(property),
                StringComparer.OrdinalIgnoreCase);
        private readonly IDictionary<string, ISet<string>> _errorMessagesByPropertyName;

        public ViewModel(TModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Model = model;
            _errorMessagesByPropertyName = _propertyValidatorsByPropertyName.Keys.ToDictionary(
                propertyName => propertyName,
                delegate { return (ISet<string>)new SortedSet<string>(StringComparer.CurrentCulture); },
                StringComparer.OrdinalIgnoreCase);

            foreach (var propertyName in _propertyValidatorsByPropertyName.Keys)
                _ValidateWithoutDependencies(propertyName);
        }

        public TModel Model
        {
            get;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            _ValidateWithDependencies(propertyChangedEventArgs.PropertyName);
            base.OnPropertyChanged(propertyChangedEventArgs);
        }
        private void _ValidateWithoutDependencies(string propertyName)
        {
            var errorMessages = _errorMessagesByPropertyName[propertyName];
            errorMessages.Clear();

            foreach (var validationResult in _propertyValidatorsByPropertyName[propertyName].Validate(Model))
                errorMessages.Add(validationResult.ErrorMessage);

            OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
        }
        private void _ValidateWithDependencies(string propertyName)
        {
            var propertiesToValidate = new Queue<string>(new[] { propertyName });
            var enlistedProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { propertyName };

            do
            {
                var propertyToValidate = propertiesToValidate.Dequeue();
                var errorMessages = _errorMessagesByPropertyName[propertyToValidate];
                errorMessages.Clear();

                foreach (var validationResult in _propertyValidatorsByPropertyName[propertyToValidate].Validate(Model))
                {
                    errorMessages.Add(validationResult.ErrorMessage);
                    foreach (var memberName in validationResult.MemberNames)
                        if (enlistedProperties.Add(memberName))
                            propertiesToValidate.Enqueue(memberName);
                }
            } while (propertiesToValidate.Any());

            _RaiseErrorsChangedFor(enlistedProperties);
        }

        public bool HasErrors
        {
            get
            {
                return _errorMessagesByPropertyName.Values.Any(Enumerable.Any);
            }
        }

        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            return GetErrors(propertyName);
        }
        public IEnumerable<string> GetErrors(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return _errorMessagesByPropertyName.Values.SelectMany(Enumerable.AsEnumerable);
            else
                return _errorMessagesByPropertyName[propertyName];
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs dataErrorsChangedEventArgs)
        {
            ErrorsChanged?.Invoke(this, dataErrorsChangedEventArgs);
        }
        private void _RaiseErrorsChangedFor(IEnumerable<string> propertyNames)
        {
            foreach (var propertyName in propertyNames)
                OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
        }
    }
}