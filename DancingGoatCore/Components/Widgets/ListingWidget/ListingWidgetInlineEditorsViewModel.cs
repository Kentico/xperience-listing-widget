using DancingGoat.InlineEditors;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// View model for inline editors of listing widget.
    /// </summary>
    public class ListingWidgetInlineEditorsViewModel
    {
        /// <summary>
        /// View model for page type selector drop-down.
        /// </summary>
        public DropDownEditorViewModel PageTypeSelectorViewModel { get; set; }


        /// <summary>
        /// View model for transformation selector drop-down.
        /// </summary>
        public DropDownEditorViewModel TransformationSelectorViewModel { get; set; }


        /// <summary>
        /// View model for order by field selector drop-down.
        /// </summary>
        public DropDownEditorViewModel OrderFieldSelectorViewModel { get; set; }
    }
}
