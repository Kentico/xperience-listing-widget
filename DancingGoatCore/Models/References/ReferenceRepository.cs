using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    /// <summary>
    /// Encapsulates access to references.
    /// </summary>
    public class ReferenceRepository
    {
        private readonly IPageRetriever pageRetriever;


        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceRepository"/> class.
        /// </summary>
        /// <param name="pageRetriever">Retriever for pages based on given parameters.</param>
        public ReferenceRepository(IPageRetriever pageRetriever)
        {
            this.pageRetriever = pageRetriever;
        }


        /// <summary>
        /// Asynchronously returns an enumerable collection of references ordered by the node order.
        /// </summary>
        /// <param name="nodeAliasPath">The node alias path of the parent page in the content tree.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <param name="count">The number of references to return. Use 0 as value to return all records.</param>
        public Task<IEnumerable<Reference>> GetReferencesAsync(string nodeAliasPath, CancellationToken cancellationToken, int count = 0)
        {
            return pageRetriever.RetrieveAsync<Reference>(
                query => query
                    .Path(nodeAliasPath, PathTypeEnum.Children)
                    .TopN(count)
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(ReferenceRepository)}|{nameof(GetReferencesAsync)}|{nodeAliasPath}|{count}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(nodeAliasPath, PathTypeEnum.Children).PageOrder()),
                cancellationToken);
        }
    }
}