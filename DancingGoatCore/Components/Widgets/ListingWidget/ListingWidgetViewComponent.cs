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
        private readonly TransformationDropDownService transformationDropdownService;
        private readonly PageTypesDropDownService pageTypeDropdownService;
        private readonly IPageBuilderDataContextRetriever pageBuilderDataContextRetriever;
        private readonly SupportedTransformations supportedTransformations;
        private readonly IOrderByFieldService orderByFieldService;


        /// <summary>
        /// Widget identifier.
        /// </summary>
        public const string IDENTIFIER = "DancingGoat.General.ListingWidget";


        /// <summary>
        /// Creates an instance of <see cref="ListingWidgetViewComponent"/> class.
        /// </summary>
        /// <param name="pageBuilderDataContextRetriever">Page builder data context retriever.</param>
        /// <param name="transformationDropdownService">Transformation drop-down service.</param>
        /// <param name="pageTypeDropdownService">Page repository.</param>
        /// <param name="supportedTransformations">Supported transformations.</param>
        /// <param name="orderByFieldService">Order by field service.</param>
        public ListingWidgetViewComponent(IPageBuilderDataContextRetriever pageBuilderDataContextRetriever, TransformationDropDownService transformationDropdownService, PageTypesDropDownService pageTypeDropdownService, SupportedTransformations supportedTransformations, IOrderByFieldService orderByFieldService)
        {
            this.pageBuilderDataContextRetriever = pageBuilderDataContextRetriever;
            this.orderByFieldService = orderByFieldService;
            this.transformationDropdownService = transformationDropdownService;
            this.pageTypeDropdownService = pageTypeDropdownService;
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
            selectedOrderByField = orderByFieldService.IsValidField(selectedPageType, selectedOrderByField) ? selectedOrderByField : string.Empty;

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
                    OrderFieldSelectorViewModel = orderByFieldService.GetDropDownModel(selectedPageType, viewModel.Properties.OrderByField),
                    PageTypeSelectorViewModel = pageTypeDropdownService.GetDropDownModel(selectedPageType),
                    TransformationSelectorViewModel = transformationDropdownService.GetDropDownModel(viewModel.Properties.SelectedTransformationPath, selectedPageType),
                };
            }

            return View("~/Components/Widgets/ListingWidget/_ListingWidget.cshtml", model);
        }
    }
}
