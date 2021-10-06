using System.ComponentModel.DataAnnotations;
using System.Linq;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace DancingGoat.Models
{
    public class AddressSelectorViewModel
    {
        [Display(Name = "Address")]
        public int? AddressID { get; set; }
        

        public SelectList Addresses { get; set; }


        public bool HasSomeAddress => (Addresses != null) && Addresses.Any();
    }
}