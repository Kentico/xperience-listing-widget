using System.Threading.Tasks;

using CMS.Base;
using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Search;

using Microsoft.AspNetCore.Http;

namespace DancingGoat.Infrastructure
{
    /// <summary>
    /// Middleware to ensure proper smart search index initialization after installation or application deployment.
    /// </summary>
    internal class SmartSearchIndexRebuildMiddleware
    {
        private readonly object indexLock = new object();
        private static bool indexRebuilt = false;


        private readonly ISiteService siteService;
        private readonly ISearchIndexInfoProvider searchIndexInfoProvider;
        private readonly IEventLogService eventLogService;


        /// <summary>
        /// The <see cref="RequestDelegate"/> representing the next middleware in the pipeline.
        /// </summary>
        private readonly RequestDelegate next;


        /// <summary>
        /// Creates new instance of <see cref="SmartSearchIndexRebuildMiddleware"/>.
        /// </summary>
        /// <param name="next">
        /// The <see cref="RequestDelegate"/> representing the next middleware in the pipeline.
        /// </param>
        /// <param name="siteService">Site service.</param>
        /// <param name="searchIndexInfoProvider">Search index info provider.</param>
        public SmartSearchIndexRebuildMiddleware(RequestDelegate next, 
            ISiteService siteService, 
            ISearchIndexInfoProvider searchIndexInfoProvider,
            IEventLogService eventLogService)
        {
            this.next = next;
            this.siteService = siteService;
            this.searchIndexInfoProvider = searchIndexInfoProvider;
            this.eventLogService = eventLogService;
        }


        /// <summary>
        /// Invokes the logic of the middleware.
        /// </summary>
        /// <param name="context">
        /// The <see cref="HttpContext"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> that completes when the middleware has completed processing.
        /// </returns>
        public async Task InvokeAsync(HttpContext context)
        {
            if (!indexRebuilt)
            {
                lock (indexLock)
                {
                    if (!indexRebuilt)
                    {
                        var site = siteService.CurrentSite;
                        RebuildSiteIndexes(site.SiteName);

                        indexRebuilt = true;
                    }
                }
            }

            await next(context);
        }


        private void RebuildSiteIndexes(SiteInfoIdentifier siteIdentifier)
        {
            var indexes = searchIndexInfoProvider.Get().OnSite(siteIdentifier);

            foreach (var index in indexes)
            {
                RebuildIndex(index);
            }
        }


        private void RebuildIndex(SearchIndexInfo index)
        {
            try
            {
                // After installation or deployment the index is new but no web farm task is issued
                // to rebuild the index. This will ensure that index is rebuilt on instance upon first request.
                if (index.IndexStatusLocal == IndexStatusEnum.NEW)
                {
                    var taskCreationParameters = new SearchTaskCreationParameters
                    {
                        TaskType = SearchTaskTypeEnum.Rebuild,
                        TaskValue = index.IndexName,
                        RelatedObjectID = index.IndexID
                    };

                    taskCreationParameters.TargetServers.Add(WebFarmHelper.ServerName);

                    SearchTaskInfoProvider.CreateTask(taskCreationParameters, true);
                }
            }
            catch (SearchIndexException ex)
            {
                eventLogService.LogException("SmartSearch", "INDEX REBUILD", ex);
            }
        }
    }
}