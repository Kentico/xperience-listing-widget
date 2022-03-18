namespace Kentico.Xperience.ListingWidget
{
    /// <summary>
    /// View model for listing widget.
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
