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

[assembly: RegisterPageRoute(ProductSection.CLASS_NAME, typeof(BrewersController), Path = ContentItemIdentifiers.BREWERS)]

namespace DancingGoat.Controllers
{
    public class BrewersController : Controller
    {
        private readonly BrewerRepository brewerRepository;
        private readonly ICalculationService calculationService;
        private readonly IPageUrlRetriever pageUrlRetriever;


        public BrewersController(BrewerRepository brewerRepository, ICalculationService calculationService, IPageUrlRetriever pageUrlRetriever)
        {
            this.brewerRepository = brewerRepository;
            this.calculationService = calculationService;
            this.pageUrlRetriever = pageUrlRetriever;
        }


        // GET: Brewers
        public ActionResult Index()
        {
            var item = GetFilteredBrewers(null);
            var filter = new BrewerFilterViewModel(brewerRepository);
            filter.Load();

            var model = new ProductListViewModel
            {
                Filter = filter,
                Items = item
            };

            return View(model);
        }


        // POST: Filter
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Brewers/Filter")]
        public ActionResult Filter(BrewerFilterViewModel filter)
        {
            if (!Request.IsAjaxRequest())
            {
                return NotFound();
            }

            var items = GetFilteredBrewers(filter);

            return PartialView("BrewersList", items);
        }


        private IEnumerable<ProductListItemViewModel> GetFilteredBrewers(IRepositoryFilter filter)
        {
            var brewers = brewerRepository.GetBrewers(filter);

            var items = brewers.Select(
                brewer => new ProductListItemViewModel(
                    brewer,
                    calculationService.CalculatePrice(brewer.SKU),
                    brewer.Product.PublicStatus?.PublicStatusDisplayName,
                    pageUrlRetriever)
                );
            return items;
        }
    }
}