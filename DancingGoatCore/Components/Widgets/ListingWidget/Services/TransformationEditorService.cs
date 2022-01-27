using System.Collections.Generic;
using System.Linq;

using DancingGoat.InlineEditors;

using Microsoft.Extensions.Localization;

namespace DancingGoat.Widgets
{
    /// <inheritdoc/>
    public class TransformationEditorService : ITransformationEditorService
    {
        private readonly SupportedTransformationsRetriever transformationsRetriever;
        private readonly IStringLocalizer<SharedResources> localizer;


        /// <summary>
        /// Creates an instance of <see cref="TransformationEditorService"/> class.
        /// </summary>
        /// <param name="localizer">Represents an <see cref="IStringLocalizer"/> that provides localized strings.</param>
        /// <param name="transformationsRetriever">Supported transformations.</param>
        public TransformationEditorService(IStringLocalizer<SharedResources> localizer, SupportedTransformationsRetriever transformationsRetriever)
        {
            this.localizer = localizer;
            this.transformationsRetriever = transformationsRetriever;
        }


        /// <inheritdoc/>
        public DropDownEditorViewModel GetEditorModel(string selectedOption, string pageType)
        {
            var reset = !string.IsNullOrEmpty(selectedOption) && !transformationsRetriever.IsTransformationSupported(selectedOption, pageType);
            return new DropDownEditorViewModel(nameof(ListingWidgetProperties.TransformationPath), GetOptions(pageType), selectedOption, "Transformation", GetTooltip(pageType), reset);
        }


        private IEnumerable<DropDownOptionViewModel> GetOptions(string pageType)
        {
            if (string.IsNullOrEmpty(pageType))
            {
                return Enumerable.Empty<DropDownOptionViewModel>();
            }
            return transformationsRetriever.GetTransformations(pageType).Select(transformation => new DropDownOptionViewModel(transformation.View, localizer[transformation.Name]));
        }


        private string GetTooltip(string pageType)
        {
            if (string.IsNullOrEmpty(pageType))
            {
                return string.Empty;
            }
            var tooltips = transformationsRetriever.GetTransformations(pageType)
                .Select(transformation => GetTransformationTooltip(transformation))
                .Where(tooltip => !string.IsNullOrWhiteSpace(tooltip));
            var joinedTooltip = string.Join("\n\n", tooltips);
            return string.IsNullOrWhiteSpace(joinedTooltip) ? null : joinedTooltip;
        }


        private string GetTransformationTooltip(Transformation transformation)
        {
            if (transformation == null || string.IsNullOrWhiteSpace(transformation.Description))
            {
                return null;
            }

            return $"{localizer[transformation.Name]}:\n{localizer[transformation.Description]}";


        }
    }
}
