using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DancingGoat.Models
{
    public class CountryStateViewModel
    {
        [Required(ErrorMessage = "The Country field is required")]
        [Display(Name = "Country")]
        public int CountryID { get; set; }


        [Display(Name = "State")]
        [RegularExpression(@"[0-9]*$", ErrorMessage = "The State field is required")]
        public int? StateID { get; set; }


        [BindNever]
        public SelectList Countries { get; set; }
    }
}