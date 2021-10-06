using CMS.Ecommerce;
using CMS.SiteProvider;

using DancingGoat.Infrastructure;

namespace DancingGoat.Models
{
    /// <summary>
    /// Represents a contract for a collection of public statuses.
    /// </summary>
    public class PublicStatusRepository
    {
        private readonly IPublicStatusInfoProvider publicStatusInfoProvider;
        private readonly RepositoryCacheHelper repositoryCacheHelper;


        /// <summary>
        /// Initializes a new instance of the <see cref="PublicStatusRepository"/> class using the public status provider given.
        /// </summary>
        /// <param name="publicStatusInfoProvider">Provider for <see cref="PublicStatusInfo"/> management.</param>
        /// <param name="repositoryCacheHelper">Handles caching of retrieved objects.</param>
        public PublicStatusRepository(IPublicStatusInfoProvider publicStatusInfoProvider, RepositoryCacheHelper repositoryCacheHelper)
        {
            this.publicStatusInfoProvider = publicStatusInfoProvider;
            this.repositoryCacheHelper = repositoryCacheHelper;
        }


        /// <summary>
        /// Returns a public status with the specified name.
        /// </summary>
        /// <param name="name">The code name of the public status.</param>
        public PublicStatusInfo GetByName(string name)
        {
            return repositoryCacheHelper.CacheObject(() =>
            {
                return publicStatusInfoProvider.Get(name, SiteContext.CurrentSiteID);
            }, $"{nameof(PublicStatusRepository)}|{nameof(GetByName)}|{name}");
        }


        /// <summary>
        /// Returns a public status with the specified id.
        /// </summary>
        /// <param name="statusId">The id of the public status.</param>
        public PublicStatusInfo GetById(int statusId)
        {
            return repositoryCacheHelper.CacheObject(() =>
            {
                return publicStatusInfoProvider.Get(statusId);
            }, $"{nameof(PublicStatusRepository)}|{nameof(GetById)}|{statusId}");
        }
    }
}
