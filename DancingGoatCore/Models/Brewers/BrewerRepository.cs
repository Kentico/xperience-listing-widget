using System.Collections.Generic;

using CMS.DocumentEngine.Types.DancingGoatCore;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    /// <summary>
    /// Represents a collection of brewers.
    /// </summary>
    public class BrewerRepository
    {
        private readonly IPageRetriever pageRetriever;
        

        /// <summary>
        /// Initializes a new instance of the <see cref="BrewerRepository"/> class that returns brewers.
        /// </summary>
        /// <param name="pageRetriever">Retriever for pages based on given parameters.</param>
        public BrewerRepository(IPageRetriever pageRetriever)
        {
            this.pageRetriever = pageRetriever;
        }


        /// <summary>
        /// Returns an enumerable collection of brewers ordered by the date of publication.
        /// </summary>
        /// <param name="filter">Repository filter.</param>
        /// <param name="count">The number of brewers to return. Use 0 as value to return all records.</param>
        public IEnumerable<Brewer> GetBrewers(IRepositoryFilter filter, int count = 0)
        {
            return pageRetriever.Retrieve<Brewer>(
                query => query
                    .TopN(count)
                    .WhereTrue("SKUEnabled")
                    .Where(filter?.GetWhereCondition())
                    .FilterDuplicates()
                    .OrderByDescending("SKUInStoreFrom"),
                cache => cache
                    .Key($"{nameof(BrewerRepository)}|{nameof(GetBrewers)}|{filter?.GetCacheKey()}|{count}")
                    // Include dependency on all pages of this type to flush cache when a new page is created.
                    .Dependencies((_, builder) => builder.Pages(Brewer.CLASS_NAME).ObjectType("ecommerce.sku")));
        }
    }
}