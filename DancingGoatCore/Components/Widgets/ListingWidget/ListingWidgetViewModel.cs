﻿using System.Collections.Generic;

using CMS.DataEngine;

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
        /// Page from which to retrieve child pages excluding the selected page.
        /// </summary>
        public SelectedPage SelectedPage { get; set; }



        /// <summary>
        /// View model for page type selector drop-down.
        /// </summary>
        public DropdownEditorViewModel PageTypeSelectorViewModel { get; set; }


        /// <summary>
        /// Order direction of pages.
        /// </summary>
        public OrderDirection OrderDirection { get; set; }
    }
}
