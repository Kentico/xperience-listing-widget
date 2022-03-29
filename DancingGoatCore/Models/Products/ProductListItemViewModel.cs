using System;

using CMS.Ecommerce;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    public class ProductListItemViewModel
    {
        public ProductCatalogPrices PriceDetail { get; }


        public string Name { get; }


        public string ImagePath { get; }


        public string PublicStatusName { get; }


        public bool Available { get; }


        public Guid ProductPageGUID { get; }


        public string Url { get; }


        public string CategoryName { get; set; }


        public string ShortDescription { get; set; }


        public ProductListItemViewModel(SKUTreeNode productPage, ProductCatalogPrices priceDetail, string publicStatusName, IPageUrlRetriever pageUrlRetriever)
        {
            // Set page information
            Name = productPage.DocumentName;
            Url = pageUrlRetriever.Retrieve(productPage).RelativePath;

            // Set SKU information
            ImagePath = string.IsNullOrEmpty(productPage.SKU.SKUImagePath) ? null : new FileUrl(productPage.SKU.SKUImagePath, true).WithSizeConstraint(SizeConstraint.Size(452, 452)).RelativePath;
            Available = !productPage.SKU.SKUSellOnlyAvailable || productPage.SKU.SKUAvailableItems > 0;
            PublicStatusName = publicStatusName;

            // Set additional info
            PriceDetail = priceDetail;
            ShortDescription = productPage.DocumentSKUShortDescription;
        }
    }
}