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
        /// Returns a page of specified page type.
        /// </summary>
        /// <typeparam name="TPageType">Type of the page to be retrieved.</typeparam>
        /// <param name="PageAliasPath">Specifies the path of retrieved page.</param>
        public TPageType GetPage<TPageType>(string PageAliasPath) where TPageType : TreeNode, new()
        {
            return pageRetriever.Retrieve<TPageType>(
                query => query
                    .Path(PageAliasPath, PathTypeEnum.Single),
                cache => cache
                    .Key($"{nameof(PageRepository)}|{nameof(GetPage)}|{nameof(TPageType)}|{PageAliasPath}"))
                .FirstOrDefault();
        }


        /// <summary>
        /// Retrieves pages of specified type.
        /// </summary>
        /// <param name="className">Class name defining the type of pages to be retrieved.</param>
        /// <param name="parentPageAliasPath">Parent path for child pages to be retrieved. If not specified all pages for the current site will be retrieved.</param>
        public IEnumerable<TreeNode> GetPages(string className, string parentPageAliasPath = null)
        {
            return pageRetriever.Retrieve(
                className,
                query => query
                    .Path(parentPageAliasPath, PathTypeEnum.Children)
                    .FilterDuplicates(),
                cache => cache
                    .Key($"{nameof(PageRepository)}|{nameof(GetPages)}|{className}|{parentPageAliasPath}")
                    // Includes dependency to flush cache when any page of selected page type is edited/created/deleted.
                    .Dependencies((_, builder) => builder.Pages(className)));
        }
    }
}
