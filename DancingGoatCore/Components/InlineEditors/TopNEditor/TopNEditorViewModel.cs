namespace DancingGoat.InlineEditors
{
    /// <summary>
    /// View model for top N editor.
    /// </summary>
    public class TopNEditorViewModel : InlineEditorViewModel
    {
        /// <summary>
        /// Number of items to retrieve.
        /// </summary>
        public int TopN { get; set; }
    }
}
