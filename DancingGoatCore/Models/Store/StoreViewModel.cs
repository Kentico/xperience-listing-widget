using System.Collections.Generic;

namespace DancingGoat.Models
{
    public class StoreViewModel
    {
        public IEnumerable<ProductListItemViewModel> FeaturedProducts { get; set; }

        public IEnumerable<ProductListItemViewModel> HotTipProducts { get; set; }

        public IEnumerable<ManufactureListItemViewModel> Manufacturers { get; set; }
    }
}