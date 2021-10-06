using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    public class OrderItemViewModel
    {
        public int SKUID { get; set; }

        public string SKUImagePath { get; set; }

        public string SKUName { get; set; }

        public int UnitCount { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPriceInMainCurrency { get; set; }
    }
}