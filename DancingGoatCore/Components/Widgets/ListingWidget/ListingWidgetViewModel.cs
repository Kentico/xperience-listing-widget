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
        /// View model for page type selector drop-down.
        /// </summary>
        public DropdownEditorViewModel PageTypeSelectorViewModel { get; set; }
    }
}
