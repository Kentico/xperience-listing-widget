using System.Collections.Generic;
using System.Linq;

using CMS.DataEngine;
using CMS.Helpers;

using DancingGoat.InlineEditors;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class PageTypesEditorService : IPageTypesEditorService
    {
        private readonly SupportedTransformations supportedTransformations;

        /// <summary>
        /// Creates an instance of <see cref="PageTypesEditorService"/> class.
        /// </summary>
        /// <param name="supportedTransformations">Supported transformations.</param>
        public PageTypesEditorService(SupportedTransformations supportedTransformations)
        {
            this.supportedTransformations = supportedTransformations;
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public DropDownEditorViewModel GetEditorModel(string selectedOption)
        {
            return new DropDownEditorViewModel(nameof(ListingWidgetProperties.SelectedPageType), GetOptions(), selectedOption, "Page type");
        }


        private IEnumerable<DropDownOptionViewModel> GetOptions()
        {
            var SupportedPageTypes = supportedTransformations.Transformations.Keys;
            var classes = DataClassInfoProvider.GetClasses()
                .WhereIn("ClassName", SupportedPageTypes)
                .Columns("ClassName", "ClassDisplayName")
                .ToDictionary(c => c.ClassName, c => c.ClassDisplayName);
            return classes.Select(dataType => new DropDownOptionViewModel(dataType.Key, ResHelper.GetString(dataType.Value)));
        }
    }
}
