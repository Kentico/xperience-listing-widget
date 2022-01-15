using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    /// <summary>
    /// Provides methods for retrieving pages.
    /// </summary>
    public class PageRepository : IPageRepository
    {
        private readonly IPageRetriever pageRetriever;


        /// <summary>
        /// Initializes a new instance of the <see cref="PageRepository"/> class that returns pages. 
        /// </summary>
        /// <param name="pageRetriever">Retriever for pages based on given parameters.</param>
        public PageRepository(IPageRetriever pageRetriever)
        {
            this.pageRetriever = pageRetriever;
        }


        /// <summary>
        /// Returns page name.
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


        /// <summary>
        /// Retrieves pages of specified type.
        /// </summary>
        /// <param name="pageType">Page type of pages to be retrieved.</param>
        /// <param name="parentPageAliasPath">Parent path for child pages to be retrieved. If not specified all pages for the current site will be retrieved.</param>
        public IEnumerable<TreeNode> GetPages(string pageType, string parentPageAliasPath = null)
        {
            return pageRetriever.Retrieve(
                pageType,
                query => query
                    .Path(parentPageAliasPath, PathTypeEnum.Children)
                    .FilterDuplicates(),
                cache => cache
                    .Key($"{nameof(PageRepository)}|{nameof(GetPages)}|{pageType}|{parentPageAliasPath}")
                    // Includes dependency to flush cache when any page of selected page type is edited/created/deleted.
                    .Dependencies((_, builder) => builder.Pages(pageType)));
        }
    }
}
