using System.ComponentModel.DataAnnotations;

using CMS.Ecommerce;

namespace DancingGoat.Models
{
    public class CustomerViewModel
    {
        [Required]
        [Display(Name = "First name")]
        [MaxLength(100, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        public string FirstName { get; set; }


        [Required]
        [Display(Name = "Last name")]
        [MaxLength(100, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Please enter your email")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [MaxLength(100, ErrorMessage = "The maximum length of email address is {1} characters.")]
        public string Email { get; set; }


        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(26, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        public string PhoneNumber { get; set; }


        [Display(Name = "Company")]
        [MaxLength(200, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        public string Company { get; set; }


        [Display(Name = "Organization ID")]
        [MaxLength(50, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        public string OrganizationID { get; set; }


        [Display(Name = "Tax registration ID")]
        [MaxLength(50, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        public string TaxRegistrationID { get; set; }


        public bool IsCompanyAccount { get; set; }


        public CustomerViewModel(CustomerInfo customer)
        {
            if (customer == null)
            {
                return;
            }

            FirstName = customer.CustomerFirstName;
            LastName = customer.CustomerLastName;
            Email = customer.CustomerEmail;
            PhoneNumber = customer.CustomerPhone;
            Company = customer.CustomerCompany;
            OrganizationID = customer.CustomerOrganizationID;
            TaxRegistrationID = customer.CustomerTaxRegistrationID;
            IsCompanyAccount = customer.CustomerHasCompanyInfo;
        }


        public CustomerViewModel()
        {
        }


        public void ApplyToCustomer(CustomerInfo customer, bool emailCanBeChanged)
        {
            customer.CustomerFirstName = FirstName;
            customer.CustomerLastName = LastName;
            customer.CustomerPhone = PhoneNumber;
            customer.CustomerCompany = Company;
            customer.CustomerOrganizationID = OrganizationID;
            customer.CustomerTaxRegistrationID = TaxRegistrationID;

            if (emailCanBeChanged)
            {
                customer.CustomerEmail = Email;
            }
        }
    }
}