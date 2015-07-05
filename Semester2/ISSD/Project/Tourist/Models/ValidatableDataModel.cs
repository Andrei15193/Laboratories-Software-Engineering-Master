using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
namespace Tourist.Models
{
    [DataContract(Namespace = Constants.XmlNamespace)]
    public class ValidatableDataModel
        : INotifyDataErrorInfo
    {
        private IDictionary<string, ISet<string>> _assertErrorsByPropertyName;
        private IDictionary<string, INotifyDataErrorInfo> _validatableProperties;

        protected ValidatableDataModel()
        {
            _Initialize(default(StreamingContext));
        }

        public bool HasErrors
        {
            get
            {
                return _assertErrorsByPropertyName.Values.Any(Enumerable.Any);
            }
        }

        public IEnumerable<string> GetErrors(string propertyName)
        {
            if (propertyName == null)
                return _assertErrorsByPropertyName
                      .SelectMany(assertErrorsByProperty => assertErrorsByProperty
                                                           .Value
                                                           .Concat(_GetDataErrorsFor(assertErrorsByProperty.Key)));
            else
                return _GetAssertErrorsFor(propertyName).Concat(_GetDataErrorsFor(propertyName));
        }

        private IEnumerable<string> _GetDataErrorsFor(string propertyName)
        {
            INotifyDataErrorInfo dataErrorProvider;
            if (_validatableProperties.TryGetValue(propertyName, out dataErrorProvider))
                foreach (var dataError in dataErrorProvider.GetErrors(null))
                    yield return Convert.ToString(dataError);
        }

        private IEnumerable<string> _GetAssertErrorsFor(string propertyName)
        {
            ISet<string> propertyErrors;
            if (_assertErrorsByPropertyName.TryGetValue(propertyName, out propertyErrors))
                return propertyErrors;
            else
                return Enumerable.Empty<string>();
        }
        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            return GetErrors(propertyName);
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs dataErrorsChangedEventArgs)
        {
            var eventHandler = ErrorsChanged;
            if (eventHandler != null)
                eventHandler(this, dataErrorsChangedEventArgs);
        }

        protected void Assert(bool condition, string errorMessage, [CallerMemberName]string propertyName = null)
        {
            if (condition)
            {
                ISet<string> propertyErrors;
                if (_assertErrorsByPropertyName.TryGetValue(propertyName, out propertyErrors) && propertyErrors.Remove(errorMessage))
                    OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
            }
            else
            {
                ISet<string> propertyErrors;
                if (!_assertErrorsByPropertyName.TryGetValue(propertyName, out propertyErrors))
                {
                    propertyErrors = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
                    _assertErrorsByPropertyName.Add(propertyName, propertyErrors);
                }
                if (propertyErrors.Add(errorMessage))
                    OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
            }
        }
        protected void SubscribeValidatableProperty(INotifyDataErrorInfo validatableViewModelProperty, [CallerMemberName]string propertyName = null)
        {
            if (validatableViewModelProperty == null)
                throw new ArgumentNullException("validatableViewModelProperty");
            if (string.IsNullOrWhiteSpace(propertyName))
                if (propertyName == null)
                    throw new ArgumentNullException("propertyName");
                else
                    throw new ArgumentException("Cannot be empty or white space!", "propertyName");

            validatableViewModelProperty.ErrorsChanged += _ValidatableViewModelPropertyErrorsChanged;
            _validatableProperties[propertyName] = validatableViewModelProperty;
        }
        protected void UnsubscribeValidatableProperty([CallerMemberName]string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                if (propertyName == null)
                    throw new ArgumentNullException("propertyName");
                else
                    throw new ArgumentException("Cannot be empty or white space!", "propertyName");

            INotifyDataErrorInfo validatableViewModelProperty;
            if (_validatableProperties.TryGetValue(propertyName, out validatableViewModelProperty))
            {
                validatableViewModelProperty.ErrorsChanged -= _ValidatableViewModelPropertyErrorsChanged;
                _validatableProperties.Remove(propertyName);
            }
        }

        private void _ValidatableViewModelPropertyErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            var validatablePropertyName = _validatableProperties
                                         .SingleOrDefault(property => ReferenceEquals(property.Value, sender))
                                         .Key;
            if (!string.IsNullOrWhiteSpace(validatablePropertyName))
                OnErrorsChanged(new DataErrorsChangedEventArgs(validatablePropertyName));
        }

        [OnDeserializing]
        private void _Initialize(StreamingContext streamingContext)
        {
            _assertErrorsByPropertyName = new SortedDictionary<string, ISet<string>>(StringComparer.OrdinalIgnoreCase);
            _validatableProperties = new Dictionary<string, INotifyDataErrorInfo>(StringComparer.OrdinalIgnoreCase);
        }
    }
}