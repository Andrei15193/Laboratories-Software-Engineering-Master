using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public class ValidatableModel
        : INotifyDataErrorInfo
    {
        protected ValidatableModel()
        {
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add
            {
                _ValidationErrors.ErrorsChanged += value;
            }
            remove
            {
                _ValidationErrors.ErrorsChanged -= value;
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return _ValidationErrors;
            else
                return _ValidationErrors.Where(validationError => propertyName.Equals(validationError.PropertyName, StringComparison.Ordinal));
        }

        public bool HasErrors
        {
            get
            {
                return _ValidationErrors.Any();
            }
        }

        protected void AssertValidation(bool assertion, string propertyName, string errorId, Type declaringType = null)
        {
            _ValidationErrors.UpdateValidationEntry(declaringType ?? GetType(), propertyName, errorId, assertion);
        }
        protected bool HasValidationError(string propertyName, string errorId, Type declaringType = null)
        {
            return _validationErrors.HasValidationError(propertyName, errorId, declaringType ?? GetType());
        }

        private ValidationErrors _ValidationErrors
        {
            get
            {
                if (_validationErrors == null)
                    _validationErrors = new ValidationErrors();

                return _validationErrors;
            }
        }

        private ValidationErrors _validationErrors;

        private sealed class ValidationErrors
            : IEnumerable<ValidationError>
        {
            public void UpdateValidationEntry(Type objectType, string propertyName, string errorId, bool assertion = false)
            {
                bool hasChanged = false;

                if (!assertion)
                    hasChanged = _validationEntries.Add(new ValidationError(objectType, propertyName, errorId));
                else
                    hasChanged = _validationEntries.Remove(new ValidationError(objectType, propertyName, errorId));

                if (hasChanged)
                    _OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
            }

            public bool HasValidationError(string propertyName, string errorId, Type declaringType)
            {
                return _validationEntries.Contains(new ValidationError(declaringType, propertyName, errorId));
            }

            public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

            public IEnumerator<ValidationError> GetEnumerator()
            {
                return _validationEntries.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private void _OnErrorsChanged(DataErrorsChangedEventArgs eventArgs)
            {
                EventHandler<DataErrorsChangedEventArgs> eventHandler = ErrorsChanged;

                if (eventHandler != null)
                    eventHandler(this, eventArgs);
            }

            private ISet<ValidationError> _validationEntries = new SortedSet<ValidationError>(ValidationErrorComparer.Instance);

            private sealed class ValidationErrorComparer
                : IComparer<ValidationError>
            {
                public int Compare(ValidationError left, ValidationError right)
                {
                    int comparisonResult = StringComparer.Ordinal.Compare(left.PropertyName, right.PropertyName);

                    if (comparisonResult == 0)
                        comparisonResult = StringComparer.Ordinal.Compare(left.ErrorId, right.ErrorId);

                    return comparisonResult;
                }

                public static ValidationErrorComparer Instance
                {
                    get
                    {
                        return _instance;
                    }
                }

                private static ValidationErrorComparer _instance = new ValidationErrorComparer();
            }
        }
    }
}