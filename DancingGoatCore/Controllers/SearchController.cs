using System;
using System.Collections.Generic;
using System.Linq;

using CMS.Base;
using CMS.Membership;
using CMS.Search;
using CMS.WebAnalytics;

using DancingGoat.Infrastructure;
using DancingGoat.Models;

using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace DancingGoat.Controllers
{
    public class SearchController : Controller
    {
        private const string INDEX_NAME = "DancingGoatCore.Index";
        private const int PAGE_SIZE = 5;
        private const int DEFAULT_PAGE_NUMBER = 1;

        private readonly TypedSearchItemViewModelFactory searchItemViewModelFactory;
        private readonly IPagesActivityLogger pagesActivityLogger;
        private readonly IAnalyticsLogger analyticsLogger;
        private readonly ISiteService siteService;


        public SearchController(TypedSearchItemViewModelFactory searchItemViewModelFactory, IPagesActivityLogger pagesActivityLogger,
            IAnalyticsLogger analyticsLogger, ISiteService siteService)
        {
            this.searchItemViewModelFactory = searchItemViewModelFactory;
            this.pagesActivityLogger = pagesActivityLogger;
            this.analyticsLogger = analyticsLogger;
            this.siteService = siteService;
        }


        // GET: Search
        public ActionResult Index(string searchText, int page = DEFAULT_PAGE_NUMBER)
        {
            if (String.IsNullOrWhiteSpace(searchText))
            {
                var empty = new SearchResultsModel
                {
                    Items = new List<SearchResultItemModel>()
                };
                return View(empty);
            }

            // Validate page number (starting from 1)
            page = Math.Max(page, DEFAULT_PAGE_NUMBER);

            var searchParameters = SearchParameters.PrepareForPages(searchText, new[] { INDEX_NAME }, page, PAGE_SIZE, MembershipContext.AuthenticatedUser);
            var searchResults = SearchHelper.Search(searchParameters);

            pagesActivityLogger.LogInternalSearch(searchText);

            var siteId = siteService.CurrentSite.SiteID;
            var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
            var hostAddress = HttpContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString();
            var uri = new Uri(HttpContext.Request.GetEncodedUrl());

            analyticsLogger.LogInternalSearchKeywords(new AnalyticsData(siteId, searchText, culture: culture, uri: uri, hostAddress: hostAddress));

            var searchResultItemModels = searchResults.Items
                .Select(searchItemViewModelFactory.GetTypedSearchResultItemModel);

            var model = new SearchResultsModel
            {
                Items = searchResultItemModels.ToList(),
                Query = searchText,
                CurrentPage = page,
                NumberOfPages = (int)Math.Ceiling(searchResults.TotalNumberOfResults / (double)PAGE_SIZE)
            };

            return View(model);
        }
    }
}