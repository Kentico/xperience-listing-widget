using System.Collections.Generic;

using DancingGoat.InlineEditors;

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
        public IEnumerable<DropdownOptionViewModel> SupportedPageTypes { get; set; }


        /// <summary>
        /// Name of the selected page type to display.
        /// </summary>
        public string SelectedPageType { get; set; }
    }
}
