using Kentico.Xperience.ListingWidget.InlineEditors;

namespace Kentico.Xperience.ListingWidget
{
    /// <summary>
    /// Provides methods to get data for transformation editor.
    /// </summary>
    public interface ITransformationEditorService
    {
        /// <summary>
        /// Gets drop-down editor view model for transformation editor.
        /// </summary>
        /// <param name="selectedOption">Selected option from drop-down editor.</param>
        /// <param name="pageType">Page type.</param>
        DropDownEditorViewModel GetEditorModel(string selectedOption, string pageType);
    }
}