using System;
using System.Collections.Generic;

using CMS.MediaLibrary;
using CMS.SiteProvider;

using DancingGoat.Infrastructure;

namespace DancingGoat.Models
{
    /// <summary>
    /// Represents a collection of media files.
    /// </summary>
    public class MediaFileRepository
    {
        private readonly IMediaLibraryInfoProvider mediaLibraryInfoProvider;
        private readonly IMediaFileInfoProvider mediaFileInfoProvider;
        private readonly RepositoryCacheHelper repositoryCacheHelper;


        /// <summary>
        /// Initializes a new instance of the <see cref="MediaFileRepository"/> class.
        /// </summary>
        /// <param name="mediaLibraryInfoProvider">Provider for <see cref="MediaLibraryInfo"/> management.</param>
        /// <param name="mediaFileInfoProvider">Provider for <see cref="MediaFileInfo"/> management.</param>
        /// <param name="repositoryCacheHelper">Handles caching of retrieved objects.</param>
        public MediaFileRepository(IMediaLibraryInfoProvider mediaLibraryInfoProvider, IMediaFileInfoProvider mediaFileInfoProvider, RepositoryCacheHelper repositoryCacheHelper)
        {
            this.mediaLibraryInfoProvider = mediaLibraryInfoProvider;
            this.mediaFileInfoProvider = mediaFileInfoProvider;
            this.repositoryCacheHelper = repositoryCacheHelper;
        }


        /// <summary>
        /// Returns instance of <see cref="MediaFileInfo"/> specified by library name.
        /// </summary>
        /// <param name="mediaLibraryName">Name of the media library.</param>
        public MediaLibraryInfo GetByName(string mediaLibraryName)
        {
            return repositoryCacheHelper.CacheObject(() =>
            {
                return mediaLibraryInfoProvider.Get(mediaLibraryName, SiteContext.CurrentSiteID);
            }, $"{nameof(MediaFileRepository)}|{nameof(GetByName)}|{mediaLibraryName}");
        }


        /// <summary>
        /// Returns all media files in the media library.
        /// </summary>
        /// <param name="mediaLibraryName">Name of the media library.</param>
        public IEnumerable<MediaFileInfo> GetMediaFiles(string mediaLibraryName)
        {
            return repositoryCacheHelper.CacheObjects(() =>
            {
                var mediaLibrary = GetByName(mediaLibraryName);

                if (mediaLibrary == null)
                {
                    throw new InvalidOperationException("Media library not found.");
                }

                return mediaFileInfoProvider.Get()
                    .WhereEquals("FileLibraryID", mediaLibrary.LibraryID);
            }, $"{nameof(MediaFileRepository)}|{nameof(GetMediaFiles)}|{mediaLibraryName}");
        }


        /// <summary>
        /// Returns media file with given identifier and site name.
        /// </summary>
        /// <param name="fileIdentifier">Identifier of the media file.</param>
        /// <param name="siteName">Site ID.</param>
        public MediaFileInfo GetMediaFile(Guid fileIdentifier, int siteId)
        {
            return repositoryCacheHelper.CacheObject(() =>
            {
                return mediaFileInfoProvider.Get(fileIdentifier, siteId);
            }, $"{nameof(MediaFileRepository)}|{nameof(GetMediaFile)}|{fileIdentifier}|{siteId}");
        }
    }
}