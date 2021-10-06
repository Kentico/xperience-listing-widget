using CMS.DocumentEngine.Types.DancingGoatCore;

namespace DancingGoat.Models
{
    public class FilterPackViewModel : ITypedProductViewModel
    {
        public int Quantity { get; set; }

        public static FilterPackViewModel GetViewModel(FilterPack filterPack)
        {
            return new FilterPackViewModel
            {
                Quantity = filterPack.FilterPackQuantity
            };
        }
    }
}