# General listing widget 

The general listing widget enables a content editor to display a customizable list of pages. 

Content editor can specify: 
- **Page type** - Type of pages to be displayed.
- **Transformation** - Layout of the listing.
- **Parent page** - Path to a page to display children for. The parent page itself is not included in the listing.
- **Select top N pages** - Number of pages to be displayed.
- **Order by field** - Field to order by the pages.
- **Order** - Order direction of the listing.

![Listing widget](/ListingWidget.gif)

## How to use Listing widget

1. Listing widget is placed in the **Kentico.Xperience.ListingWidget** project. Copy the project to your solution.
2. Add dependency to the listing widget project.
3. Set Xperience nugget version in the listing widget project same as it is on your live site. 
4. Ensure IoC for listing widget.
    - for example: 
        ```
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
        ```
5. Widget identifier is "Kentico.Xperience.ListingWidget", change your area restrictions according to it.
6. Add transformation services to your project and ensure IoC for them - see below "How to add a transformation". 
    - A preferred location for the transformation files is *{your_project}/ Components/Widgets/ListingWidget/Transformations/{TransformationPageType}*.
    - If your site is DancingGoat Core sample site, you can copy the transformations from the DancingGoatCore project.
7. Include css and js of the listing widget's in-line editors to your project (*Kentico.Xperience.ListingWidget/Assets/**/\*.css*, *Kentico.Xperience.ListingWidget/Assets/**/\*.css*).
    - If you want to use Grunt (see Gruntfile.js in the DancingGoatCore project):
        ```
        grunt.initConfig({
            concat: {
                pageBuilder: {
                    files: {
                        // Styles - admin
                        'wwwroot/Content/Bundles/Admin/pageComponents.css': ['wwwroot/PageBuilder/Admin/**/*.css', '../Kentico.Xperience.ListingWidget/Assets/**/*.css'],
                        // Scripts - admin
                        'wwwroot/Content/Bundles/Admin/pageComponents.js': ['wwwroot/PageBuilder/Admin/**/*.js', '../Kentico.Xperience.ListingWidget/Assets/**/*.js'],
                    }
                }
            },
        }
        ```

8. note: Kentico.Xperience.ListingWidget project has InternalsVisible settings for tests in the *Kentico.Xperience.ListingWidget/Properties/AssemblyInfo.cs*. 

## How to add a transformation

1. Create a **transformation view model** implementing *ITransformationViewModel*.
2. Create a **transformation razor view**. 
3. Create a **transformation service** implementing *BaseTransformationService*. The service should handle all transformations for a concrete type. 
    - Set the page type supported by transformation to the **PageType** field.
    - Add your new transformation to the **Transformations** collection.
    - Override other methods according to your needs.
4. Register the transformation service to IoC.
    - for example (see *DancingGoatCore/Helpers/IServiceCollectionExtensions.cs*):
        ```
        public static void AddListingWidgetTransformationServices(this IServiceCollection services)
        {
            services.AddSingleton<ITransformationService, ArticlesTransformationService>();
            services.AddSingleton<ITransformationService, CafesTransformationService>();
            services.AddSingleton<ITransformationService, CoffeesTransformationService>();
        }
        ```

See examples of the transformations in the corresponding destination in the DancingGoatCore project.