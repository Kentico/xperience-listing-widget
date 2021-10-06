using System;
using System.Linq;

using CMS.MediaLibrary;
using CMS.SiteProvider;

using DancingGoat.Widgets;

using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

[assembly: RegisterWidget(BannerWidgetViewComponent.IDENTIFIER, typeof(BannerWidgetViewComponent), "Banner", typeof(BannerWidgetProperties), Description = "Displays the text and image.", IconClass = "icon-ribbon")]

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Banner widget service.
    /// </summary>
    public class BannerWidgetViewComponent : ViewComponent
    {
        /// <summary>
        /// Widget identifier.
        /// </summary>
        public const string IDENTIFIER = "DancingGoat.HomePage.BannerWidget";


        private readonly IMediaFileInfoProvider mediaFileProvider;
        private readonly IMediaFileUrlRetriever fileUrlRetriever;


        /// <summary>
        /// Initializes a new instance of the <see cref="BannerWidgetViewComponent"/> class.
        /// </summary>
        /// <param name="mediaFileProvider">The media file provider.</param>
        /// <param name="fileUrlRetriever">The media file URL retriever.</param>
        public BannerWidgetViewComponent(IMediaFileInfoProvider mediaFileProvider, IMediaFileUrlRetriever fileUrlRetriever)
        {
            this.mediaFileProvider = mediaFileProvider;
            this.fileUrlRetriever = fileUrlRetriever;
        }


        public ViewViewComponentResult Invoke(BannerWidgetProperties properties)
        {
            var imagePath = GetImagePath(properties);

            return View("~/Components/Widgets/BannerWidget/_BannerWidget.cshtml", new BannerWidgetModel
            {
                ImagePath = imagePath,
                Text = properties.Text,
                LinkUrl = properties.LinkUrl,
                LinkTitle = properties.LinkTitle
            });
        }


        private string GetImagePath(BannerWidgetProperties properties)
        {
            var imageGuid = properties.Image.FirstOrDefault()?.FileGuid ?? Guid.Empty;
            if (imageGuid == Guid.Empty)
            {
                return null;
            }

            var image = mediaFileProvider.Get(imageGuid, SiteContext.CurrentSiteID);
            if (image == null)
            {
                return string.Empty;
            }

            return fileUrlRetriever.Retrieve(image).RelativePath;
        }
    }
}