using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;
using CMS.Ecommerce;
using CMS.SiteProvider;

using DancingGoat.Infrastructure;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    /// <summary>
    /// Encapsulates access to hot tips.
    /// </summary>
    public class HotTipsRepository
    {
        private readonly IPageRetriever pageRetriever;
        private readonly RepositoryCacheHelper repositoryCacheHelper;


        /// <summary>
        /// Initializes a new instance of the <see cref="HotTipsRepository"/> class that returns hot tips.
        /// </summary>
        /// <param name="pageRetriever">Retriever for pages based on given parameters.</param>
        /// <param name="repositoryCacheHelper">Handles caching of retrieved objects.</param>
        public HotTipsRepository(IPageRetriever pageRetriever, RepositoryCacheHelper repositoryCacheHelper)
        {
            this.pageRetriever = pageRetriever;
            this.repositoryCacheHelper = repositoryCacheHelper;
        }


        /// <summary>
        /// Returns collection of products categorized under Hot tips page.
        /// </summary>
        /// <param name="parentAliasPath">Parent node alias path.</param>
        public IEnumerable<SKUTreeNode> GetHotTipProducts(string parentAliasPath)
        {
            return repositoryCacheHelper.CachePages(() =>
            {
                var hotTipsPage = pageRetriever.Retrieve<HotTips>(
                    query => query
                        .Path($"{parentAliasPath}/Hot-tips", PathTypeEnum.Single)
                        .TopN(1))
                    .FirstOrDefault();

                return hotTipsPage?.Fields.HotTips
                    .OfType<SKUTreeNode>() ?? Enumerable.Empty<SKUTreeNode>();
            }, $"{nameof(HotTipsRepository)}|{nameof(GetHotTipProducts)}|{parentAliasPath}", new[] 
            {
                CacheDependencyKeyProvider.GetDependencyCacheKeyForObjectType("cms.adhocrelationship"),
                CacheDependencyKeyProvider.GetDependencyCacheKeyForObjectType("ecommerce.sku"), 
                $"node|{SiteContext.CurrentSiteName}|{parentAliasPath}"
            });
        }
    }
}