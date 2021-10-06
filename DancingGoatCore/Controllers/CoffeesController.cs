using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine.Types.DancingGoatCore;

using DancingGoat;
using DancingGoat.Controllers;
using DancingGoat.Helpers;
using DancingGoat.Models;
using DancingGoat.Services;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

[assembly: RegisterPageRoute(ProductSection.CLASS_NAME, typeof(CoffeesController), Path = ContentItemIdentifiers.COFFEES)]

namespace DancingGoat.Controllers
{
    public class CoffeesController : Controller
    {
        private readonly CoffeeRepository coffeeRepository;
        private readonly ICalculationService calculationService;
        private readonly IPageUrlRetriever pageUrlRetriever;


        public CoffeesController(CoffeeRepository coffeeRepository, ICalculationService calculationService, IPageUrlRetriever pageUrlRetriever)
        {
            this.coffeeRepository = coffeeRepository;
            this.calculationService = calculationService;
            this.pageUrlRetriever = pageUrlRetriever;
        }


        // GET: Coffees
        public ActionResult Index([FromServices] IStringLocalizer<SharedResources> localizer)
        {
            var items = GetFilteredCoffees(null);
            var filter = new CoffeeFilterViewModel();
            filter.Load(localizer);

            return View(new ProductListViewModel
            {
                Filter = filter,
                Items = items
            });
        }


        // POST: Filter
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Coffees/Filter")]
        public ActionResult Filter(CoffeeFilterViewModel filter)
        {
            if (!Request.IsAjaxRequest())
            {
                return NotFound();
            }
               
            var items = GetFilteredCoffees(filter);

            return PartialView("CoffeeList", items);
        }


        private IEnumerable<ProductListItemViewModel> GetFilteredCoffees(IRepositoryFilter filter)
        {
            var coffees = coffeeRepository.GetCoffees(filter);
           
            var items = coffees.Select(
                coffee => new ProductListItemViewModel(
                    coffee,
                    calculationService.CalculatePrice(coffee.SKU),
                    coffee.Product.PublicStatus?.PublicStatusDisplayName,
                    pageUrlRetriever)
                );
            return items;
        }
    }
}