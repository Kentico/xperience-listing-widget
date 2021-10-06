using System.Collections.Generic;

using CMS.DocumentEngine.Types.DancingGoatCore;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    /// <summary>
    /// Represents a collection of links to social networks.
    /// </summary>
    public class SocialLinkRepository
    {
        private readonly IPageRetriever pageRetriever;


        /// <summary>
        /// Initializes a new instance of the <see cref="SocialLinkRepository"/> class that returns links.
        /// </summary>
        /// <param name="pageRetriever">Retriever for pages based on given parameters.</param>
        public SocialLinkRepository(IPageRetriever pageRetriever)
        {
            this.pageRetriever = pageRetriever;
        }


        /// <summary>
        /// Returns an enumerable collection of links to social networks ordered by a position in the content tree.
        /// </summary>
        public IEnumerable<SocialLink> GetSocialLinks()
        {
            return pageRetriever.Retrieve<SocialLink>(
                query => query
                    .OrderByAscending("NodeOrder"),
                cache => cache
                    .Key($"{nameof(SocialLinkRepository)}|{nameof(GetSocialLinks)}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.Pages(SocialLink.CLASS_NAME).PageOrder()));
        }
    }
}