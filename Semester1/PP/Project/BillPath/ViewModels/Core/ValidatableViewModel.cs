using System;
using System.Collections;
using System.ComponentModel;

namespace BillPath.ViewModels.Core
{
    public class ValidatableViewModel
        : ViewModel
    {
        protected ValidatableViewModel(INotifyDataErrorInfo validatable)
        {
            if (validatable == null)
                throw new ArgumentNullException("validatableModel");

            _validatable = validatable;
            _validatable.ErrorsChanged += delegate { OnPropertyChanged("ValidationErrors"); };
        }

        public IEnumerable ValidationErrors
        {
            get
            {
                return _validatable.GetErrors(string.Empty);
            }
        }

        private INotifyDataErrorInfo _validatable;
    }
}