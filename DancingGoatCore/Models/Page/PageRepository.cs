using System;
using System.Collections.Generic;
using System.Linq;

using CMS.DataEngine;
using CMS.DocumentEngine;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    /// <inheritdoc/>
    public class PageRepository : IPageRepository
    {
        private readonly IPageRetriever pageRetriever;


        /// <summary>
        /// Creates a new instance of the <see cref="PageRepository"/> class that returns pages. 
        /// </summary>
        /// <param name="pageRetriever">Retriever for pages.</param>
        public PageRepository(IPageRetriever pageRetriever)
        {
            this.pageRetriever = pageRetriever;
        }


        /// <summary>
        /// Gets name of the page.
        /// </summary>
        /// <param name="PageAliasPath">Specifies the path of the page.</param>
        public string GetPageName(string pageAliasPath)
        {
            return pageRetriever.Retrieve<TreeNode>(
                 query => query
                     .Column("DocumentName")
                     .Path(pageAliasPath),
                 cache => cache
                     .Key($"{nameof(PageRepository)}|{nameof(GetPageName)}|{pageAliasPath}"))
                 .FirstOrDefault()
                 ?.DocumentName;
        }


        /// <inheritdoc/>
        public IEnumerable<TreeNode> GetPages(string pageType, string parentPageAliasPath = null, int topN = 0, string orderByField = null, OrderDirection orderDirection = OrderDirection.Default, Action<DocumentQuery> customQuery = null, string customDependencyKey = null, Action<IPageCacheDependencyBuilder<TreeNode>, IEnumerable<TreeNode>> customDependency = null)
        {
            return pageRetriever.Retrieve(
                pageType,
                query =>
                    {
                        customQuery?.Invoke(query);
                        query.Path(parentPageAliasPath, PathTypeEnum.Children)
                         .TopN(topN)
                         .OrderBy(orderDirection, orderByField)
                         .FilterDuplicates();
                    },
                cache => cache
                    .Key($"{nameof(PageRepository)}|{nameof(GetPages)}|{pageType}|{parentPageAliasPath}|{topN}|{orderByField}|{orderDirection}{(customDependencyKey == null ? "|" + customDependencyKey : "")}")
                    // Includes dependency to flush cache when any page of selected page type is edited/created/deleted.
                    .Dependencies((pages, builder) =>
                        {
                            builder.Pages(pageType);
                            customDependency?.Invoke(builder, pages);
                        }
                    ));
        }
    }
}
