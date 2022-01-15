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
        /// Gets name of the page.
        /// </summary>
        /// <param name="PageAliasPath">Specifies the path of the page.</param>
        public string GetPageName(string pageAliasPath);


        /// <summary>
        /// Gets a collection of specified page type pages.
        /// </summary>
        /// <param name="pageType">Class name defining the type of pages to be retrieved.</param>
        /// <param name="parentPageAliasPath">Parent path for child pages to be retrieved. If not specified all pages for the current site will be retrieved.</param>
        public IEnumerable<TreeNode> GetPages(string pageType, string parentPageAliasPath = null);
    }
}
