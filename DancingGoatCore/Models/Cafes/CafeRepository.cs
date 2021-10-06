using System;
using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    /// <summary>
    /// Represents a collection of cafes.
    /// </summary>
    public partial class CafeRepository
    {
        private readonly IPageRetriever pageRetriever;


        /// <summary>
        /// Initializes a new instance of the <see cref="CafeRepository"/> class that returns cafes.
        /// </summary>
        /// <param name="pageRetriever">Retriever for pages based on given parameters.</param>
        public CafeRepository(IPageRetriever pageRetriever)
        {
            this.pageRetriever = pageRetriever;
        }


        /// <summary>
        /// Returns an enumerable collection of company cafes ordered by a position in the content tree.
        /// </summary>
        /// <param name="nodeAliasPath">The node alias path of the articles section in the content tree.</param>
        /// <param name="count">The number of cafes to return. Use 0 as value to return all records.</param>
        public IEnumerable<Cafe> GetCompanyCafes(string nodeAliasPath, int count = 0)
        {
            return pageRetriever.Retrieve<Cafe>(
                query => query
                    .Path(nodeAliasPath, PathTypeEnum.Children)
                    .TopN(count)
                    .WhereTrue("CafeIsCompanyCafe")
                    .OrderBy("NodeOrder"),
                cache => cache
                    .Key($"{nameof(CafeRepository)}|{nameof(GetCompanyCafes)}|{nodeAliasPath}|{count}")
                    // Include path dependency to flush cache when a new child page is created or page order is changed.
                    .Dependencies((_, builder) => builder.PagePath(nodeAliasPath, PathTypeEnum.Children).PageOrder()));
        }


        /// <summary>
        /// Returns a single cafe for the given <paramref name="guid"/>.
        /// </summary>
        /// <param name="guid">Node Guid.</param>
        public Cafe GetCafeByGuid(Guid guid)
        {
            return pageRetriever.Retrieve<Cafe>(
                query => query
                    .WhereEquals("NodeGUID", guid)
                    .TopN(1),
                cache => cache
                    .Key($"{nameof(CafeRepository)}|{nameof(GetCafeByGuid)}|{guid}"))
                .FirstOrDefault();
        }


        /// <summary>
        /// Returns an enumerable collection of partner cafes ordered by a position in the content tree.
        /// </summary>
        /// <param name="nodeAliasPath">The node alias path of the articles section in the content tree.</param>
        public IEnumerable<Cafe> GetPartnerCafes(string nodeAliasPath)
        {
            return pageRetriever.Retrieve<Cafe>(
              query => query
                  .Path(nodeAliasPath, PathTypeEnum.Children)
                  .WhereFalse("CafeIsCompanyCafe")
                  .OrderBy("NodeOrder"),
              cache => cache
                  .Key($"{nameof(CafeRepository)}|{nameof(GetPartnerCafes)}|{nodeAliasPath}")
                  // Include path dependency to flush cache when a new child page is created or page order is changed.
                  .Dependencies((_, builder) => builder.PagePath(nodeAliasPath, PathTypeEnum.Children).PageOrder()));
        }
    }
}