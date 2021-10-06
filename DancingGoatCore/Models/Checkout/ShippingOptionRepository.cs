using System.Collections.Generic;

using CMS.Ecommerce;
using CMS.SiteProvider;

using DancingGoat.Infrastructure;

namespace DancingGoat.Models
{
    /// <summary>
    /// Provides CRUD operations for shipping options.
    /// </summary>
    public class ShippingOptionRepository
    {
        private readonly IShippingOptionInfoProvider shippingOptionInfoProvider;
        private readonly RepositoryCacheHelper repositoryCacheHelper;


        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingOptionRepository"/> class using the shipping option provider given.
        /// </summary>
        /// <param name="shippingOptionInfoProvider">Provider for <see cref="ShippingOptionInfo"/> management.</param>
        /// <param name="repositoryCacheHelper">Handles caching of retrieved objects.</param>
        public ShippingOptionRepository(IShippingOptionInfoProvider shippingOptionInfoProvider, RepositoryCacheHelper repositoryCacheHelper)
        {
            this.shippingOptionInfoProvider = shippingOptionInfoProvider;
            this.repositoryCacheHelper = repositoryCacheHelper;
        }


        /// <summary>
        /// Returns a shipping option with the specified identifier.
        /// </summary>
        /// <param name="shippingOptionId">Shipping option's identifier.</param>
        /// <returns><see cref="ShippingOptionInfo"/> object representing a shipping option with the specified identifier. Returns <c>null</c> if not found.</returns>
        public ShippingOptionInfo GetById(int shippingOptionId)
        {
            return repositoryCacheHelper.CacheObject(() =>
            {
                var shippingInfo = shippingOptionInfoProvider.Get(shippingOptionId);

                if (shippingInfo == null || shippingInfo.ShippingOptionSiteID != SiteContext.CurrentSiteID)
                {
                    return null;
                }

                return shippingInfo;
            }, $"{nameof(ShippingOptionRepository)}|{nameof(GetById)}|{shippingOptionId}");
        }


        /// <summary>
        /// Returns an enumerable collection of all enabled shipping options.
        /// </summary>
        /// <returns>Collection of enabled shipping options. See <see cref="ShippingOptionInfo"/> for detailed information.</returns>
        public IEnumerable<ShippingOptionInfo> GetAllEnabled()
        {
            return repositoryCacheHelper.CacheObjects(() =>
            {
                return shippingOptionInfoProvider.GetBySite(SiteContext.CurrentSiteID, true);
            }, $"{nameof(ShippingOptionRepository)}|{nameof(GetAllEnabled)}");
        }
    }
}