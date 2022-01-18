using DancingGoat.InlineEditors;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Provides methods to get data for order by field editor.
    /// </summary>
    public interface IOrderByFieldService
    {
        /// <summary>
        /// Gets model for order by field drop-down editor.
        /// </summary>
        /// <param name="pageType">Page type for options to be retrieved.</param>
        /// <param name="orderByField">Order by field.</param>
        DropdownEditorViewModel GetDropDownModel(string pageType, string orderByField);


        /// <summary>
        /// Verifies that field could be selected for provided page type.
        /// </summary>
        /// <param name="pageType">Page type.</param>
        /// <param name="orderByField">Order by field.</param>
        bool IsValidField(string pageType, string orderByField);
    }
}
