namespace Kentico.Xperience.ListingWidget.Widgets
{
    /// <summary>
    /// View model for Listing widget.
    /// </summary>
    public class ListingWidgetViewModel
    {
        /// <summary>
        /// View models for inline editors.
        /// </summary>
        public ListingWidgetInlineEditors InlineEditors { get; set; }


        /// <summary>
        /// Selected values from inline editors.
        /// </summary>
        public ListingWidgetSelectedValues SelectedValues { get; set; }
    }
}
