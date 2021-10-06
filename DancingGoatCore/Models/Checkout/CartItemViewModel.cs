using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    public class CartItemViewModel
    {
        public int SKUID { get; set; }

        public string SKUImagePath { get; set; }

        public string SKUName { get; set; }

        public int CartItemID { get; set; }

        public int CartItemUnits { get; set; }

        public decimal TotalPrice { get; set; }
    }
}