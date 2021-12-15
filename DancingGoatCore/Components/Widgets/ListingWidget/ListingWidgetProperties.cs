using System.Collections.Generic;

using Kentico.PageBuilder.Web.Mvc;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Listing widget properties.
    /// </summary>
    public class ListingWidgetProperties : IWidgetProperties
    {
        /// <summary>
        /// All supported page types.
        /// </summary>
        public static readonly IEnumerable<string> SupportedPageTypes = new List<string> { "Article", "Cafe", "Product" };

        /// <summary>
        /// Name of the selected page type.
        /// </summary>
        public string SelectedPageType { get; set; }
    }
}
