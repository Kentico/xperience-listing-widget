using System.Collections.Generic;
using System.Linq;

using Kentico.Xperience.ListingWidget.InlineEditors;
using Kentico.Xperience.ListingWidget.Widgets;

namespace Kentico.Xperience.ListingWidget
{
    /// <inheritdoc/>
    internal class TransformationEditorService : ITransformationEditorService
    {
        private readonly ListingWidgetTransformationsRetriever transformationsRetriever;


        /// <summary>
        /// Creates an instance of <see cref="TransformationEditorService"/> class.
        /// </summary>
        /// <param name="transformationsRetriever">Supported transformations retriever.</param>
        public TransformationEditorService(ListingWidgetTransformationsRetriever transformationsRetriever)
        {
            this.transformationsRetriever = transformationsRetriever;
        }


        /// <inheritdoc/>
        public DropDownEditorViewModel GetEditorModel(string selectedOption, string pageType)
        {
            var reset = !string.IsNullOrEmpty(selectedOption) && !transformationsRetriever.IsSupported(selectedOption, pageType);
            return new DropDownEditorViewModel(nameof(ListingWidgetProperties.TransformationPath), GetOptions(pageType), selectedOption, "Transformation", GetTooltip(pageType), reset);
        }


        private IEnumerable<DropDownOptionViewModel> GetOptions(string pageType)
        {
            if (string.IsNullOrEmpty(pageType))
            {
                return Enumerable.Empty<DropDownOptionViewModel>();
            }
            return transformationsRetriever.Retrieve(pageType).Select(transformation => new DropDownOptionViewModel(transformation.View, transformation.Name));
        }


        private string GetTooltip(string pageType)
        {
            if (string.IsNullOrEmpty(pageType))
            {
                return string.Empty;
            }
            var tooltips = transformationsRetriever.Retrieve(pageType)
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

            return $"{transformation.Name}:\n{transformation.Description}";
        }
    }
}
