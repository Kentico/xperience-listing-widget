using Microsoft.AspNetCore.Mvc;

using Kentico.PageBuilder.Web.Mvc;

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
        public const string IDENTIFIER = "DancingGoat.ListingWidget";

        /// <summary>
        /// Returns instance of <see cref="IViewComponentResult"/>.
        /// </summary>
        /// <param name="viewModel">Component view model with <see cref="ListingWidgetProperties"/> properties.</param>
        public IViewComponentResult Invoke(ComponentViewModel<ListingWidgetProperties> viewModel)
        {
            return View("~/Components/Widgets/ListingWidget/_ListingWidget.cshtml");
        }
    }
}
