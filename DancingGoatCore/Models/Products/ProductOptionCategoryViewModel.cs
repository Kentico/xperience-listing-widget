using System.Collections.Generic;
using System.Linq;

using CMS.Ecommerce;


namespace DancingGoat.Models
{
    public class ProductOptionCategoryViewModel
    {
        private readonly OptionCategoryInfo Category;


        public string Name => string.IsNullOrEmpty(Category.CategoryLiveSiteDisplayName) ?
            Category.CategoryDisplayName :
            Category.CategoryLiveSiteDisplayName;


        public OptionCategorySelectionTypeEnum SelectionType => Category.CategorySelectionType;


        public IEnumerable<SKUInfo> Options { get; }


        public int SelectedOptionId { get; set; }


        public ProductOptionCategoryViewModel(int skuID, int selectedOptionID, OptionCategoryInfo category, ISKUInfoProvider skuInfoProvider)
        {
            SelectedOptionId = selectedOptionID;
            Category = category;
            Options = GetOptions(skuID, category.CategoryID, skuInfoProvider);
        }


        private IEnumerable<SKUInfo> GetOptions(int skuID, int categoryID, ISKUInfoProvider skuInfoProvider)
        {
            // Get all variant's options
            var variantOptionIDs = VariantOptionInfo.Provider.Get()
                                                            .WhereIn("VariantSKUID", VariantHelper.GetVariants(skuID).Column("SKUID"))
                                                            .Column("OptionSKUID");

            var variantOptionsList = skuInfoProvider.Get()
                                                    .WhereIn("SKUID", variantOptionIDs)
                                                    .OrderBy("SKUOrder")
                                                    .ToList();

            // Create option categories with selectable variant options
            return variantOptionsList.Where(o => o.SKUOptionCategoryID == categoryID);
        }
    }
}