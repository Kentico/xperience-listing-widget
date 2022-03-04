using Microsoft.Extensions.DependencyInjection;

using Kentico.Xperience.ListingWidget.Models;
using Kentico.Xperience.ListingWidget.Widgets;

namespace Kentico.Xperience.ListingWidget
{
    public static class ListingWidgetIServiceCollectionExtension
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
