using DancingGoat.InlineEditors;
using DancingGoat.Widgets;

using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Mvc;

[assembly: RegisterWidget(ListingWidgetViewComponent.IDENTIFIER, typeof(ListingWidgetViewComponent), "Listing",
    typeof(ListingWidgetProperties), Description = "Displays pages from selected location.", IconClass = "icon-l-grid-3-2")]

namespace DancingGoat.Widgets
{
    /// <summary>
    /// ViewComponent for listing widget.
    /// </summary>
    public class ListingWidgetViewComponent : ViewComponent
    {
        /// <summary>
        /// Widget identifier.
        /// </summary>
        public const string IDENTIFIER = "DancingGoat.General.ListingWidget";


        private readonly IPageBuilderDataContextRetriever pageBuilderDataContextRetriever;
        private readonly ITransformationEditorService transformationEditorService;
        private readonly IPageTypesEditorService pageTypeEditorService;
        private readonly IOrderByFieldEditorService orderByFieldEditorService;
        private readonly SupportedTransformationsRetriever transformationsRetriever;


        /// <summary>
        /// Creates an instance of <see cref="ListingWidgetViewComponent"/> class.
        /// </summary>
        /// <param name="pageBuilderDataContextRetriever">Page builder data context retriever.</param>
        /// <param name="transformationEditorService">Transformation editor service.</param>
        /// <param name="pageTypeEditorService">Page type editor service.</param>
        /// <param name="orderByFieldEditorService">Order by field editor service.</param>
        /// <param name="transformationsRetriever">Supported transformations retriever.</param>
        public ListingWidgetViewComponent(IPageBuilderDataContextRetriever pageBuilderDataContextRetriever, ITransformationEditorService transformationEditorService, IPageTypesEditorService pageTypeEditorService, IOrderByFieldEditorService orderByFieldEditorService, SupportedTransformationsRetriever transformationsRetriever)
        {
            this.pageBuilderDataContextRetriever = pageBuilderDataContextRetriever;
            this.orderByFieldEditorService = orderByFieldEditorService;
            this.transformationEditorService = transformationEditorService;
            this.pageTypeEditorService = pageTypeEditorService;
            this.transformationsRetriever = transformationsRetriever;
        }


        /// <summary>
        /// Returns instance of <see cref="IViewComponentResult"/>.
        /// </summary>
        /// <param name="viewModel">Component view model with <see cref="ListingWidgetProperties"/> properties.</param>
        public IViewComponentResult Invoke(ComponentViewModel<ListingWidgetProperties> viewModel)
        {
            var widgetProperties = viewModel.Properties;
            var selectedPageType = widgetProperties.PageType;
            var selectedTopN = widgetProperties.TopN;
            var selectedOrderDirection = widgetProperties.OrderDirection;
            var selectedOrderByField = widgetProperties.OrderByField;
            var selectedTransformation = widgetProperties.TransformationPath;
            selectedTransformation = transformationsRetriever.IsTransformationSupported(selectedTransformation, selectedPageType) ? selectedTransformation : null;
            selectedOrderByField = orderByFieldEditorService.IsValidField(selectedPageType, selectedOrderByField) ? selectedOrderByField : string.Empty;

            var model = new ListingWidgetViewModel
            {
                SelectedValues = new ListingWidgetSelectedValues
                {
                    TopN = selectedTopN,
                    OrderDirection = selectedOrderDirection,
                    OrderByField = selectedOrderByField,
                    TransformationPath = selectedTransformation,
                    PageType = selectedPageType,
                    ParentPageAliasPath = widgetProperties.ParentPageAliasPath,
                }
            };

            if (pageBuilderDataContextRetriever.Retrieve().EditMode)
            {
                model.InlineEditors = new ListingWidgetInlineEditors
                {
                    OrderFieldSelector = orderByFieldEditorService.GetEditorModel(selectedPageType, widgetProperties.OrderByField),
                    PageTypeSelector = pageTypeEditorService.GetEditorModel(selectedPageType),
                    TransformationSelector = transformationEditorService.GetEditorModel(widgetProperties.TransformationPath, selectedPageType),
                    TopNEditor = new TopNEditorViewModel
                    {
                        PropertyName = nameof(ListingWidgetProperties.TopN),
                        TopN = selectedTopN
                    },
                    OrderDirectionEditor = new OrderDirectionViewModel
                    {
                        PropertyName = nameof(ListingWidgetProperties.OrderDirection),
                        Order = selectedOrderDirection
                    },
                    PathEditor = new PathEditorProperties
                    {
                        PropertyName = nameof(ListingWidgetProperties.ParentPageAliasPath),
                        PageAliasPath = widgetProperties.ParentPageAliasPath
                    }
                };
            }

            return View("~/Components/Widgets/ListingWidget/_ListingWidget.cshtml", model);
        }
    }
}
