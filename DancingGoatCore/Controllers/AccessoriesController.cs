using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine.Types.DancingGoatCore;
using CMS.Ecommerce;

using DancingGoat;
using DancingGoat.Controllers;
using DancingGoat.Helpers;
using DancingGoat.Models;
using DancingGoat.Services;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using Microsoft.AspNetCore.Mvc;

[assembly: RegisterPageRoute(ProductSection.CLASS_NAME, typeof(AccessoriesController), Path = ContentItemIdentifiers.ACCESSORIES)]

namespace DancingGoat.Controllers
{
    public class AccessoriesController : Controller
    {
        private readonly string[] accessoryClasses = new[] { FilterPack.CLASS_NAME, Tableware.CLASS_NAME };
    
        private readonly ProductRepository productRepository;
        private readonly ICalculationService calculationService;
        private readonly IPageUrlRetriever pageUrlRetriever;


        public AccessoriesController(ProductRepository productRepository, ICalculationService calculationService, IPageUrlRetriever pageUrlRetriever)
        {
            this.productRepository  = productRepository;
            this.calculationService = calculationService;
            this.pageUrlRetriever = pageUrlRetriever;
        }


        // GET: Grinder
        public ActionResult Index()
        {
            var items = GetFilteredAccessories(null);
            var filter = new AccessoryFilterViewModel();
            filter.Load();

            var model = new ProductListViewModel
            {
                Filter = filter,
                Items = items
            };

            return View(model);
        }


        // POST: Filter
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Accessories/Filter")]
        public ActionResult Filter(AccessoryFilterViewModel filter)
        {
            if (!Request.IsAjaxRequest())
            {
                return NotFound();
            }

            var items = GetFilteredAccessories(filter);

            return PartialView("AccessoriesList", items);
        }



        private IEnumerable<ProductListItemViewModel> GetFilteredAccessories(AccessoryFilterViewModel filter)
        {
            var accessories = productRepository.GetProducts(filter, accessoryClasses);                

            var items = accessories.Select(
                    accessory => new ProductListItemViewModel(
                        accessory,
                        calculationService.CalculatePrice(accessory.SKU),
                        GetProductStatus(accessory),
                        pageUrlRetriever)
                    );

            return items;
        }


        private string GetProductStatus(SKUTreeNode accessory)
        {
            if (accessory is FilterPack filterPack)
            {
                return filterPack.Product.PublicStatus?.PublicStatusDisplayName;
            }

            if (accessory is Tableware tableware)
            {
                return tableware.Product.PublicStatus?.PublicStatusDisplayName;
            }

            return string.Empty;
        }
    }
}