using System.ComponentModel.DataAnnotations;

namespace DancingGoat.Models
{
    public class CartItemUpdateModel
    {
        public int ID { get; set; }


        public int SKUID { get; set; }


        [Range(0, int.MaxValue, ErrorMessage = "Type a number greater than 0 to the Units field.")]
        public int Units { get; set; }
    }
}