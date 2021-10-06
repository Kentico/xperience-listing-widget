using System.ComponentModel.DataAnnotations;

using CMS.Ecommerce;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DancingGoat.Models
{
    public class ShippingOptionViewModel
    {
        [Display(Name = "Delivery method")]
        public int ShippingOptionID { get; set; }


        [BindNever]
        public SelectList ShippingOptions { get; set; }


        public bool IsVisible { get; set; } = true;


        public ShippingOptionViewModel()
        {
        }


        public ShippingOptionViewModel(ShippingOptionInfo shippingOption, SelectList shippingOptions, bool isVisible = true)
        {
            ShippingOptions = shippingOptions;
            ShippingOptionID = shippingOption?.ShippingOptionID ?? 0;
            IsVisible = isVisible;
        }
    }
}