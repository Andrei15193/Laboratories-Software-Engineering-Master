using System;
using Tourist.Models;
namespace Tourist.ViewModels
{
    public class ContactDetailsViewModel
        : DataViewModel<ContactDetails>
    {
        public ContactDetailsViewModel(ContactDetails contactDetails)
            : base(contactDetails)
        {
            Address = GetPropertyViewModel("Address",
                                           () => DataModel.Address,
                                           value =>
                                           {
                                               if (string.IsNullOrWhiteSpace(value))
                                                   DataModel.Address = null;
                                               else
                                                   DataModel.Address = value;
                                           });
            PhoneNumber = GetPropertyViewModel("PhoneNumber",
                                               () => DataModel.PhoneNumber,
                                               value => DataModel.PhoneNumber = (string.IsNullOrWhiteSpace(value) ? null : value.Trim()));
            Website = GetPropertyViewModel("Website",
                                           () => DataModel.Website == null ? null : DataModel.Website.ToString(),
                                           value =>
                                           {
                                               try
                                               {
                                                   if (string.IsNullOrWhiteSpace(value))
                                                       DataModel.Website = null;
                                                   else
                                                       DataModel.Website = new Uri(value, UriKind.Absolute);
                                               }
                                               catch
                                               {
                                                   DataModel.Website = null;
                                                   throw;
                                               }
                                           },
                                           "The website must be a valid URL");
        }

        public IDataPropertyViewModel<string> Address
        {
            get;
            private set;
        }
        public IDataPropertyViewModel<string> Website
        {
            get;
            private set;
        }
        public IDataPropertyViewModel<string> PhoneNumber
        {
            get;
            private set;
        }
    }
}