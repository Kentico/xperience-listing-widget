using System.Collections.Generic;
using System.Linq;

using CMS.DataEngine;

using DancingGoat.InlineEditors;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Provides methods to get options for page type drop-down selector.
    /// </summary>
    public class PageTypesDropDownService
    {
        private readonly SupportedTransformations supportedTransformations;

        /// <summary>
        /// Creates an instance of <see cref="PageTypesDropDownService"/> class.
        /// </summary>
        /// <param name="supportedTransformations">Supported transformations.</param>
        public PageTypesDropDownService(SupportedTransformations supportedTransformations)
        {
            this.supportedTransformations = supportedTransformations;
        }


        /// <summary>
        /// Gets drop-down editor view model for page type drop-down selector.
        /// </summary>
        /// <param name="selectedOption">Selected option from drop-down selector.</param>
        public DropDownEditorViewModel GetDropDownModel(string selectedOption)
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
            return classes.Select(dataType => new DropDownOptionViewModel(dataType.Key, dataType.Value));
        }
    }
}
