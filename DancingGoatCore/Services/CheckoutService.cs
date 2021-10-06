using System;
using System.Collections.Generic;
using System.Linq;

using CMS.Ecommerce;
using CMS.Globalization;

using DancingGoat.Models;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace DancingGoat.Services
{
    /// <summary>
    /// Provides methods to build checkout models.
    /// </summary>
    public class CheckoutService : ICheckoutService
    {
        private readonly IShoppingService mShoppingService;
        private readonly PaymentMethodRepository mPaymentMethodRepository;
        private readonly ShippingOptionRepository mShippingOptionRepository;
        private readonly CountryRepository mCountryRepository;
        private readonly CustomerAddressRepository mAddressRepository;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="shoppingService">Shopping service</param>
        /// <param name="addressRepository">Address repository</param>
        /// <param name="paymentMethodRepository">Payment method repository</param>
        /// <param name="shippingOptionRepository">Shipping option repository</param>
        /// <param name="countryRepository">Country repository</param>
        public CheckoutService(IShoppingService shoppingService, CustomerAddressRepository addressRepository, PaymentMethodRepository paymentMethodRepository, ShippingOptionRepository shippingOptionRepository, CountryRepository countryRepository)
        {
            mShoppingService = shoppingService;
            mPaymentMethodRepository = paymentMethodRepository;
            mShippingOptionRepository = shippingOptionRepository;
            mCountryRepository = countryRepository;
            mAddressRepository = addressRepository;
        }


        /// <summary>
        /// Gets shipping option info.
        /// </summary>
        /// <param name="shippingOptionId">ID of shipping option which should be returned.</param>
        public ShippingOptionInfo GetShippingOption(int shippingOptionId)
        {
            return mShippingOptionRepository.GetById(shippingOptionId);
        }


        /// <summary>
        /// Gets address with <paramref name="addressID"/> ID, but only if the address belongs to the customer used by shopping cart.
        /// </summary>
        /// <param name="addressID">The identifier of the address.</param>
        /// <returns>Address of the customer used by shopping cart. <c>Null</c> if customer does not have address with given Id.</returns>
        public AddressInfo GetCustomerAddress(int addressID)
        {
            var customer = mShoppingService.GetCurrentShoppingCart().Customer;
            return mAddressRepository.GetByCustomerId(customer.CustomerID).FirstOrDefault(a => a.AddressID == addressID);
        }


        /// <summary>
        /// Gets payment method with required ID.
        /// </summary>
        /// <param name="paymentMethodId">ID of payment method which should be returned.</param>
        public PaymentOptionInfo GetPaymentMethod(int paymentMethodId)
        {
            return mPaymentMethodRepository.GetById(paymentMethodId);
        }


        /// <summary>
        /// Create an enumerable collection of states for given country.
        /// </summary>
        /// <param name="countryId">Country ID which states should be listed.</param>
        public IEnumerable<StateInfo> GetCountryStates(int countryId)
        {
            return mCountryRepository.GetCountryStates(countryId).ToList();
        }


        /// <summary>
        /// Gets the address with the specified identifier.
        /// </summary>
        /// <param name="addressID">Address identifier.</param>
        public AddressInfo GetAddress(int addressID)
        {
            return mAddressRepository.GetById(addressID);
        }


        /// <summary>
        /// Checks if coupon code value can be used during checkout process.
        /// </summary>
        /// <param name="couponCode">Coupon code to be checked.</param>
        /// <returns>
        /// True if coupon code defined in <paramref name="couponCode"/> is accepted by shopping cart or if coupon code is empty or null.
        /// </returns>
        public bool IsCouponCodeValueValid(string couponCode)
        {
            var cart = mShoppingService.GetCurrentShoppingCart();

            return string.IsNullOrEmpty(couponCode) || cart.CouponCodes.AllAppliedCodes.Select(x => x.Code).Contains(couponCode, ECommerceHelper.CouponCodeComparer);
        }


        /// <summary>
        /// Checks if shipping option is among applicable shipping options for the shopping cart.
        /// </summary>
        /// <param name="shippingOptionId">ID of shipping option.</param>
        /// <returns>True if shipping option can be used in the shopping cart.</returns>
        public bool IsShippingOptionValid(int shippingOptionId)
        {
            var shippingOptions = mShippingOptionRepository.GetAllEnabled().ToList();

            return shippingOptions.Exists(s => s.ShippingOptionID == shippingOptionId);
        }


        /// <summary>
        /// Checks if country exists.
        /// </summary>
        /// <param name="countryId">ID of country which should be checked</param>
        /// <returns>Return true if country exists.</returns>
        public bool IsCountryValid(int countryId)
        {
            return mCountryRepository.GetCountry(countryId) != null;
        }


        /// <summary>
        /// Checks if state is valid for given country.
        /// </summary>
        /// <param name="countryId">ID of state`s country </param>
        /// <param name="stateId">ID of state</param>
        /// <returns>True if state is not required or state belongs to the country.</returns>
        public bool IsStateValid(int countryId, int? stateId)
        {
            var states = mCountryRepository.GetCountryStates(countryId).ToList();

            return (states.Count < 1) || states.Exists(s => s.StateID == stateId);
        }


        /// <summary>
        /// Checks if payment method is applicable for given shopping cart.
        /// </summary>
        /// <param name="paymentMethodId">ID of payment method</param>
        /// <returns>True if payment method applicable for current shopping cart.</returns>
        public bool IsPaymentMethodValid(int paymentMethodId)
        {
            var cart = mShoppingService.GetCurrentShoppingCart();
            var paymentMethods = GetApplicablePaymentMethods(cart).ToList();

            return paymentMethods.Exists(p => p.PaymentOptionID == paymentMethodId);
        }


        /// <summary>
        /// Creates view model for checkout delivery step.
        /// </summary>
        /// <param name="customer">Filled customer details</param>
        /// <param name="billingAddress">Filled billing address</param>
        /// <param name="shippingOption">Selected shipping option</param>
        public DeliveryDetailsViewModel PrepareDeliveryDetailsViewModel(CustomerViewModel customer = null, BillingAddressViewModel billingAddress = null, ShippingOptionViewModel shippingOption = null)
        {
            var cart = mShoppingService.GetCurrentShoppingCart();
            var countries = CreateCountryList();
            var shippingOptions = CreateShippingOptionList();

            customer = customer ?? new CustomerViewModel(cart.Customer);

            var addresses = (cart.Customer != null)
                ? mAddressRepository.GetByCustomerId(cart.Customer.CustomerID)
                : Enumerable.Empty<AddressInfo>();

            var billingAddresses = new SelectList(addresses, nameof(AddressInfo.AddressID), nameof(AddressInfo.AddressName));

            billingAddress = billingAddress ?? new BillingAddressViewModel(mShoppingService.GetBillingAddress(), countries, mCountryRepository, billingAddresses);
            shippingOption = shippingOption ?? new ShippingOptionViewModel(cart.ShippingOption, shippingOptions, cart.IsShippingNeeded);

            billingAddress.BillingAddressCountryStateSelector.Countries = billingAddress.BillingAddressCountryStateSelector.Countries ?? countries;
            billingAddress.BillingAddressSelector = billingAddress.BillingAddressSelector ?? new AddressSelectorViewModel { Addresses = billingAddresses };
            shippingOption.ShippingOptions = shippingOptions;

            var viewModel = new DeliveryDetailsViewModel
            {
                Customer = customer,
                BillingAddress = billingAddress,
                ShippingOption = shippingOption
            };

            return viewModel;
        }


        /// <summary>
        /// Creates view model for checkout preview step.
        /// </summary>
        /// <param name="paymentMethod">Payment method selected on preview step</param>
        public PreviewViewModel PreparePreviewViewModel(PaymentMethodViewModel paymentMethod = null)
        {
            var cart = mShoppingService.GetCurrentShoppingCart();
            var billingAddress = mShoppingService.GetBillingAddress();
            var shippingOption = cart.ShippingOption;
            var paymentMethods = CreatePaymentMethodList(cart);

            paymentMethod = paymentMethod ?? new PaymentMethodViewModel(cart.PaymentOption, paymentMethods);

            // PaymentMethods are excluded from automatic binding and must be recreated manually after post action
            paymentMethod.PaymentMethods = paymentMethod.PaymentMethods ?? paymentMethods;

            var deliveryDetailsModel = new DeliveryDetailsViewModel
            {
                Customer = new CustomerViewModel(cart.Customer),
                BillingAddress = new BillingAddressViewModel(billingAddress, null, mCountryRepository),
                ShippingOption = new ShippingOptionViewModel(shippingOption, null, cart.IsShippingNeeded)
            };

            var cartModel = new CartViewModel(cart);

            var viewModel = new PreviewViewModel
            {
                CartModel = cartModel,
                DeliveryDetails = deliveryDetailsModel,
                ShippingName = shippingOption?.ShippingOptionDisplayName ?? "",
                PaymentMethod = paymentMethod
            };

            return viewModel;
        }


        /// <summary>
        /// Creates view model for Shopping cart step.
        /// </summary>
        public CartViewModel PrepareCartViewModel()
        {
            var cart = mShoppingService.GetCurrentShoppingCart();
            var appliedCouponCodes = cart.CouponCodes?.AllAppliedCodes?.Select(x => x.Code);
            var cartViewModel = new CartViewModel(cart);

            if (appliedCouponCodes != null)
            {
                cartViewModel.AppliedCouponCodes = appliedCouponCodes;
            }

            return cartViewModel;
        }


        private SelectList CreatePaymentMethodList(ShoppingCartInfo cart)
        {
            var paymentMethods = GetApplicablePaymentMethods(cart);

            return new SelectList(paymentMethods, "PaymentOptionID", "PaymentOptionDisplayName");
        }


        private IEnumerable<PaymentOptionInfo> GetApplicablePaymentMethods(ShoppingCartInfo cart)
        {
            return mPaymentMethodRepository.GetAll().Where(paymentMethod => PaymentOptionInfoProvider.IsPaymentOptionApplicable(cart, paymentMethod));
        }


        private SelectList CreateCountryList()
        {
            var allCountries = mCountryRepository.GetAllCountries();
            return new SelectList(allCountries, "CountryID", "CountryDisplayName");
        }


        private SelectList CreateShippingOptionList()
        {
            var shippingOptions = mShippingOptionRepository.GetAllEnabled();
            var cart = mShoppingService.GetCurrentShoppingCart();

            var selectList = shippingOptions.Select(s =>
            {
                var shippingPrice = mShoppingService.CalculateShippingOptionPrice(s);
                var currency = cart.Currency;

                return new SelectListItem
                {
                    Value = s.ShippingOptionID.ToString(),
                    Text = $"{s.ShippingOptionDisplayName} ({String.Format(currency.CurrencyFormatString, shippingPrice)})"
                };
            });

            return new SelectList(selectList, "Value", "Text");
        }
    }
}