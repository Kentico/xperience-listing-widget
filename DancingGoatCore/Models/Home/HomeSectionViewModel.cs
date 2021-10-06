using System.Linq;

using CMS.DocumentEngine.Types.DancingGoatCore;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    public class HomeSectionViewModel
    {
        public string Heading { get; set; }
        public string Text { get; set; }
        public string MoreButtonText { get; set; }
        public string MoreButtonUrl { get; set; }

        public static HomeSectionViewModel GetViewModel(HomeSection homeSection, IPageUrlRetriever pageUrlRetriever, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            var link = homeSection?.Fields.Link.FirstOrDefault();
            return homeSection == null ? null : new HomeSectionViewModel
            {
                Heading = homeSection.Fields.Heading,
                Text = homeSection.Fields.Text,
                MoreButtonText = homeSection.Fields.LinkText,
                MoreButtonUrl = link != null ? pageUrlRetriever.Retrieve(link).RelativePath : string.Empty
            };
        }
    }
}