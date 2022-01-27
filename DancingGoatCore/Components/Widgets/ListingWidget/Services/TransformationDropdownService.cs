using System.Collections.Generic;
using System.Linq;

using DancingGoat.InlineEditors;

using Microsoft.Extensions.Localization;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Provides methods to get options and tooltip for transformation drop-down selector.
    /// </summary>
    public class TransformationDropDownService
    {
        private readonly SupportedTransformations supportedTransformations;
        private readonly IStringLocalizer<SharedResources> localizer;


        /// <summary>
        /// Creates an instance of <see cref="TransformationDropDownService"/> class.
        /// </summary>
        /// <param name="localizer">Represents an <see cref="IStringLocalizer"/> that provides localized strings.</param>
        /// <param name="supportedTransformations">Supported transformations.</param>
        public TransformationDropDownService(IStringLocalizer<SharedResources> localizer, SupportedTransformations supportedTransformations)
        {
            this.localizer = localizer;
            this.supportedTransformations = supportedTransformations;
        }


        /// <summary>
        /// Gets drop-down editor view model for transformation drop-down selector.
        /// </summary>
        /// <param name="selectedOption">Selected option from drop-down selector.</param>
        /// <param name="pageType">Page type.</param>
        public DropDownEditorViewModel GetDropDownModel(string selectedOption, string pageType)
        {
            var update = !string.IsNullOrEmpty(selectedOption) && !supportedTransformations.IsTransformationSupportedForPageType(selectedOption, pageType);
            return new DropDownEditorViewModel(nameof(ListingWidgetProperties.SelectedTransformationPath), GetOptions(pageType), selectedOption, "Transformation", GetTooltip(pageType), update);
        }


        private IEnumerable<DropDownOptionViewModel> GetOptions(string pageType)
        {
            if (string.IsNullOrEmpty(pageType) || !supportedTransformations.Transformations.ContainsKey(pageType))
            {
                return Enumerable.Empty<DropDownOptionViewModel>();
            }
            return supportedTransformations.Transformations[pageType].Select(transformation => new DropDownOptionViewModel(transformation.View, localizer[transformation.Name]));
        }


        private string GetTooltip(string pageType)
        {
            if (string.IsNullOrEmpty(pageType) || !supportedTransformations.Transformations.ContainsKey(pageType))
            {
                return string.Empty;
            }
            var tooltips = supportedTransformations.Transformations[pageType]
                .Select(transformation => GetTransformationTooltip(transformation))
                .Where(tooltip => !string.IsNullOrWhiteSpace(tooltip));
            var joinedTooltip = string.Join("\n\n", tooltips);
            return string.IsNullOrWhiteSpace(joinedTooltip) ? null : joinedTooltip;
        }


        private string GetTransformationTooltip(Transformation transformation)
        {
            if (transformation == null)
            {
                return null;
            }
            return string.IsNullOrWhiteSpace(transformation.ToolTip)
                ? null
                : $"{localizer[transformation.Name]}:\n{localizer[transformation.ToolTip]}";
        }
    }
}
