using Kentico.Xperience.ListingWidget.InlineEditors;

namespace Kentico.Xperience.ListingWidget
{
    /// <summary>
    /// Models for inline editors of listing widget.
    /// </summary>
    public class ListingWidgetInlineEditors
    {
        /// <summary>
        /// View model for page type selector drop-down.
        /// </summary>
        public DropDownEditorViewModel PageTypeEditor { get; set; }


        /// <summary>
        /// View model for transformation selector drop-down.
        /// </summary>
        public DropDownEditorViewModel TransformationEditor { get; set; }


        /// <summary>
        /// View model for order by field selector drop-down.
        /// </summary>
        public DropDownEditorViewModel OrderByFieldEditor { get; set; }


        /// <summary>
        /// View model for topN inline editor.
        /// </summary>
        public TopNEditorViewModel TopNEditor { get; set; }


        /// <summary>
        /// View model for order direction inline editor.
        /// </summary>
        public OrderDirectionViewModel OrderDirectionEditor { get; set; }
    }
}
