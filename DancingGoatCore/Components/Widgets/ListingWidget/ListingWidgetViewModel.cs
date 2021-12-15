using System.Collections.Generic;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// View model for Listing widget.
    /// </summary>
    public class ListingWidgetViewModel
    {
        /// <summary>
        /// Pages to show.
        /// </summary>
        public IEnumerable<ListingWidgetPageViewModel> Pages { get; set; }


        /// <summary>
        /// All supported page types.
        /// </summary>
        public IEnumerable<string> SupportedPageTypes { get; set; }


        /// <summary>
        /// Name of the selected page type to display.
        /// </summary>
        public string SelectedPageType { get; set; }
    }
}
