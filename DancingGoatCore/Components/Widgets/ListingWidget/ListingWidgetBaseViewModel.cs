using System.Collections.Generic;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// View model for the Listing widget.
    /// </summary>
    public class ListingWidgetBaseViewModel
    {
        /// <summary>
        /// Pages to show.
        /// </summary>
        public IEnumerable<ListingWidgetBasePageViewModel> Pages { get; set; }
    }
}
