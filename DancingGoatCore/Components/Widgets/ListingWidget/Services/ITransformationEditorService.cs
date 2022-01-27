using DancingGoat.InlineEditors;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Provides methods to get options and tooltip for transformation drop-down selector.
    /// </summary>
    public interface ITransformationEditorService
    {
        /// <summary>
        /// Gets drop-down editor view model for transformation drop-down selector.
        /// </summary>
        /// <param name="selectedOption">Selected option from drop-down selector.</param>
        /// <param name="pageType">Page type.</param>
        DropDownEditorViewModel GetEditorModel(string selectedOption, string pageType);
    }
}