using System.ComponentModel.DataAnnotations;

namespace DancingGoat.Models
{
    public class CouponCodesUpdateModel
    {
        [MaxLength(200, ErrorMessage = "Maximum allowed length of the input text is {1}")]
        public string NewCouponCode { get; set; }


        public string RemoveCouponCode { get; set; }
    }
}