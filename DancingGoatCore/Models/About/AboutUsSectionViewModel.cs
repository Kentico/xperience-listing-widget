using CMS.DocumentEngine.Types.DancingGoatCore;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    public class AboutUsSectionViewModel
    {
        public string Heading { get; set; }
        
        public string Text { get; set; }

        public string ImagePath { get; set; }

        public static AboutUsSectionViewModel GetViewModel(AboutUsSection aboutUsSection, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            return new AboutUsSectionViewModel
            {
                Heading = aboutUsSection.Fields.Heading,
                Text = aboutUsSection.Fields.Text,
                ImagePath = aboutUsSection.Fields.Image == null ? null : attachmentUrlRetriever.Retrieve(aboutUsSection.Fields.Image).RelativePath
            };
        }
    }
}