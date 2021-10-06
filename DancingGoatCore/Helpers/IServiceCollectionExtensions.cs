using DancingGoat.Infrastructure;
using DancingGoat.Models;
using DancingGoat.PageTemplates;
using DancingGoat.Services;

using Microsoft.Extensions.DependencyInjection;

namespace DancingGoat
{
    public static class IServiceCollectionExtensions
    {
        public static void AddDancingGoatServices(this IServiceCollection services)
        {
            AddViewComponentServices(services);

            AddRepositories(services);

            services.AddSingleton<TypedProductViewModelFactory>();
            services.AddSingleton<TypedSearchItemViewModelFactory>();
            services.AddSingleton<ICalculationService, CalculationService>();
            services.AddSingleton<ICheckoutService, CheckoutService>();
            services.AddSingleton<RepositoryCacheHelper>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddSingleton<ArticleRepository>();
            services.AddSingleton<CafeRepository>();
            services.AddSingleton<ContactRepository>();
            services.AddSingleton<CountryRepository>();
            services.AddSingleton<NavigationRepository>();
            services.AddSingleton<SocialLinkRepository>();
            services.AddSingleton<BrewerRepository>();
            services.AddSingleton<CoffeeRepository>();
            services.AddSingleton<ManufacturerRepository>();
            services.AddSingleton<PublicStatusRepository>();
            services.AddSingleton<ProductRepository>();
            services.AddSingleton<VariantRepository>();
            services.AddSingleton<HotTipsRepository>();
            services.AddSingleton<CustomerAddressRepository>();
            services.AddSingleton<ShippingOptionRepository>();
            services.AddSingleton<PaymentMethodRepository>();
            services.AddSingleton<AboutUsRepository>();
            services.AddSingleton<MediaFileRepository>();
            services.AddSingleton<ReferenceRepository>();
            services.AddSingleton<HomeRepository>();
            services.AddSingleton<OrderRepository>();
        }

        private static void AddViewComponentServices(IServiceCollection services)
        {
            services.AddSingleton<ArticleWithSidebarPageTemplateService>();
            services.AddSingleton<ArticlePageTemplateService>();
        }
    }
}
