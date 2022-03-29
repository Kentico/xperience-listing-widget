using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Mvc;

namespace Kentico.Xperience.ListingWidget.Widgets
{
    /// <summary>
    /// View component for transformation of listing widget.
    /// </summary>
    public class TransformationViewComponent : ViewComponent
    {
        private readonly ITransformationStrategy transformationStrategy;
        private readonly IPageRepository pageRepository;
        private readonly IPageBuilderDataContextRetriever pageBuilderDataContextRetriever;


        /// <summary>
        /// Creates an instance of <see cref="TransformationViewComponent"/> class.
        /// </summary>
        /// <param name="transformationStrategy">Strategy providing transformation service for selected transformation.</param>
        /// <param name="pageRepository">Page repository.</param>
        /// <param name="pageBuilderDataContextRetriever">Page builder data context retriever.</param>
        public TransformationViewComponent(ITransformationStrategy transformationStrategy, IPageRepository pageRepository, IPageBuilderDataContextRetriever pageBuilderDataContextRetriever)
        {
            this.transformationStrategy = transformationStrategy;
            this.pageRepository = pageRepository;
            this.pageBuilderDataContextRetriever = pageBuilderDataContextRetriever;
        }


        /// <summary>
        /// Returns transformation view as instance of <see cref="IViewComponentResult"/>.
        /// </summary>
        /// <param name="model">Model with properties selected from inline editors of listing widget.</param>
        public IViewComponentResult Invoke(ListingWidgetSelectedValues model)
        {
            if (string.IsNullOrEmpty(model.PageType) || string.IsNullOrEmpty(model.TransformationPath))
            {
                if (pageBuilderDataContextRetriever.Retrieve().EditMode)
                {
                    return View("~/Components/Widgets/ListingWidget/Transformations/_NoTransformationSelected.cshtml");
                }
                return Content(string.Empty);
            }

            var transformationService = transformationStrategy.GetService(model.TransformationPath);
            var customQueryParametrization = transformationService.GetCustomQueryParametrization(model.TransformationPath);
            var customCacheDependencyKey = transformationService.GetCustomCacheDependencyKey(model.TransformationPath);
            var customCacheDependency = transformationService.GetCustomCacheDependency(model.TransformationPath);

            var pages = pageRepository.GetPages(model.PageType, model.ParentPageAliasPath, model.TopN, model.OrderByField, model.OrderDirection, customQueryParametrization, customCacheDependencyKey, customCacheDependency);
            var transformationModel = transformationService.GetModel(pages);

            return View(model.TransformationPath, transformationModel);
        }
    }
}
