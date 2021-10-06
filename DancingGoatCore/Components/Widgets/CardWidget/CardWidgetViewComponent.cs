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

[assembly: RegisterWidget(CardWidgetViewComponent.IDENTIFIER, typeof(CardWidgetViewComponent), "Card", typeof(CardWidgetProperties), Description = "Displays an image with a centered text.", IconClass = "icon-rectangle-paragraph")]

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Controller for card widget.
    /// </summary>
    public class CardWidgetViewComponent : ViewComponent
    {
        /// <summary>
        /// Widget identifier.
        /// </summary>
        public const string IDENTIFIER = "DancingGoat.LandingPage.CardWidget";


        private readonly MediaFileRepository mediaFileRepository;
        private readonly IMediaFileUrlRetriever mediaFileUrlRetriever;


        /// <summary>
        /// Creates an instance of <see cref="CardWidgetViewComponent"/> class.
        /// </summary>
        /// <param name="mediaFileRepository">Repository for media files.</param>
        /// <param name="mediaFileUrlRetriever">The media file URL retriever.</param>
        public CardWidgetViewComponent(MediaFileRepository mediaFileRepository, IMediaFileUrlRetriever mediaFileUrlRetriever)
        {
            this.mediaFileRepository = mediaFileRepository;
            this.mediaFileUrlRetriever = mediaFileUrlRetriever;
        }


        public ViewViewComponentResult Invoke(CardWidgetProperties properties)
        {
            var image = GetImage(properties);

            return View("~/Components/Widgets/CardWidget/_CardWidget.cshtml", new CardWidgetViewModel
            {
                ImagePath = image == null ? null : mediaFileUrlRetriever.Retrieve(image).RelativePath,
                Text = properties.Text
            });
        }


        private MediaFileInfo GetImage(CardWidgetProperties properties)
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