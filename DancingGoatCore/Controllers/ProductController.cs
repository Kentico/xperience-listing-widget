using System;
using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine.Types.DancingGoatCore;
using CMS.Ecommerce;

using DancingGoat.Controllers;
using DancingGoat.Infrastructure;
using DancingGoat.Models;
using DancingGoat.Services;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

[assembly: RegisterPageRoute(Coffee.CLASS_NAME, typeof(ProductController), ActionName = nameof(ProductController.Detail))]
[assembly: RegisterPageRoute(Brewer.CLASS_NAME, typeof(ProductController), ActionName = nameof(ProductController.Detail))]
[assembly: RegisterPageRoute(FilterPack.CLASS_NAME, typeof(ProductController), ActionName = nameof(ProductController.Detail))]
[assembly: RegisterPageRoute(Tableware.CLASS_NAME, typeof(ProductController), ActionName = nameof(ProductController.Detail))]
[assembly: RegisterPageRoute(ManualGrinder.CLASS_NAME, typeof(ProductController), ActionName = nameof(ProductController.Detail))]
[assembly: RegisterPageRoute(ElectricGrinder.CLASS_NAME, typeof(ProductController), ActionName = nameof(ProductController.Detail))]

namespace DancingGoat.Controllers
{
    public class ProductController : Controller
    {
        private readonly IPageDataContextRetriever dataRetriever;
        private readonly ICalculationService calculationService;
        private readonly VariantRepository variantRepository;
        private readonly TypedProductViewModelFactory typedProductViewModelFactory;
        private readonly IShoppingService shoppingService;
        private readonly ISKUInfoProvider skuInfoProvider;


        public ProductController(IPageDataContextRetriever dataRetriever, ICalculationService calculationService, 
            VariantRepository variantRepository, TypedProductViewModelFactory typedProductViewModelFactory,
            IShoppingService shoppingService, ISKUInfoProvider skuInfoProvider)
        {
            this.dataRetriever = dataRetriever;
            this.calculationService = calculationService;
            this.variantRepository = variantRepository;
            this.typedProductViewModelFactory = typedProductViewModelFactory;
            this.shoppingService = shoppingService;
            this.skuInfoProvider = skuInfoProvider;
        }


        public ActionResult Detail()
        {
            var product = dataRetriever.Retrieve<SKUTreeNode>().Page;
            var sku = product.SKU;

            // If a product is not found or not allowed for sale, redirect to 404
            if (!sku?.SKUEnabled ?? true)
            {
                return NotFound();
            }

            // all variants are disabled, redirect to 404
            var variants = variantRepository.GetByProductId(product.NodeSKUID);
            var variantCount = variants.Count();

            if (variantCount > 0 && variantCount == variants.Count(i => !i.Variant.SKUEnabled))
            {
                return NotFound();
            }

            var viewModel = PrepareProductDetailViewModel(product);

            return View(viewModel);
        }


        [HttpPost]
        [Route("Product/Variant")]
        public JsonResult Variant(List<int> options, int parentProductID, [FromServices] IStringLocalizer<SharedResources> localizer)
        {
            var variant = variantRepository.GetByProductIdAndOptions(parentProductID, options);

            if (variant == null)
            {
                return Json(new
                {
                    stockMessage = localizer["Not available"],
                    totalPrice = "-"
                });
            }

            var cart = shoppingService.GetCurrentShoppingCart();
            var variantPrice = calculationService.CalculatePrice(variant.Variant);
            var isInStock = ((variant.Variant.SKUTrackInventory == TrackInventoryTypeEnum.Disabled) || (variant.Variant.SKUAvailableItems > 0));
            var allowSale = isInStock || !variant.Variant.SKUSellOnlyAvailable;

            return GetVariantResponse(variantPrice,
                                      isInStock,
                                      allowSale,
                                      variant.Variant.SKUID,
                                      cart.Currency,
                                      localizer);

        }


        private JsonResult GetVariantResponse(ProductCatalogPrices priceDetail, bool inStock, bool allowSale, int variantSKUID, CurrencyInfo currency, IStringLocalizer<SharedResources> localizer)
        {
            string priceSavings = string.Empty;

            var discount = priceDetail.StandardPrice - priceDetail.Price;
            var beforeDiscount = priceDetail.Price + discount;

            if (discount > 0)
            {
                var discountPercentage = Math.Round(discount * 100 / beforeDiscount);
                var formattedDiscount = String.Format(currency.CurrencyFormatString, discount);
                priceSavings = $"{formattedDiscount} ({discountPercentage}%)";
            }

            var response = new
            {
                totalPrice = String.Format(currency.CurrencyFormatString, priceDetail.Price),
                beforeDiscount = discount > 0 ? String.Format(currency.CurrencyFormatString, beforeDiscount) : string.Empty,
                savings = priceSavings,
                stockMessage = inStock ? localizer["In stock"].Value : localizer["Out of stock"].Value,
                inStock,
                allowSale,
                variantSKUID
            };

            return Json(response);
        }


        private ProductViewModel PrepareProductDetailViewModel(SKUTreeNode product)
        {
            var sku = product.SKU;

            var cheapestVariant = GetCheapestVariant(product);
            var variantCategories = PrepareProductOptionCategoryViewModels(sku, cheapestVariant);

            // Calculate the price of the product or selected variant
            var cheapestProduct = cheapestVariant != null ? cheapestVariant.Variant : sku;
            var price = calculationService.CalculatePrice(cheapestProduct);

            // Create a strongly typed view model with product type specific information
            var typedProduct = typedProductViewModelFactory.GetViewModel(product);

            var viewModel = (cheapestVariant != null)
                ? new ProductViewModel(product, price, typedProduct, cheapestVariant, variantCategories)
                : new ProductViewModel(product, price, typedProduct);
            return viewModel;
        }


        private IEnumerable<ProductOptionCategoryViewModel> PrepareProductOptionCategoryViewModels(SKUInfo sku, ProductVariant cheapestVariant)
        {
            var categories = variantRepository.GetVariantOptionCategories(sku.SKUID);

            // Set the selected options in the variant categories which represents the cheapest variant
            var variantCategories =
                cheapestVariant?.ProductAttributes.Select(
                    option =>
                        new ProductOptionCategoryViewModel(sku.SKUID, option.SKUID,
                            categories.FirstOrDefault(c => c.CategoryID == option.SKUOptionCategoryID), skuInfoProvider));

            return variantCategories;
        }


        private ProductVariant GetCheapestVariant(SKUTreeNode product)
        {
            return variantRepository.GetByProductId(product.NodeSKUID).OrderBy(v => v.Variant.SKUPrice).FirstOrDefault();
        }
    }
}