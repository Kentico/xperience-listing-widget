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
        /// Path of the selected transformation view.
        /// </summary>
        public string SelectedTransformationPath { get; set; }


        /// <summary>
        /// Page from which to retrieve child pages excluding the selected page.
        /// </summary>
        public SelectedPage SelectedPage { get; set; }


        /// <summary>
        /// Number of pages to retrieve.
        /// </summary>
        public int TopN { get; set; } = 10;


        /// <summary>
        /// Field to order by retrieved pages.
        /// </summary>
		public string OrderByField { get; set; }


        /// <summary>
        /// Order direction of pages.
        /// </summary>
        public OrderDirection OrderDirection { get; set; } = OrderDirection.Ascending;
    }
}
