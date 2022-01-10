using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace DancingGoat.InlineEditors
{
    /// <summary>
    /// View model for drop-down editor.
    /// </summary>
    public class DropdownEditorViewModel : InlineEditorViewModel
    {
        /// <summary>
        /// Selected option.
        /// </summary>
        public string SelectedOption { get; set; }


        /// <summary>
        /// Select list for all options of the selector.
        /// </summary>
        public SelectList Options { get; set; }


        /// <summary>
        /// Label of the selector.
        /// </summary>
        public string Label { get; set; }


        /// <summary>
        /// Creates an instance of <see cref="DropdownEditorViewModel"/> class.
        /// </summary>
        /// <param name="propertyName">Name of related widget property.</param>
        /// <param name="selectedOption">Selected option.</param>
        /// <param name="options">All options of the selector.</param> 
        /// <param name="label">Label of the selector.</param> 
        public DropdownEditorViewModel(string propertyName, string selectedOption, IEnumerable<DropdownOptionViewModel> options, string label)
        {
            PropertyName = propertyName;
            SelectedOption = selectedOption;
            Options = new SelectList(options, nameof(DropdownOptionViewModel.Value), nameof(DropdownOptionViewModel.Text));
            Label = label;
        }
    }
}
