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

[assembly: RegisterPageRoute(ProductSection.CLASS_NAME, typeof(GrindersController), Path = ContentItemIdentifiers.GRINDERS)]

namespace DancingGoat.Controllers
{
    public class GrindersController : Controller
    {
        private readonly string[] grinderClasses = new[] { ManualGrinder.CLASS_NAME, ElectricGrinder.CLASS_NAME };

        private readonly ProductRepository productRepository;        
        private readonly ICalculationService calculationService;
        private readonly IPageUrlRetriever pageUrlRetriever;


        public GrindersController(ProductRepository productRepository, ICalculationService calculationService, IPageUrlRetriever pageUrlRetriever)
        {            
            this.productRepository = productRepository;
            this.calculationService = calculationService;
            this.pageUrlRetriever = pageUrlRetriever;
        }


        // GET: Grinder
        public ActionResult Index()
        {
            var items = GetFilteredGrinders(null);
            var filter = new GrinderFilterViewModel(productRepository);
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
        [Route("Grinders/Filter")]
        public ActionResult Filter(GrinderFilterViewModel filter)
        {
            if (!Request.IsAjaxRequest())
            {
                return NotFound();
            }

            var items = GetFilteredGrinders(filter);

            return PartialView("GrindersList", items);
        }


        private IEnumerable<ProductListItemViewModel> GetFilteredGrinders(GrinderFilterViewModel filter)
        {
            var grinders = productRepository.GetProducts(filter, grinderClasses);

            var items = grinders.Select(
                    grinder => new ProductListItemViewModel(
                        grinder,
                        calculationService.CalculatePrice(grinder.SKU),
                        GetProductStatus(grinder),
                        pageUrlRetriever)
                    );

            return items;
        }


        private string GetProductStatus(SKUTreeNode grinder)
        {
            if (grinder is ManualGrinder manualGrinder)
            {
                return manualGrinder.Product.PublicStatus?.PublicStatusDisplayName;
            }

            if (grinder is ElectricGrinder electricGrinder)
            {
                return electricGrinder.Product.PublicStatus?.PublicStatusDisplayName;
            }

            return string.Empty;
        }
    }
}