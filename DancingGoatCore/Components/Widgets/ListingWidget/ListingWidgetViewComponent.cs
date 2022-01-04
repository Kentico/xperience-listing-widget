using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;

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


        /// <summary>
        /// All supported page types.
        /// </summary>
        public static readonly IEnumerable<string> SupportedPageTypes = new List<string> { Article.CLASS_NAME, Cafe.CLASS_NAME, Coffee.CLASS_NAME };


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
            var selectedPageType = viewModel.Properties.SelectedPageType;
            var pages = string.IsNullOrEmpty(selectedPageType) ? new List<TreeNode>() : repository.GetPages(selectedPageType);

            var classes = DataClassInfoProvider.GetClasses()
                .Where("ClassName IN ('" + string.Join("','", SupportedPageTypes) + "')")
                .Columns("ClassName", "ClassDisplayName")
                .ToDictionary(c => c.ClassName, c => c.ClassDisplayName);

            var model = new ListingWidgetViewModel
            {
                Pages = pages.Select(page => new ListingWidgetPageViewModel(page.DocumentName)),
                SupportedPageTypes = classes.Select(dataType => new DropdownOptionViewModel(dataType.Key, dataType.Value)),
                SelectedPageType = selectedPageType
            };

            return View("~/Components/Widgets/ListingWidget/_ListingWidget.cshtml", model);
        }
    }
}
