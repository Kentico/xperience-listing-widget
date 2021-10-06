using CMS.Ecommerce;

using System.Linq;

namespace DancingGoat.Services
{
    /// <summary>
    /// Provides methods for prices calculations.
    /// </summary>
    public class CalculationService : ICalculationService
    {
        private readonly IShoppingService mShoppingService;
        private readonly ICatalogPriceCalculatorFactory mCatalogPriceCalculatorFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="shoppingService">Shopping service.</param>
        /// <param name="catalogPriceCalculatorFactory">Catalog price calculator factory.</param>
        public CalculationService(IShoppingService shoppingService, ICatalogPriceCalculatorFactory catalogPriceCalculatorFactory)
        {
            mShoppingService = shoppingService;
            mCatalogPriceCalculatorFactory = catalogPriceCalculatorFactory;
        }


        /// <summary>
        /// Calculates and returns prices of the given product.
        /// </summary>
        /// <param name="product">Product to calculate prices for.</param>
        public ProductCatalogPrices CalculatePrice(SKUInfo product)
        {
            var cart = mShoppingService.GetCurrentShoppingCart();

            var prices = mCatalogPriceCalculatorFactory
                         .GetCalculator(cart.ShoppingCartSiteID)
                         .GetPrices(product, Enumerable.Empty<SKUInfo>(), cart);

            return prices;
        }
    }
}