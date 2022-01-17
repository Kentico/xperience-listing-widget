using CMS.DataEngine;

using DancingGoat.InlineEditors;

using Kentico.PageBuilder.Web.Mvc;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Listing widget properties.
    /// </summary>
    public class ListingWidgetProperties : IWidgetProperties
    {
        /// <summary>
        /// Name of the selected page type.
        /// </summary>
        public string SelectedPageType { get; set; }


        /// <summary>
        /// Page from which to retrieve child pages excluding the selected page.
        /// </summary>
        public SelectedPage SelectedPage { get; set; }


        /// <summary>
        /// Order direction of pages.
        /// </summary>
        public OrderDirection OrderDirection { get; set; } = OrderDirection.Ascending;

    }
}
