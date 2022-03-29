using CMS.DocumentEngine.Types.DancingGoatCore;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    public class ReferenceViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Text { get; set; }

        public string ImagePath { get; set; }


        public static ReferenceViewModel GetViewModel(Reference reference, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            if (reference == null)
            {
                return null;
            }

            return new ReferenceViewModel
            {
                Name = reference.ReferenceName,
                Description = reference.ReferenceDescription,
                Text = reference.ReferenceText,
                ImagePath = attachmentUrlRetriever.Retrieve(reference.Fields.Image).RelativePath
            };
        }
    }
}