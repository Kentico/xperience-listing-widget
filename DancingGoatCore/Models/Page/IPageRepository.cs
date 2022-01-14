using System.Collections.Generic;

using CMS.DataEngine;
using CMS.DocumentEngine;

namespace DancingGoat.Models
{
    /// <summary>
    /// Provides methods for retrieving pages.
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
        public IEnumerable<TreeNode> GetPages(string className, string parentPageAliasPath = null, int topN = 0, string orderByField = null, OrderDirection orderDirection = OrderDirection.Default);
    }
}
