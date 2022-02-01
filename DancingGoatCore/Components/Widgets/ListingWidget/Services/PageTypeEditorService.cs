using System.Collections.Generic;
using System.Linq;

using CMS.DataEngine;
using CMS.Helpers;

using DancingGoat.InlineEditors;

namespace DancingGoat.Widgets
{
    /// <inheritdoc/>
    internal class PageTypeEditorService : IPageTypeEditorService
    {
        private readonly ListingWidgetPageTypesRetriever pageTypesRetriever;


        /// <summary>
        /// Creates an instance of <see cref="PageTypeEditorService"/> class.
        /// </summary>
        /// <param name="supportedTransformations">Supported transformations.</param>
        public PageTypeEditorService(ListingWidgetPageTypesRetriever pageTypesRetriever)
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
            var SupportedPageTypes = pageTypesRetriever.Retrieve();
            var classes = DataClassInfoProvider.GetClasses()
                .WhereIn("ClassName", SupportedPageTypes.ToList())
                .Columns("ClassName", "ClassDisplayName")
                .ToDictionary(c => c.ClassName, c => c.ClassDisplayName);
            return classes.Select(dataType => new DropDownOptionViewModel(dataType.Key, ResHelper.GetString(dataType.Value)));
        }
    }
}
