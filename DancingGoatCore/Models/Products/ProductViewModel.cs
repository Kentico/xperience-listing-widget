using System;
using System.Collections.Generic;

using CMS.Ecommerce;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    public class ProductViewModel
    {
        public ITypedProductViewModel TypedProduct { get; }


        public ProductCatalogPrices PriceDetail { get; }


        public IEnumerable<ProductOptionCategoryViewModel> ProductOptionCategories { get; }


        public int SelectedVariantID { get; set; }


        public bool IsInStock { get; }


        public bool AllowSale { get; }


        public string Name { get; }


        public string Description { get; }


        public string ShortDescription { get; }


        public int SKUID { get; }


        public string ImagePath { get; }


        public ProductViewModel(SKUTreeNode productPage, ProductCatalogPrices priceDetail,
            ITypedProductViewModel typedProductViewModel = null)
        {
            // Set page information
            Name = productPage.DocumentName;
            Description = productPage.DocumentSKUDescription;
            ShortDescription = productPage.DocumentSKUShortDescription;

            // Set SKU information
            var sku = productPage.SKU;
            SKUID = sku.SKUID;
            ImagePath = string.IsNullOrEmpty(sku.SKUImagePath) ? null : new FileUrl(sku.SKUImagePath, true).WithSizeConstraint(SizeConstraint.MaxWidthOrHeight(400)).RelativePath;
            IsInStock = sku.SKUTrackInventory == TrackInventoryTypeEnum.Disabled ||
                        sku.SKUAvailableItems > 0;
            AllowSale = IsInStock || !sku.SKUSellOnlyAvailable;

            // Set additional info
            TypedProduct = typedProductViewModel;
            PriceDetail = priceDetail;
        }


        public ProductViewModel(SKUTreeNode productPage, ProductCatalogPrices price,
            ITypedProductViewModel typedProductViewModel, ProductVariant defaultVariant,
            IEnumerable<ProductOptionCategoryViewModel> categories)
            : this(productPage, price, typedProductViewModel)
        {
            if (defaultVariant == null)
            {
                throw new ArgumentNullException(nameof(defaultVariant));
            }

            var variant = defaultVariant.Variant;

            IsInStock = ((variant.SKUTrackInventory == TrackInventoryTypeEnum.Disabled) || (variant.SKUAvailableItems > 0));
            AllowSale = IsInStock || !variant.SKUSellOnlyAvailable;

            SelectedVariantID = variant.SKUID;

            // Variant categories which will be rendered
            ProductOptionCategories = categories;
        }
    }
}