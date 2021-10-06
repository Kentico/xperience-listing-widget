using System;

using CMS.Ecommerce;
using CMS.Globalization;

namespace DancingGoat.Models
{
    public class OrderAddressViewModel
    {
        public string AddressLine1 { get; set; }


        public string AddressLine2 { get; set; }


        public string AddressCity { get; set; }


        public string AddressPostalCode { get; set; }


        public string AddressState { get; set; }


        public string AddressCountry { get; set; }


        public OrderAddressViewModel()
        {
        }


        public OrderAddressViewModel(OrderAddressInfo address, ICountryInfoProvider countryInfoProvider, IStateInfoProvider stateInfoProvider)
        {
            if (address == null)
            {
                return;
            }

            if (countryInfoProvider == null)
            {
                throw new ArgumentNullException(nameof(countryInfoProvider));
            }
            if (stateInfoProvider == null)
            {
                throw new ArgumentNullException(nameof(stateInfoProvider));
            }

            AddressLine1 = address.AddressLine1;
            AddressLine2 = address.AddressLine2;
            AddressCity = address.AddressCity;
            AddressPostalCode = address.AddressZip;
            AddressState = stateInfoProvider.Get(address.AddressStateID)?.StateDisplayName ?? String.Empty;
            AddressCountry = countryInfoProvider.Get(address.AddressCountryID)?.CountryDisplayName ?? String.Empty;
        }
    }
}