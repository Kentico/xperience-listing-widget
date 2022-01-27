using CMS.DataEngine;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Model with properties selected from inline editors of listing widget.
    /// </summary>
    public class ListingWidgetSelectedValues
    {
        /// <summary>
        /// Name of the selected page type to display.
        /// </summary>
        public string PageType { get; set; }


        /// <summary>
        /// Path of the selected transformation view.
        /// </summary>
        public string TransformationPath { get; set; }


        /// <summary>
        /// Page from which to retrieve child pages excluding the selected page.
        /// </summary>
        public string ParentPageAliasPath { get; set; }


        /// <summary>
        /// Field to order by retrieved pages.
        /// </summary>
        public string OrderByField { get; set; }


        /// <summary>
        /// Number of pages to retrieve.
        /// </summary>
        public int TopN { get; set; }


        /// <summary>
        /// Order direction of pages.
        /// </summary>
        public OrderDirection OrderDirection { get; set; }
    }
}
