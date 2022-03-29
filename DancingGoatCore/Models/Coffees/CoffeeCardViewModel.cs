using CMS.Ecommerce;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// View model for Coffee card.
    /// </summary>
    public class CoffeeCardViewModel
    {
        /// <summary>
        /// Coffee card heading.
        /// </summary>
        public string Heading { get; set; }


        /// <summary>
        /// Coffee card background image path.
        /// </summary>
        public string ImagePath { get; set; }


        /// <summary>
        /// Coffee card text.
        /// </summary>
        public string Text { get; set; }


        /// <summary>
        /// Coffee detail URL.
        /// </summary>
        public string Url { get; set; }


        /// <summary>
        /// Gets ViewModel for <paramref name="coffee"/>.
        /// </summary>
        /// <param name="coffee">Coffee.</param>
        /// <param name="pageUrlRetriever">Page URL retriever.</param>
        /// <returns>Hydrated ViewModel.</returns>
        public static CoffeeCardViewModel GetViewModel(SKUTreeNode coffee, IPageUrlRetriever pageUrlRetriever)
        {
            return coffee == null
                ? new CoffeeCardViewModel()
                : new CoffeeCardViewModel
                {
                    Heading = coffee.DocumentSKUName,
                    ImagePath = string.IsNullOrEmpty(coffee.SKU.SKUImagePath) ? null : new FileUrl(coffee.SKU.SKUImagePath, true).WithSizeConstraint(SizeConstraint.MaxWidthOrHeight(300)).RelativePath,
                    Text = coffee.DocumentSKUShortDescription,
                    Url = pageUrlRetriever.Retrieve(coffee).RelativePath
                };
        }
    }
}