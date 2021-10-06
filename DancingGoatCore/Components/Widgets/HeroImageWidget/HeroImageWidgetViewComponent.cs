using System;
using System.Linq;

using CMS.MediaLibrary;
using CMS.SiteProvider;

using DancingGoat.Models;
using DancingGoat.Widgets;

using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

[assembly: RegisterWidget(HeroImageWidgetViewComponent.IDENTIFIER, typeof(HeroImageWidgetViewComponent), "Hero image", typeof(HeroImageWidgetProperties), Description = "Displays an image, text, and a CTA button.", IconClass = "icon-badge")]

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Controller for hero image widget.
    /// </summary>
    public class HeroImageWidgetViewComponent : ViewComponent
    {
        /// <summary>
        /// Widget identifier.
        /// </summary>
        public const string IDENTIFIER = "DancingGoat.LandingPage.HeroImage";


        private readonly MediaFileRepository mediaFileRepository;
        private readonly IMediaFileUrlRetriever mediaFileUrlRetriever;


        /// <summary>
        /// Creates an instance of <see cref="BannerWidgetController"/> class.
        /// </summary>
        /// <param name="mediaFileRepository">Repository for media files.</param>
        /// <param name="mediaFileUrlRetriever">The media file URL retriever.</param>
        public HeroImageWidgetViewComponent(MediaFileRepository mediaFileRepository, IMediaFileUrlRetriever mediaFileUrlRetriever)
        {
            this.mediaFileRepository = mediaFileRepository;
            this.mediaFileUrlRetriever = mediaFileUrlRetriever;
        }


        public ViewViewComponentResult Invoke(HeroImageWidgetProperties properties)
        {
            var image = GetImage(properties);

            return View("~/Components/Widgets/HeroImageWidget/_HeroImageWidget.cshtml", new HeroImageWidgetViewModel
            {
                ImagePath = image == null ? null : mediaFileUrlRetriever.Retrieve(image).RelativePath,
                Text = properties.Text,
                ButtonText = properties.ButtonText,
                ButtonTarget = properties.ButtonTarget,
                Theme = properties.Theme
            });
        }


        private MediaFileInfo GetImage(HeroImageWidgetProperties properties)
        {
            var imageGuid = properties.Image.FirstOrDefault()?.FileGuid ?? Guid.Empty;
            if (imageGuid == Guid.Empty)
            {
                return null;
            }

            return mediaFileRepository.GetMediaFile(imageGuid, SiteContext.CurrentSiteID);
        }
    }
}