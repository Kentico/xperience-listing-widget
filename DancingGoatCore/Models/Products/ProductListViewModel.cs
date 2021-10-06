using System.Collections.Generic;

namespace DancingGoat.Models
{
    public class ProductListViewModel
    {
        public IRepositoryFilter Filter { get; set; }

        public IEnumerable<ProductListItemViewModel> Items { get; set; }
    }
}