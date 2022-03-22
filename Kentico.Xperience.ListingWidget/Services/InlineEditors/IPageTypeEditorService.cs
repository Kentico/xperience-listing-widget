using Kentico.Xperience.ListingWidget.InlineEditors;

namespace Kentico.Xperience.ListingWidget
{
    /// <summary>
    /// Provides methods to get data for page type editor.
    /// </summary>
    public interface IPageTypeEditorService
    {
        /// <summary>
        /// Gets drop-down editor view model for page type editor.
        /// </summary>
        /// <param name="selectedOption">Selected option from drop-down editor.</param>
        DropDownEditorViewModel GetEditorModel(string selectedOption);
    }
}