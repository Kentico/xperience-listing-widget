using System.Linq;
using System.Collections.Generic;

using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;

using DancingGoat.Models;
using DancingGoat.Widgets;
using DancingGoat.InlineEditors;


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

        
        private readonly IPageRepository repository;
        private readonly IPageBuilderDataContextRetriever pageBuilderDataContextRetriever;


        /// <summary>
        /// Creates an instance of <see cref="ListingWidgetViewComponent"/> class.
        /// </summary>
        /// <param name="repository">Page repository.</param>
        public ListingWidgetViewComponent(IPageRepository repository, IPageBuilderDataContextRetriever pageBuilderDataContextRetriever)
        {
            this.repository = repository;
            this.pageBuilderDataContextRetriever = pageBuilderDataContextRetriever;
        }


        /// <summary>
        /// Returns instance of <see cref="IViewComponentResult"/>.
        /// </summary>
        /// <param name="viewModel">Component view model with <see cref="ListingWidgetProperties"/> properties.</param>
        public IViewComponentResult Invoke(ComponentViewModel<ListingWidgetProperties> viewModel)
        {
            var selectedPageType = viewModel.Properties.SelectedPageType;
            var pages = string.IsNullOrEmpty(selectedPageType) 
                ? new List<TreeNode>() 
                : repository.GetPages(selectedPageType, viewModel.Properties.SelectedPage?.Path);
            var model = new ListingWidgetViewModel 
            { 
                SelectedPage = viewModel.Properties.SelectedPage,
                Pages = pages.Select(page => new ListingWidgetPageViewModel(page.DocumentName))
            };
            
            if (pageBuilderDataContextRetriever.Retrieve().EditMode)
            {
                model.PageTypeSelectorViewModel = new DropdownEditorViewModel(nameof(ListingWidgetProperties.SelectedPageType), GetSupportedPageTypes(), selectedPageType, "Page type");
            }
            return View("~/Components/Widgets/ListingWidget/_ListingWidget.cshtml", model);
        }


        private IEnumerable<DropdownOptionViewModel> GetSupportedPageTypes()
        {
            var SupportedPageTypes = new List<string> { Article.CLASS_NAME, Cafe.CLASS_NAME, Coffee.CLASS_NAME };
            var classes = DataClassInfoProvider.GetClasses()
                .WhereIn("ClassName", SupportedPageTypes)
                .Columns("ClassName", "ClassDisplayName")
                .ToDictionary(c => c.ClassName, c => c.ClassDisplayName);
            return classes.Select(dataType => new DropdownOptionViewModel(dataType.Key, dataType.Value));
        }
    }
}
