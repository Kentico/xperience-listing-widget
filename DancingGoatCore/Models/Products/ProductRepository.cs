using System;
using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.SiteProvider;

using DancingGoat.Infrastructure;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    /// <summary>
    /// Represents a collection of products.
    /// </summary>
    public class ProductRepository
    {
        private readonly IPageRetriever pageRetriever;
        private readonly ISKUInfoProvider skuInfoProvider;
        private readonly RepositoryCacheHelper repositoryCacheHelper;


        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class that returns products.
        /// </summary>
        /// <param name="pageRetriever">Retriever for pages based on given parameters.</param>
        /// <param name="skuInfoProvider">Provider for <see cref="SKUInfo"/> management.</param>
        /// <param name="repositoryCacheHelper">Handles caching of retrieved objects.</param>
        public ProductRepository(IPageRetriever pageRetriever, ISKUInfoProvider skuInfoProvider, RepositoryCacheHelper repositoryCacheHelper)
        {
            this.pageRetriever = pageRetriever;
            this.skuInfoProvider = skuInfoProvider;
            this.repositoryCacheHelper = repositoryCacheHelper;
        }


        /// <summary>
        /// Returns the product with the specified identifier.
        /// </summary>
        /// <param name="nodeGUID">The product node identifier.</param>
        /// <returns>The product with the specified node identifier, if found; otherwise, null.</returns>
        public SKUTreeNode GetProduct(Guid nodeGUID)
        {
            return repositoryCacheHelper.CachePage(() =>
            {
                var page = pageRetriever.Retrieve<TreeNode>(
                    query => query
                        .WhereEquals("NodeGUID", nodeGUID))
                    .FirstOrDefault();

                if ((page == null) || !page.IsProduct())
                {
                    return null;
                }

                // Load product type specific fields from the database
                page.MakeComplete(true);

                return page as SKUTreeNode;
            }, $"{nameof(ProductRepository)}|{nameof(GetProduct)}|{nodeGUID}", new[] 
            {
                CacheDependencyKeyProvider.GetDependencyCacheKeyForObjectType("ecommerce.sku"), 
                $"nodeguid|{SiteContext.CurrentSiteName}|{nodeGUID}" 
            });
        }


        /// <summary>
        /// Returns the product with the specified SKU identifier.
        /// </summary>
        /// <param name="skuId">The product or variant SKU identifier.</param>
        public SKUTreeNode GetProductForSKU(int skuId)
        {
            var sku = skuInfoProvider.Get(skuId);
            if ((sku == null) || sku.IsProductOption)
            {
                return null;
            }

            if (sku.IsProductVariant)
            {
                skuId = sku.SKUParentSKUID;
            }

            return pageRetriever.Retrieve<TreeNode>(
                query => query
                    .WhereEquals("NodeSKUID", skuId),
                cache => cache
                    .Key($"{nameof(ProductRepository)}|{nameof(GetProductForSKU)}|{skuId}")
                    .Dependencies((_, builder) => builder.ObjectType("ecommerce.sku")))
                .FirstOrDefault() as SKUTreeNode;
        }


        /// <summary>
        /// Returns an enumerable collection of products ordered by the date of publication.
        /// </summary>
        /// <param name="statusId">The products with status identifier.</param>
        /// <param name="count">The number of products to return. Use 0 as value to return all records.</param>
        public IEnumerable<SKUTreeNode> GetProductsByStatus(int statusId, int count = 0)
        {
            return pageRetriever.Retrieve<TreeNode>(
                query => query
                    .TopN(count)
                    .WhereEquals("SKUPublicStatusID", statusId)
                    .OrderByDescending("SKUInStoreFrom")
                    .WhereTrue("SKUEnabled")
                    .FilterDuplicates(),
                cache => cache
                    .Key($"{nameof(ProductRepository)}|{nameof(GetProductsByStatus)}|{statusId}|{count}")
                    .Dependencies((_, builder) => builder.ObjectType("ecommerce.sku").Custom($"ecommerce.publicstatus|byid|{statusId}")))
                .Select(page => page as SKUTreeNode);

        }


        /// <summary>
        /// Returns an enumerable collection of products.
        /// </summary>        
        /// <param name="filter">Repository filter.</param>
        /// <param name="classNames">Class names of products to retrieve.</param>
        public IEnumerable<SKUTreeNode> GetProducts(IRepositoryFilter filter, params string[] classNames)
        {
            return pageRetriever.Retrieve<TreeNode>(
                query => query
                    .WhereIn("ClassName", classNames)
                    .Where(filter?.GetWhereCondition())
                    .WhereTrue("SKUEnabled")
                    .FilterDuplicates(),
                cache => cache
                    .Key($"{nameof(ProductRepository)}|{nameof(GetProducts)}|{filter?.GetCacheKey()}|{string.Join(";", classNames)}")
                    .Dependencies((_, builder) => builder.ObjectType("ecommerce.sku")))
                .Select(page => page as SKUTreeNode);
        }


        /// <summary>
        /// Returns an enumerable products collection from the given path ordered by <see cref="TreeNode.NodeOrder"/>.
        /// </summary>
        /// <param name="parentPageAliasPath">Parent page alias path.</param>
        /// <param name="count">The number of products to return. Use 0 as value to return all records.</param>
        public IEnumerable<SKUTreeNode> GetProducts(string parentPageAliasPath, int count = 0)
        {
            return pageRetriever.Retrieve<TreeNode>(
                query => query
                    .TopN(count)
                    .Path(parentPageAliasPath, PathTypeEnum.Children)
                    .OrderByAscending("NodeOrder")
                    .WhereTrue("SKUEnabled")
                    .FilterDuplicates(),
                cache => cache
                    .Key($"{nameof(ProductRepository)}|{nameof(GetProducts)}|{parentPageAliasPath}|{count}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(parentPageAliasPath, PathTypeEnum.Children).ObjectType("ecommerce.sku").PageOrder()))
                .Select(page => page as SKUTreeNode);
        }
    }
}