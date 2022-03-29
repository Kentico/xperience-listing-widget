using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine.Types.DancingGoatCore;
using CMS.Ecommerce;

using DancingGoat.Controllers;
using DancingGoat.Models;
using DancingGoat.Services;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using Microsoft.AspNetCore.Mvc;

[assembly: RegisterPageRoute(ManufacturerSection.CLASS_NAME, typeof(ManufacturersController))]
[assembly: RegisterPageRoute(Manufacturer.CLASS_NAME, typeof(ManufacturersController), ActionName = nameof(ManufacturersController.Detail))]

namespace DancingGoat.Controllers
{
    public class ManufacturersController : Controller
    {
        private readonly IPageDataContextRetriever dataRetriever;
        private readonly ManufacturerRepository manufacturerRepository;
        private readonly ProductRepository productRepository;
        private readonly ICalculationService calculationService;
        private readonly PublicStatusRepository publicStatusRepository;
        private readonly IPageUrlRetriever pageUrlRetriever;


        public ManufacturersController(IPageDataContextRetriever dataRetriever,
            ManufacturerRepository manufacturerRepository, 
            ProductRepository productRepository, 
            ICalculationService calculationService,
            PublicStatusRepository publicStatusRepository, 
            IPageUrlRetriever pageUrlRetriever)
        {
            this.dataRetriever = dataRetriever;
            this.manufacturerRepository = manufacturerRepository;
            this.productRepository = productRepository;
            this.calculationService = calculationService;
            this.publicStatusRepository = publicStatusRepository;
            this.pageUrlRetriever = pageUrlRetriever;
        }


        public ActionResult Index()
        {
            var manufacturers = manufacturerRepository.GetManufacturers(ContentItemIdentifiers.MANUFACTURERS);

            var model = GetManufacturersViewModel(manufacturers);

            return View(model);
        }


        public ActionResult Detail()
        {
            var manufacturer = dataRetriever.Retrieve<Manufacturer>().Page;
            var manufactutersProducts = productRepository.GetProducts(manufacturer.NodeAliasPath);
            var model = GetProductsViewModel(manufactutersProducts, manufacturer.NodeAlias);

            return View(new ManufacturerDetailViewModel(manufacturer, model));
        }


        private IEnumerable<ManufactureListItemViewModel> GetManufacturersViewModel(IEnumerable<Manufacturer> manufacturers)
        {
            return manufacturers.Select(manufacturer => new ManufactureListItemViewModel(manufacturer, pageUrlRetriever));
        }


        private IEnumerable<ProductListItemViewModel> GetProductsViewModel(IEnumerable<SKUTreeNode> products, string pageAlias)
        {
            return products.Select(
                product => new ProductListItemViewModel(
                    product,
                    calculationService.CalculatePrice(product.SKU),
                    publicStatusRepository.GetById(product.SKU.SKUPublicStatusID)?.PublicStatusDisplayName,
                    pageUrlRetriever) 
                {
                    CategoryName = pageAlias 
                });
        }
    }
}