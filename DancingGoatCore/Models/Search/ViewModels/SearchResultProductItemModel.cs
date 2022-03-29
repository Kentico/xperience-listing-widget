using CMS.Ecommerce;
using CMS.Helpers;
using CMS.Search;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    public class SearchResultProductItemModel : SearchResultPageItemModel
    {
        public string Description { get; set; }


        public string ShortDescription { get; set; }


        public ProductCatalogPrices PriceDetail { get; set; }


        public SearchResultProductItemModel(SearchResultItem resultItem, SKUTreeNode page, ProductCatalogPrices priceDetail, IPageUrlRetriever pageUrlRetriever)
            : base(resultItem, page, pageUrlRetriever)
        {
            Description = page.DocumentSKUDescription;
            ShortDescription = HTMLHelper.StripTags(page.DocumentSKUShortDescription, false);
            PriceDetail = priceDetail;
        }
    }
}