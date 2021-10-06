using CMS.Ecommerce;

namespace DancingGoat.Services
{
    /// <summary>
    /// Interface for service providing methods for prices calculations.
    /// </summary>
    public interface ICalculationService
    {
        /// <summary>
        /// Calculates and returns prices of the given product.
        /// </summary>
        /// <param name="product">Product to calculate prices for.</param>
        ProductCatalogPrices CalculatePrice(SKUInfo product);
    }
}