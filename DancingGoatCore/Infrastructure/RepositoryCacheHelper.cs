using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.SiteProvider;

using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;

using Microsoft.AspNetCore.Http;

namespace DancingGoat.Infrastructure
{
    public class RepositoryCacheHelper
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IProgressiveCache cache;


        public RepositoryCacheHelper(IHttpContextAccessor httpContextAccessor, IProgressiveCache cache)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.cache = cache;
        }


        public IEnumerable<TObjectType> CacheObjects<TObjectType>(Func<IEnumerable<TObjectType>> getData, string baseCacheKey, string[] dependencyCacheKeys = null)
            where TObjectType : BaseInfo
        {
            if (!IsCacheEnabled())
            {
                return getData?.Invoke();
            }

            Func<CacheSettings, IEnumerable<TObjectType>> provideData = (cacheSettings) =>
            {
                var result = getData?.Invoke();
                if (result == null || !result.Any())
                {
                    cacheSettings.CacheMinutes = 0;
                    return result;
                }

                var objects = result.ToList();
                cacheSettings.CacheDependency = GetCacheDependency(dependencyCacheKeys, objects);
                return objects;
            };

            var cacheKey = GetCacheItemKey(baseCacheKey);
            var cacheSettings = CreateCacheSettings(cacheKey);
            return cache.Load(provideData, cacheSettings);
        }


        public TObjectType CacheObject<TObjectType>(Func<TObjectType> getData, string baseCacheKey, string[] dependencyCacheKeys = null)
            where TObjectType : BaseInfo
        {
            if (!IsCacheEnabled())
            {
                return getData?.Invoke();
            }

            
            Func<CacheSettings, TObjectType> provideData = (cacheSettings) =>
            {
                var result = getData?.Invoke();
                if (result == null)
                {
                    cacheSettings.CacheMinutes = 0;
                    return null;
                }

                cacheSettings.CacheDependency = GetCacheDependency(dependencyCacheKeys, new[] { result });
                return result;
            };

            var cacheKey = GetCacheItemKey(baseCacheKey);
            var cacheSettings = CreateCacheSettings(cacheKey);
            return cache.Load(provideData, cacheSettings);
        }


        public TPageType CachePage<TPageType>(Func<TPageType> getData, string baseCacheKey, string[] dependencyCacheKeys = null)
            where TPageType : TreeNode
        {
            if (!IsCacheEnabled())
            {
                return getData?.Invoke();
            }

            Func<CacheSettings, TPageType> provideData = (cacheSettings) =>
            {
                var result = getData?.Invoke();
                if (result == null)
                {
                    cacheSettings.CacheMinutes = 0;
                    return null;
                }

                cacheSettings.CacheMinutes = GetMinutes(result.DocumentPublishTo, cacheSettings.CacheMinutes);
                cacheSettings.CacheDependency = GetPagesCacheDependency(dependencyCacheKeys, new[] { result });
                return result;
            };

            var cacheKey = GetCacheItemKey(baseCacheKey);
            var cacheSettings = CreateCacheSettings(cacheKey);
            return cache.Load(provideData, cacheSettings);
        }


        public IEnumerable<TPageType> CachePages<TPageType>(Func<IEnumerable<TPageType>> getData, string baseCacheKey, string[] dependencyCacheKeys = null)
            where TPageType : TreeNode
        {
            if (!IsCacheEnabled())
            {
                return getData?.Invoke();
            }


            Func<CacheSettings, IEnumerable<TPageType>> provideData = (cacheSettings) =>
            {
                var result = getData?.Invoke();
                if (result == null || !result.Any())
                {
                    cacheSettings.CacheMinutes = 0;
                    return result;
                }

                var pages = result.ToList();
                var earliestPublishTo = pages
                    .Where(treeNode => treeNode.DocumentPublishTo >= DateTime.Now)
                    .Min(treeNode => treeNode.DocumentPublishTo);

                cacheSettings.CacheMinutes = GetMinutes(earliestPublishTo, cacheSettings.CacheMinutes);
                cacheSettings.CacheDependency = GetPagesCacheDependency(dependencyCacheKeys, pages);
                return pages;
            };

            var cacheKey = GetCacheItemKey(baseCacheKey);
            var cacheSettings = CreateCacheSettings(cacheKey);
            return cache.Load(provideData, cacheSettings);
        }


        private CMSCacheDependency GetCacheDependency(string[] dependencyCacheKeys, IEnumerable<BaseInfo> objects)
        {
            var objectType = objects.First().TypeInfo.ObjectType;
            dependencyCacheKeys = dependencyCacheKeys ?? new[]
            {
                CacheDependencyKeyProvider.GetDependencyCacheKeyForObjectType(objectType) }
            ;

            return CacheHelper.GetCacheDependency(dependencyCacheKeys);
        }


        private CMSCacheDependency GetPagesCacheDependency(string[] dependencyCacheKeys, IEnumerable<TreeNode> pages)
        {
            var className = pages.First().ClassName;
            dependencyCacheKeys = dependencyCacheKeys ?? new[]
            {
                CacheDependencyKeyProvider.GetDependencyCacheKeyForPageType(SiteContext.CurrentSiteName, className),
                CacheDependencyKeyProvider.GetDependencyCacheKeyForObjectType(className)
            };

            var keys = new HashSet<string>(dependencyCacheKeys, StringComparer.OrdinalIgnoreCase);
            var pathKeys = pages.Where(page => page.HasUrl())
                .Select(page => page.NodeAliasPath)
                .SelectMany(path => DocumentDependencyCacheKeysBuilder.GetParentPathsDependencyCacheKeys(SiteContext.CurrentSiteName, path))
                .Distinct();

            foreach (var pathKey in pathKeys)
            {
                keys.Add(pathKey);
            }

            return CacheHelper.GetCacheDependency(keys);
        }


        private double GetMinutes(DateTime publishTo, double minutes)
        {
            if (publishTo == DateTime.MaxValue || publishTo < DateTime.Now)
            {
                return minutes;
            }

            var minutesToInvalidation = (publishTo - DateTime.Now).Minutes;
            return Math.Min(minutesToInvalidation, minutes);
        }


        private string GetCacheItemKey(string baseCacheKey)
        {
            var builder = new StringBuilder(127)
                          .Append(baseCacheKey)
                          .Append("|").Append(SiteContext.CurrentSiteName)
                          .Append("|").Append(CultureInfo.CurrentCulture.Name);

            return builder.ToString();
        }


        private CacheSettings CreateCacheSettings(string cacheKey)
        {
            return new CacheSettings(CacheHelper.CacheMinutes(SiteContext.CurrentSiteName), cacheKey);
        }


        private bool IsCacheEnabled()
        {
            return !IsPreviewEnabled();
        }


        private bool IsPreviewEnabled()
        {
            return httpContextAccessor.HttpContext.Kentico().Preview().Enabled;
        }
    }
}