using System;
using System.ComponentModel.DataAnnotations;

using CMS.Ecommerce;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DancingGoat.Models
{
    public class BillingAddressViewModel
    {
        [Required]
        [Display(Name = "Address line")]
        [MaxLength(100, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        public string BillingAddressLine1 { get; set; }



        [Display(Name = "Address line 2")]
        [MaxLength(100, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        public string BillingAddressLine2 { get; set; }


        [Required]
        [Display(Name = "City")]
        [MaxLength(100, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        public string BillingAddressCity { get; set; }


        [Required]
        [Display(Name = "Postal code")]
        [MaxLength(20, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        public string BillingAddressPostalCode { get; set; }
        

        public CountryStateViewModel BillingAddressCountryStateSelector { get; set; }


        public AddressSelectorViewModel BillingAddressSelector { get; set; }


        [BindNever]
        public SelectList Countries { get; set; }


        public string BillingAddressState { get; set; }


        public string BillingAddressCountry { get; set; }


        public BillingAddressViewModel()
        {
        }


        public BillingAddressViewModel(AddressInfo address, SelectList countries, CountryRepository countryRepository, SelectList addresses = null)
        {
            if (address != null)
            {
                if (countryRepository == null)
                {
                    throw new ArgumentNullException(nameof(countryRepository));
                }

                BillingAddressLine1 = address.AddressLine1;
                BillingAddressLine2 = address.AddressLine2;
                BillingAddressCity = address.AddressCity;
                BillingAddressPostalCode = address.AddressZip;
                BillingAddressState = countryRepository.GetState(address.AddressStateID)?.StateDisplayName ?? String.Empty;
                BillingAddressCountry = countryRepository.GetCountry(address.AddressCountryID)?.CountryDisplayName ?? String.Empty;
                Countries = countries;
            }

            BillingAddressCountryStateSelector = new CountryStateViewModel
            {
                Countries = countries,
                CountryID = address?.AddressCountryID ?? 0,
                StateID = address?.AddressStateID ?? 0
            };

            BillingAddressSelector = new AddressSelectorViewModel
            {
                Addresses = addresses,
                AddressID = address?.AddressID ?? 0
            };
        }


        public void ApplyTo(AddressInfo address)
        {
            address.AddressLine1 = BillingAddressLine1;
            address.AddressLine2 = BillingAddressLine2;
            address.AddressCity = BillingAddressCity;
            address.AddressZip = BillingAddressPostalCode;
            address.AddressCountryID = BillingAddressCountryStateSelector.CountryID;
            address.AddressStateID = BillingAddressCountryStateSelector.StateID ?? 0;
        }
    }
}