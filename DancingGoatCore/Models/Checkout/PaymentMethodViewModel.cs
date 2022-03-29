using System.ComponentModel.DataAnnotations;

using CMS.Ecommerce;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DancingGoat.Models
{
    public class PaymentMethodViewModel
    {
        [Required(ErrorMessage = "Select payment method")]
        [Display(Name = "How would you like to pay?")]
        public int PaymentMethodID { get; set; }


        [BindNever]
        public SelectList PaymentMethods { get; set; }


        public PaymentMethodViewModel()
        {
        }


        public PaymentMethodViewModel(PaymentOptionInfo paymentMethod, SelectList paymentMethods)
        {
            PaymentMethods = paymentMethods;

            if (paymentMethod != null)
            {
                PaymentMethodID = paymentMethod.PaymentOptionID;
            }
        }
    }
}