using System.Collections.Generic;

using CMS.DocumentEngine.Types.DancingGoatCore;

namespace DancingGoat.Models
{
    public class ManufacturerDetailViewModel
    {
        public string Name { get; }


        public IEnumerable<ProductListItemViewModel> Products { get; }


        public ManufacturerDetailViewModel(Manufacturer manufacturer, IEnumerable<ProductListItemViewModel> products)
        {
            Name = manufacturer.DocumentName;
            Products = products;
        }
    }
}