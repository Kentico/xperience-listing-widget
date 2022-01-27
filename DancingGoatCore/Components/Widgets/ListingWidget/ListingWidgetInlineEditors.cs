using DancingGoat.InlineEditors;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Models for inline editors of listing widget.
    /// </summary>
    public class ListingWidgetInlineEditors
    {
        /// <summary>
        /// View model for page type selector drop-down.
        /// </summary>
        public DropDownEditorViewModel PageTypeSelector { get; set; }


        /// <summary>
        /// View model for transformation selector drop-down.
        /// </summary>
        public DropDownEditorViewModel TransformationSelector { get; set; }


        /// <summary>
        /// View model for order by field selector drop-down.
        /// </summary>
        public DropDownEditorViewModel OrderFieldSelector { get; set; }


        /// <summary>
        /// View model for topN inline editor.
        /// </summary>
        public TopNEditorViewModel TopNEditor { get; set; }


        /// <summary>
        /// View model for order direction inline editor.
        /// </summary>
        public OrderDirectionViewModel OrderDirectionEditor { get; set; }


        /// <summary>
        /// Properties for path editor.
        /// </summary>
        public PathEditorProperties PathEditor { get; set; }
    }
}
