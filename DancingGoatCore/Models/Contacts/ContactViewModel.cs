using Microsoft.Extensions.Localization;

namespace DancingGoat.Models
{
    public class ContactViewModel
    {
        public string Name { get; set; }


        public string Phone { get; set; }


        public string Email { get; set; }


        public string ZIP { get; set; }


        public string Street { get; set; }


        public string City { get; set; }


        public string Country { get; set; }


        public string CountryCode { get; set; }


        public string State { get; set; }


        public string StateCode { get; set; }


        public ContactViewModel()
        {
        }


        public ContactViewModel(IContact contact)
        {
            Name = contact.Name;
            Phone = contact.Phone;
            Email = contact.Email;
            ZIP = contact.ZIP;
            Street = contact.Street;
            City = contact.City;
        }


        public static ContactViewModel GetViewModel(IContact contact, CountryRepository countryProvider, IStringLocalizer localizer)
        {
            var countryStateName = CountryStateName.Parse(contact.Country);
            var country = countryProvider.GetCountry(countryStateName.CountryName);
            var state = countryProvider.GetState(countryStateName.StateName);

            var model = new ContactViewModel(contact)
            {
                CountryCode = country.CountryTwoLetterCode,
                Country = localizer[country.CountryDisplayName]
            };

            if (state != null)
            {
                model.StateCode = state.StateName;
                model.State = localizer[state.StateDisplayName];
            }

            return model;
        }
    }
}