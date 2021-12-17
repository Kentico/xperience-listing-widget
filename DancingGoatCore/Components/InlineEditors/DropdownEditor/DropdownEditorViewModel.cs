using System.Collections.Generic;

namespace DancingGoat.InlineEditors
{
    public class DropdownEditorViewModel : InlineEditorViewModel
    {
        /// <summary>
        /// Selected option.
        /// </summary>
        public string Selected { get; set; }


        /// <summary>
        /// All options of the selector.
        /// </summary>
        public IEnumerable<DropdownOptionViewModel> Options { get; set; }


        /// <summary>
        /// Label of the selector.
        /// </summary>
        public string Label { get; set; }
    }
}
