using System.Collections.Generic;

using CMS.Ecommerce;

using DancingGoat.Infrastructure;

namespace DancingGoat.Models
{
    /// <summary>
    /// Provides CRUD operations for customer addresses.
    /// </summary>
    public class CustomerAddressRepository
    {
        private readonly IAddressInfoProvider addressInfoProvider;
        private readonly RepositoryCacheHelper repositoryCacheHelper;


        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerAddressRepository"/> class using the address provider given.
        /// </summary>
        /// <param name="addressInfoProvider">Provider for <see cref="AddressInfo"/> management.</param>
        /// <param name="repositoryCacheHelper">Handles caching of retrieved objects.</param>
        public CustomerAddressRepository(IAddressInfoProvider addressInfoProvider, RepositoryCacheHelper repositoryCacheHelper)
        {
            this.addressInfoProvider = addressInfoProvider;
            this.repositoryCacheHelper = repositoryCacheHelper;
        }


        /// <summary>
        /// Returns a customer's address with the specified identifier.
        /// </summary>
        /// <param name="addressId">Identifier of the customer's address.</param>
        /// <returns>Customer's address with the specified identifier. Returns <c>null</c> if not found.</returns>
        public AddressInfo GetById(int addressId)
        {
            return repositoryCacheHelper.CacheObject(() =>
            {
                return addressInfoProvider.Get(addressId);
            }, $"{nameof(CustomerAddressRepository)}|{nameof(GetById)}|{addressId}");
        }


        /// <summary>
        /// Returns an enumerable collection of a customer's addresses.
        /// </summary>
        /// <param name="customerId">Customer's identifier.</param>
        /// <returns>Collection of customer's addresses. See <see cref="AddressInfo"/> for detailed information.</returns>
        public IEnumerable<AddressInfo> GetByCustomerId(int customerId)
        {
            return repositoryCacheHelper.CacheObjects(() =>
            {
                return addressInfoProvider.GetByCustomer(customerId);
            }, $"{nameof(CustomerAddressRepository)}|{nameof(GetByCustomerId)}|{customerId}");
        }


        /// <summary>
        /// Saves a customer's address into the database.
        /// </summary>
        /// <param name="address"><see cref="AddressInfo"/> object representing a customer's address that is inserted.</param>
        public void Upsert(AddressInfo address)
        {
            addressInfoProvider.Set(address);
        }
    }
}