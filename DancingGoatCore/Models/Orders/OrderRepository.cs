using System.Collections.Generic;
using System.Linq;

using CMS.Ecommerce;
using CMS.SiteProvider;

using DancingGoat.Infrastructure;

namespace DancingGoat.Models
{
    /// <summary>
    /// Provides CRUD operations for orders.
    /// </summary>
    public class OrderRepository
    {
        private readonly IOrderInfoProvider orderInfoProvider;
        private readonly RepositoryCacheHelper repositoryCacheHelper;


        /// <summary>
        /// Initializes a new instance of the <see cref="OrderRepository"/> class.
        /// </summary>
        /// <param name="orderInfoProvider">Provider for <see cref="OrderInfo"/> management.</param>
        /// <param name="repositoryCacheHelper">Handles caching of retrieved objects.</param>
        public OrderRepository(IOrderInfoProvider orderInfoProvider, RepositoryCacheHelper repositoryCacheHelper)
        {
            this.orderInfoProvider = orderInfoProvider;
            this.repositoryCacheHelper = repositoryCacheHelper;
        }


        /// <summary>
        /// Returns an order with the specified identifier.
        /// </summary>
        /// <param name="orderId">Order's identifier.</param>
        /// <returns><see cref="OrderInfo"/> object representing an order with the specified identifier. Returns <c>null</c> if not found.</returns>
        public OrderInfo GetById(int orderId)
        {
            return repositoryCacheHelper.CacheObject(() =>
            {
                var orderInfo = orderInfoProvider.Get(orderId);

                if (orderInfo == null || orderInfo.OrderSiteID != SiteContext.CurrentSiteID)
                {
                    return null;
                }

                return orderInfo;
            }, $"{nameof(OrderRepository)}|{nameof(GetById)}|{orderId}");
        }


        /// <summary>
        /// Returns an enumerable collection of TopN orders of the given customer ordered by OrderDate descending.
        /// </summary>
        /// <param name="customerId">Customer's identifier.</param>
        /// <param name="count">Number of retrieved orders. Using 0 returns all records.</param>
        /// <returns>Collection of the customer's orders. See <see cref="OrderInfo"/> for detailed information.</returns>
        public IEnumerable<OrderInfo> GetByCustomerId(int customerId, int count = 0)
        {
            return repositoryCacheHelper.CacheObjects(() =>
            {
                return orderInfoProvider.GetBySite(SiteContext.CurrentSiteID)
                    .WhereEquals("OrderCustomerID", customerId)
                    .TopN(count)
                    .OrderByDescending(orderInfo => orderInfo.OrderDate);
            }, $"{nameof(OrderRepository)}|{nameof(GetByCustomerId)}|{customerId}|{count}");
        }
    }
}