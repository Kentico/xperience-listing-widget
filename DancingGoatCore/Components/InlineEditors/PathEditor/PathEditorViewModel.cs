namespace DancingGoat.InlineEditors
{
    /// <summary>
    /// View model for Path editor.
    /// </summary>
    public class PathEditorViewModel : InlineEditorViewModel
    {
        /// <summary>
        /// Value shown in Path editor.
        /// </summary>
        public string Value{ get; set; }


        /// <summary>
        /// Tooltip of value in Path editor.
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// Specifies page selection state.
        /// </summary>
        public PageSelectionState PageSelectionState { get; set; }
    }
}
