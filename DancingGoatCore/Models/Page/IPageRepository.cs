using System.Collections.Generic;

using CMS.DocumentEngine;

namespace DancingGoat.Models
{
    /// <summary>
    /// Provides methods for retrieving pages.
    /// </summary>
    public interface IPageRepository
    {
        /// <summary>
        /// Returns a page of specified page type.
        /// </summary>
        /// <typeparam name="TPageType">Type of the page to be retrieved.</typeparam>
        /// <param name="PageAliasPath">Specifies the path of retrieved page.</param>
        public TPageType GetPage<TPageType>(string PageAliasPath) where TPageType : TreeNode, new();


        /// <summary>
        /// Retrieve pages of specified type.
        /// </summary>
        /// <param name="className">Class name defining the type of pages to be retrieved.</param>
        /// <param name="parentPageAliasPath">Specifies the path from which retrieves child pages excluding the parent page. Not specifying it will retrieve all pages.</param>
        public IEnumerable<TreeNode> GetPages(string className, string parentPageAliasPath = null);
    }
}
