﻿using System.Collections.Generic;

using CMS.DocumentEngine;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
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


        public IEnumerable<TPageType> GetAllPages<TPageType>() where TPageType : TreeNode, new()
        {
            return pageRetriever.Retrieve<TPageType>(
                query => query
                    .FilterDuplicates(),
                cache => cache
                    .Key($"{nameof(PageRepository)}|{nameof(GetAllPages)}"));
        }
    }
}