using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using CMS.DataEngine;
using CMS.DocumentEngine;

using Kentico.PageBuilder.Web.Mvc;

using DancingGoat.InlineEditors;
using DancingGoat.Models;
using DancingGoat.Widgets;

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


        private readonly IPageRepository repository;


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
            var selectedPages = FilterPagesByPageType(pages, viewModel.Properties.SelectedPageType);

            var model = new ListingWidgetViewModel
            {
                Pages = selectedPages.Select(p => new ListingWidgetPageViewModel(p.DocumentName)),
                SupportedPageTypes = ListingWidgetProperties.SupportedPageTypes.Select(
                    pt => new DropdownOptionViewModel(pt, DataClassInfoProvider.GetDataClassInfo(pt).ClassDisplayName)),
                SelectedPageType = viewModel.Properties.SelectedPageType
            };

            return View("~/Components/Widgets/ListingWidget/_ListingWidget.cshtml", model);
        }


        private IEnumerable<TreeNode> FilterPagesByPageType(IEnumerable<TreeNode> pages, string pageType)
        {
            if (string.IsNullOrEmpty(pageType))
            {
                return new List<TreeNode>();
            }
            return pages.Where(p => p.ClassName == pageType);
        }
    }
}
