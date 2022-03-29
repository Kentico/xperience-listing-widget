using CMS.Ecommerce;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// View model for Product card widget.
    /// </summary>
    public class ProductCardViewModel
    {
        /// <summary>
        /// Card heading.
        /// </summary>
        public string Heading { get; set; } = "Please select a product.";


        /// <summary>
        /// Card background image path.
        /// </summary>
        public string ImagePath { get; set; }


        /// <summary>
        /// Card text.
        /// </summary>
        public string Text { get; set; }


        /// <summary>
        /// Gets ViewModel for <paramref name="product"/>.
        /// </summary>
        /// <param name="product">Product.</param>
        /// <returns>Hydrated ViewModel.</returns>
        public static ProductCardViewModel GetViewModel(SKUTreeNode product)
        {
            return product == null
                ? new ProductCardViewModel()
                : new ProductCardViewModel
                {
                    Heading = product.DocumentSKUName,
                    ImagePath = string.IsNullOrEmpty(product.SKU.SKUImagePath) ? null : new FileUrl(product.SKU.SKUImagePath, true).WithSizeConstraint(SizeConstraint.MaxWidthOrHeight(300)).RelativePath,
                    Text = product.DocumentSKUShortDescription
                };
        }
    }
}