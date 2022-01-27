using System.Collections.Generic;
using System.Linq;

using CMS.DataEngine;
using CMS.Helpers;

using DancingGoat.InlineEditors;

namespace DancingGoat.Widgets
{
    /// <inheritdoc/>
    public class PageTypesEditorService : IPageTypesEditorService
    {
        private readonly SupportedPageTypesRetriever pageTypesRetriever;


        /// <summary>
        /// Creates an instance of <see cref="PageTypesEditorService"/> class.
        /// </summary>
        /// <param name="supportedTransformations">Supported transformations.</param>
        public PageTypesEditorService(SupportedPageTypesRetriever pageTypesRetriever)
        {
            this.pageTypesRetriever = pageTypesRetriever;
        }


        /// <inheritdoc/>
        public DropDownEditorViewModel GetEditorModel(string selectedOption)
        {
            return new DropDownEditorViewModel(nameof(ListingWidgetProperties.PageType), GetOptions(), selectedOption, "Page type");
        }


        private IEnumerable<DropDownOptionViewModel> GetOptions()
        {
            var SupportedPageTypes = pageTypesRetriever.GetSupportedPageTypes();
            var classes = DataClassInfoProvider.GetClasses()
                .WhereIn("ClassName", SupportedPageTypes.ToList())
                .Columns("ClassName", "ClassDisplayName")
                .ToDictionary(c => c.ClassName, c => c.ClassDisplayName);
            return classes.Select(dataType => new DropDownOptionViewModel(dataType.Key, ResHelper.GetString(dataType.Value)));
        }
    }
}
