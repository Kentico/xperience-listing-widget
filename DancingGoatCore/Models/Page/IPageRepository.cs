using System;
using System.Collections.Generic;

using CMS.DataEngine;
using CMS.DocumentEngine;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    /// <summary>
    /// Encapsulates access to pages for listing widget.
    /// </summary>
    public interface IPageRepository
    {
        /// <summary>
        /// Gets name of the page.
        /// </summary>
        /// <param name="PageAliasPath">Specifies the path of the page.</param>
        public string GetPageName(string pageAliasPath);


        /// <summary>
        /// Retrieves pages of specified type.
        /// </summary>
        /// <param name="pageType">Page type of pages to be retrieved.</param>
        /// <param name="parentPageAliasPath">Parent path for child pages to be retrieved. If not specified, all pages will be retrieved for the current site.</param>
        /// <param name="topN">The number of pages to be retrieved.</param>
        /// <param name="orderByField">The field by which retrieved pages will be sorted.</param>
        /// <param name="orderDirection">Order direction of retrieved pages.</param>
        /// <param name="customQuery">Action for applying custom query parameters.</param>
        /// <param name="customDependencyKey">Custom cache dependency key.</param>
        /// <param name="customDependency">Action for applying custom cache dependency.</param>
        public IEnumerable<TreeNode> GetPages(string pageType, string parentPageAliasPath = null, int topN = 0, string orderByField = null, OrderDirection orderDirection = OrderDirection.Default, Action<DocumentQuery> customQuery = null, string customDependencyKey = null, Action<IPageCacheDependencyBuilder<TreeNode>, IEnumerable<TreeNode>> customDependency = null);
    }
}
