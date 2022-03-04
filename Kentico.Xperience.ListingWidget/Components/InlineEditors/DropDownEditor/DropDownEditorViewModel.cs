using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kentico.Xperience.ListingWidget.InlineEditors
{
    /// <summary>
    /// View model for drop-down editor.
    /// </summary>
    public class DropDownEditorViewModel : InlineEditorViewModel
    {
        /// <summary>
        /// Select list for all options of the selector.
        /// </summary>
        public SelectList Options { get; set; }


        /// <summary>
        /// Selected option.
        /// </summary>
        public string SelectedOption { get; set; }


        /// <summary>
        /// Label of the selector.
        /// </summary>
        public string Label { get; set; }


        /// <summary>
        /// Tooltip of the selector.
        /// </summary>
        public string Tooltip { get; set; }


        /// <summary>
        /// If true drop-down will reset related widget property.
        /// </summary>
        public bool Reset { get; set; }


        /// <summary>
        /// Creates an instance of <see cref="DropdownEditorViewModel"/> class.
        /// </summary>
        /// <param name="propertyName">Name of related widget property.</param>
        /// <param name="options">All options of the selector with localized values.</param>
        /// <param name="selectedOption">Selected option.</param>
        /// <param name="label">Label of the selector.</param>
        /// <param name="tooltip">Localized tooltip of the selector.</param>
        /// <param name="reset">If true drop-down will reset related widget property.</param>
        public DropDownEditorViewModel(string propertyName, IEnumerable<DropDownOptionViewModel> options, string selectedOption, string label, string tooltip = "", bool reset = false)
        {
            PropertyName = propertyName;
            Options = new SelectList(options, nameof(DropDownOptionViewModel.Value), nameof(DropDownOptionViewModel.Text));
            SelectedOption = selectedOption;
            Label = label;
            Tooltip = tooltip;
            Reset = reset;
        }
    }
}
