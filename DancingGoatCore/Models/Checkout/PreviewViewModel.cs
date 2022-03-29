namespace DancingGoat.Models
{
    public class PreviewViewModel
    {
        public DeliveryDetailsViewModel DeliveryDetails { get; set; }

        
        public CartViewModel CartModel { get; set; }


        public CustomerViewModel CustomerDetails => DeliveryDetails?.Customer;


        public BillingAddressViewModel BillingAddress => DeliveryDetails?.BillingAddress;
        

        public PaymentMethodViewModel PaymentMethod { get;  set; }


        public string ShippingName { get; set; }
    }
}