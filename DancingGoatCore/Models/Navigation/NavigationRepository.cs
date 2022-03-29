using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;
using CMS.SiteProvider;

using DancingGoat.Infrastructure;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    /// <summary>
    /// Represents a collection of navigation items.
    /// </summary>
    public class NavigationRepository
    {
        private readonly IPageRetriever pageRetriever;
        private readonly RepositoryCacheHelper repositoryCacheHelper;


        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationRepository"/> class.
        /// </summary>
        /// <param name="pageRetriever">The page retriever.</param>
        /// <param name="repositoryCacheHelper">Handles caching of retrieved objects.</param>
        public NavigationRepository(IPageRetriever pageRetriever, RepositoryCacheHelper repositoryCacheHelper)
        {
            this.pageRetriever = pageRetriever;
            this.repositoryCacheHelper = repositoryCacheHelper;
        }


        /// <summary>
        /// Returns an enumerable collection of menu items ordered by the content tree order and level.
        /// </summary>
        public IEnumerable<TreeNode> GetMenuItems()
        {
            return pageRetriever.Retrieve<TreeNode>(
                query => query
                    .FilterDuplicates()
                    .OrderByAscending("NodeLevel", "NodeOrder")
                    .MenuItems(),
                cache => cache
                    .Key($"{nameof(NavigationRepository)}|{nameof(GetMenuItems)}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath("/", PathTypeEnum.Children).ObjectType("cms.documenttype").PageOrder()));
        }


        /// <summary>
        /// Returns an enumerable collection of footer navigation items.
        /// </summary>
        public IEnumerable<TreeNode> GetFooterNavigationItems()
        {
            return repositoryCacheHelper.CachePages(() =>
            {
                var footerNavigationPage = pageRetriever.Retrieve<FooterNavigation>(
                    query => query
                        .Path(ContentItemIdentifiers.FOOTER_NAVIGATION, PathTypeEnum.Single)
                        .TopN(1))
                    .First();

                return footerNavigationPage.Fields.NavigationItems;
            }, $"{nameof(NavigationRepository)}|{nameof(GetFooterNavigationItems)}", new[] 
            {
                CacheDependencyKeyProvider.GetDependencyCacheKeyForObjectType("cms.adhocrelationship"), 
                $"node|{SiteContext.CurrentSiteName}|/|childnodes"
            });
        }
    }
}