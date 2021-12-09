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
        /// Returns an enumerable collection of pages of specified type.
        /// </summary>
        /// <typeparam name="TPageType">Type of the pages to be retrieved.</typeparam>
        public IEnumerable<TPageType> GetAllPages<TPageType>() where TPageType : TreeNode, new();
    }
}
