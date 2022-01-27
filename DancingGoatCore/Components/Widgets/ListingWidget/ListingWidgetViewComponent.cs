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
        private readonly ITransformationEditorService transformationEditorService;
        private readonly IPageTypesEditorService pageTypeEditorService;
        private readonly IPageBuilderDataContextRetriever pageBuilderDataContextRetriever;
        private readonly IOrderByFieldEditorService orderByFieldEditorService;
        private readonly SupportedTransformations supportedTransformations;


        /// <summary>
        /// Widget identifier.
        /// </summary>
        public const string IDENTIFIER = "DancingGoat.General.ListingWidget";


        /// <summary>
        /// Creates an instance of <see cref="ListingWidgetViewComponent"/> class.
        /// </summary>
        /// <param name="pageBuilderDataContextRetriever">Page builder data context retriever.</param>
        /// <param name="transformationEditorService">Transformation drop-down editor service.</param>
        /// <param name="pageTypeEditorService">Page type drop-down editor service.</param>
        /// <param name="orderByFieldEditorService">Order by field editor service.</param>
        /// <param name="supportedTransformations">Supported transformations.</param>
        public ListingWidgetViewComponent(IPageBuilderDataContextRetriever pageBuilderDataContextRetriever, ITransformationEditorService transformationEditorService, IPageTypesEditorService pageTypeEditorService, IOrderByFieldEditorService orderByFieldEditorService, SupportedTransformations supportedTransformations)
        {
            this.pageBuilderDataContextRetriever = pageBuilderDataContextRetriever;
            this.orderByFieldEditorService = orderByFieldEditorService;
            this.transformationEditorService = transformationEditorService;
            this.pageTypeEditorService = pageTypeEditorService;
            this.supportedTransformations = supportedTransformations;
        }


        /// <summary>
        /// Returns instance of <see cref="IViewComponentResult"/>.
        /// </summary>
        /// <param name="viewModel">Component view model with <see cref="ListingWidgetProperties"/> properties.</param>
        public IViewComponentResult Invoke(ComponentViewModel<ListingWidgetProperties> viewModel)
        {
            var widgetProperties = viewModel.Properties;
            var selectedPageType = widgetProperties.SelectedPageType;
            var selectedOrderByField = widgetProperties.OrderByField;
            var selectedTransformation = viewModel.Properties.SelectedTransformationPath;
            selectedTransformation = supportedTransformations.IsTransformationSupportedForPageType(selectedTransformation, selectedPageType) ? selectedTransformation : null;
            selectedOrderByField = orderByFieldEditorService.IsValidField(selectedPageType, selectedOrderByField) ? selectedOrderByField : string.Empty;

            var model = new ListingWidgetViewModel
            {
                SelectedValues = new ListingWidgetSelectedValuesModel
                {
                    TopN = widgetProperties.TopN,
                    OrderDirection = widgetProperties.OrderDirection,
                    OrderByField = selectedOrderByField,
                    TransformationPath = selectedTransformation,
                    PageType = selectedPageType,
                    ParentPage = widgetProperties.SelectedPage,
                }
            };

            if (pageBuilderDataContextRetriever.Retrieve().EditMode)
            {
                model.EditorsModels = new ListingWidgetInlineEditorsViewModel
                {
                    OrderFieldSelectorViewModel = orderByFieldEditorService.GetEditorModel(selectedPageType, viewModel.Properties.OrderByField),
                    PageTypeSelectorViewModel = pageTypeEditorService.GetEditorModel(selectedPageType),
                    TransformationSelectorViewModel = transformationEditorService.GetEditorModel(viewModel.Properties.SelectedTransformationPath, selectedPageType),
                };
            }

            return View("~/Components/Widgets/ListingWidget/_ListingWidget.cshtml", model);
        }
    }
}
