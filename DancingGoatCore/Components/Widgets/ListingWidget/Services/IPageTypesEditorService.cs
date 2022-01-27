using DancingGoat.InlineEditors;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Provides methods to get options for page type drop-down selector.
    /// </summary>
    public interface IPageTypesEditorService
    {
        /// <summary>
        /// Gets drop-down editor view model for page type drop-down selector.
        /// </summary>
        /// <param name="selectedOption">Selected option from drop-down selector.</param>
        DropDownEditorViewModel GetEditorModel(string selectedOption);
    }
}