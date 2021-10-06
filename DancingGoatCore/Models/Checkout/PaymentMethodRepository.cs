using System.Collections.Generic;

using CMS.Ecommerce;
using CMS.SiteProvider;

using DancingGoat.Infrastructure;

namespace DancingGoat.Models
{
    /// <summary>
    /// Provides CRUD operations for payment methods.
    /// </summary>
    public class PaymentMethodRepository
    {
        private readonly IPaymentOptionInfoProvider paymentOptionInfoProvider;
        private readonly RepositoryCacheHelper repositoryCacheHelper;


        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentMethodRepository"/> class using the payment option provider given.
        /// </summary>
        /// <param name="paymentOptionInfoProvider">Provider for <see cref="PaymentOptionInfo"/> management.</param>
        public PaymentMethodRepository(IPaymentOptionInfoProvider paymentOptionInfoProvider, RepositoryCacheHelper repositoryCacheHelper)
        {
            this.paymentOptionInfoProvider = paymentOptionInfoProvider;
            this.repositoryCacheHelper = repositoryCacheHelper;
        }


        /// <summary>
        /// Returns a payment method with the specified identifier.
        /// </summary>
        /// <param name="paymentMethodId">Payment method's identifier.</param>
        /// <returns><see cref="PaymentOptionInfo"/> object representing a payment method with the specified identifier. Returns <c>null</c> if not found.</returns>
        public PaymentOptionInfo GetById(int paymentMethodId)
        {
            return repositoryCacheHelper.CacheObject(() =>
            {
                var paymentInfo = paymentOptionInfoProvider.Get(paymentMethodId);

                if (paymentInfo?.PaymentOptionSiteID == SiteContext.CurrentSiteID)
                {
                    return paymentInfo;
                }

                if (paymentInfo?.PaymentOptionSiteID == 0 && ECommerceSettings.AllowGlobalPaymentMethods(SiteContext.CurrentSiteID))
                {
                    return paymentInfo;
                }

                return null;
            }, $"{nameof(PaymentMethodRepository)}|{nameof(GetById)}|{paymentMethodId}");
        }


        /// <summary>
        /// Returns an enumerable collection of all enabled payment methods.
        /// </summary>
        /// <returns>Collection of enabled payment methods. See <see cref="PaymentOptionInfo"/> for detailed information.</returns>
        public IEnumerable<PaymentOptionInfo> GetAll()
        {
            return repositoryCacheHelper.CacheObjects(() =>
            {
                return paymentOptionInfoProvider.GetBySite(SiteContext.CurrentSiteID, true);
            }, $"{nameof(PaymentMethodRepository)}|{nameof(GetAll)}");
        }
    }
}