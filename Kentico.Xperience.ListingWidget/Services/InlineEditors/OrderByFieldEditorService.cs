using System.Collections.Generic;
using System.Linq;

using CMS.DataEngine;
using CMS.FormEngine;
using CMS.Helpers;

using Kentico.Xperience.ListingWidget.InlineEditors;
using Kentico.Xperience.ListingWidget.Widgets;

namespace Kentico.Xperience.ListingWidget
{
    /// <summary>
    /// Provides methods to get data for order by field editor.
    /// </summary>
    internal class OrderByFieldEditorService : IOrderByFieldEditorService
    {
        /// <inheritdoc/>
        public DropDownEditorViewModel GetEditorModel(string pageType, string orderByField)
        {
            var fields = GetFields(pageType);
            var validOrderByFieldSelected = fields.Any(item => item.Value == orderByField);
            var reset = !string.IsNullOrEmpty(orderByField) && !validOrderByFieldSelected;
            return new DropDownEditorViewModel(nameof(ListingWidgetProperties.OrderByField),
                fields,
                validOrderByFieldSelected ? orderByField : string.Empty,
                "Order by field",
                reset: reset
                );
        }


        /// <inheritdoc/>
        public bool IsValidField(string pageType, string orderByField)
        {
            var fields = GetFields(pageType);
            return fields.Any(item => item.Value == orderByField);
        }


        private IEnumerable<DropDownOptionViewModel> GetFields(string pageType)
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
                        yield return new DropDownOptionViewModel(field.Name, ResHelper.LocalizeString(field.GetDisplayName(null), CultureHelper.DefaultUICultureCode));
                    }
                    if (field.DataType == FieldDataType.Text)
                    {
                        if (field.GetControlName() == "TextBoxControl")
                        {
                            yield return new DropDownOptionViewModel(field.Name, ResHelper.LocalizeString(field.GetDisplayName(null), CultureHelper.DefaultUICultureCode));
                        }
                    }
                }

                if (dataClassInfo.ClassUsePublishFromTo)
                {
                    yield return new DropDownOptionViewModel("DocumentPublishFrom", "DocumentPublishFrom");
                }
            }
            yield return new DropDownOptionViewModel("DocumentCreatedWhen", "DocumentCreatedWhen");
            yield return new DropDownOptionViewModel("NodeOrder", "NodeOrder");
        }
    }
}
