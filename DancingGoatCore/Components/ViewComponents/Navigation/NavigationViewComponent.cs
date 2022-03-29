using System.Linq;

using DancingGoat.Models;

using Kentico.Content.Web.Mvc;

using Microsoft.AspNetCore.Mvc;

namespace DancingGoat.Controllers
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly NavigationRepository navigationRepository;
        private readonly IPageUrlRetriever pageUrlRetriever;


        public NavigationViewComponent(NavigationRepository navigationRepository, IPageUrlRetriever pageUrlRetriever)
        {
            this.navigationRepository = navigationRepository;
            this.pageUrlRetriever = pageUrlRetriever;
        }


        public IViewComponentResult Invoke(bool footerNavigation)
        {
            var viewName = footerNavigation ? "Footer" : "Menu";
            var menuItems = footerNavigation ? navigationRepository.GetFooterNavigationItems() : navigationRepository.GetMenuItems();
            var menuItemsModel = menuItems.Select(menuItem => MenuItemViewModel.GetViewModel(menuItem, pageUrlRetriever));

            return View($"~/Components/ViewComponents/Navigation/{viewName}.cshtml", menuItemsModel);
        }
    }
}