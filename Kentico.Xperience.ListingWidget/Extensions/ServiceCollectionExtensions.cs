using Kentico.Xperience.ListingWidget.Widgets;

using Microsoft.Extensions.DependencyInjection;

namespace Kentico.Xperience.ListingWidget
{
    public static class ServiceCollectionExtensions
    {
        public static void AddListingWidgetServices(this IServiceCollection services)
        {
            services.AddSingleton<IPageRepository, PageRepository>();
            services.AddSingleton<IOrderByFieldEditorService, OrderByFieldEditorService>();
            services.AddSingleton<ITransformationStrategy, TransformationStrategy>();
            services.AddSingleton<ITransformationEditorService, TransformationEditorService>();
            services.AddSingleton<IPageTypeEditorService, PageTypeEditorService>();
            services.AddSingleton<ListingWidgetTransformationsRetriever>();
            services.AddSingleton<ListingWidgetPageTypesRetriever>();
        }
    }
}
