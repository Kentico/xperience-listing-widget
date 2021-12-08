using System.Linq;

using Microsoft.AspNetCore.Mvc;

using CMS.DocumentEngine;

using Kentico.PageBuilder.Web.Mvc;

using DancingGoat.Widgets;
using DancingGoat.Models;

[assembly: RegisterWidget(ListingWidgetViewComponent.IDENTIFIER, typeof(ListingWidgetViewComponent), "Listing",
    typeof(ListingWidgetProperties), Description = "Displays pages from selected location.", IconClass = "icon-l-grid-3-2")]

namespace DancingGoat.Widgets
{
    /// <summary>
    /// ViewComponent for listing widget.
    /// </summary>
    public class ListingWidgetViewComponent : ViewComponent
    {
        private readonly IPageRepository repository;
        
        
        /// <summary>
        /// Widget identifier.
        /// </summary>
        public const string IDENTIFIER = "DancingGoat.General.ListingWidget";


        /// <summary>
        /// Creates an instance of <see cref="ListingWidgetViewComponent"/> class.
        /// </summary>
        /// <param name="repository">Page repository.</param>
        public ListingWidgetViewComponent(IPageRepository repository)
        {
            this.repository = repository;
        }


        /// <summary>
        /// Returns instance of <see cref="IViewComponentResult"/>.
        /// </summary>
        /// <param name="viewModel">Component view model with <see cref="ListingWidgetProperties"/> properties.</param>
        public IViewComponentResult Invoke(ComponentViewModel<ListingWidgetProperties> viewModel)
        {
            var pages = repository.GetAllPages<TreeNode>();
            var model = new ListingWidgetBaseViewModel {Pages = pages.Select(p => new ListingWidgetBasePageViewModel(p.DocumentName)) };

            return View("~/Components/Widgets/ListingWidget/_DefaultTransformationListingWidget.cshtml", model);
        }
    }
}
