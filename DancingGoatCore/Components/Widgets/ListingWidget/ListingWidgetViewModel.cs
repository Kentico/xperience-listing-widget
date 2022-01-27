namespace DancingGoat.Widgets
{
    /// <summary>
    /// View model for Listing widget.
    /// </summary>
    public class ListingWidgetViewModel
    {
        /// <summary>
        /// View models for drop-down inline editors.
        /// </summary>
        public ListingWidgetInlineEditorsViewModel EditorsModels { get; set; }


        /// <summary>
        /// Selected values from inline editors.
        /// </summary>
        public ListingWidgetSelectedValuesModel SelectedValues { get; set; }
    }
}
