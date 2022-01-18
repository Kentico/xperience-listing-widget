using System.Collections.Generic;
using System.Linq;

using CMS.DataEngine;
using CMS.FormEngine;

using DancingGoat.InlineEditors;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Provides methods to get data for order by field editor.
    /// </summary>
    internal class OrderByFieldService : IOrderByFieldService
    {
        /// <summary>
        /// Gets model for order by field drop-down editor.
        /// </summary>
        /// <param name="pageType">Page type for options to be retrieved.</param>
        /// <param name="orderByField">Order by field.</param>
        public DropdownEditorViewModel GetDropDownModel(string pageType, string orderByField)
        {
            var fields = GetFields(pageType);
            var validOrderByFieldSelected = fields.Any(item => item.Value == orderByField);
            return new DropdownEditorViewModel(nameof(ListingWidgetProperties.OrderByField),
                fields,
                validOrderByFieldSelected ? orderByField : string.Empty,
                "Order by field"
                );
        }

  
        /// <summary>
        /// Verifies that field could be selected for provided page type.
        /// </summary>
        /// <param name="pageType">Page type.</param>
        /// <param name="orderByField">Order by field.</param>
        public bool IsValidField(string pageType, string orderByField)
        {
            var fields = GetFields(pageType);
            return fields.Any(item => item.Value == orderByField);
        }


        private IEnumerable<DropdownOptionViewModel> GetFields(string pageType)
        {
            if (!string.IsNullOrEmpty(pageType))
            {
                var dataClassInfo = DataClassInfoProvider.GetDataClassInfo(pageType);

                var formInfo = new FormInfo(dataClassInfo.ClassFormDefinition);
                var fields = formInfo.GetFields(true, true);
                foreach (var field in fields)
                {
                    if (field.DataType == FieldDataType.Integer)
                    {
                        yield return new DropdownOptionViewModel(field.Name, field.GetDisplayName(null));
                    }
                    if (field.DataType == FieldDataType.Text)
                    {
                        if (field.GetControlName() == "TextBoxControl")
                        {
                            yield return new DropdownOptionViewModel(field.Name, field.GetDisplayName(null));
                        }
                    }
                }

                if (dataClassInfo.ClassUsePublishFromTo)
                {
                    yield return new DropdownOptionViewModel("DocumentPublishFrom", "DocumentPublishFrom");
                }
            }
            yield return new DropdownOptionViewModel("DocumentCreatedWhen", "DocumentCreatedWhen");
            yield return new DropdownOptionViewModel("NodeOrder", "NodeOrder");
        }
    }
}
