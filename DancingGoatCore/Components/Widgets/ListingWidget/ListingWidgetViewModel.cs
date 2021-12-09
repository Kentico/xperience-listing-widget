using System.Collections.Generic;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// View model for the Listing widget.
    /// </summary>
    public class ListingWidgetViewModel
    {
        /// <summary>
        /// Pages to show.
        /// </summary>
        public IEnumerable<ListingWidgetPageViewModel> Pages { get; set; }
    }
}
