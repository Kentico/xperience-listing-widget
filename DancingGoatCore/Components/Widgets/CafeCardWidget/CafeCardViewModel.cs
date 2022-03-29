using CMS.DocumentEngine.Types.DancingGoatCore;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// View model for Cafe card widget.
    /// </summary>
    public class CafeCardViewModel
    {
        /// <summary>
        /// Cafe name.
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Cafe background image path.
        /// </summary>
        public string PhotoPath { get; set; }


        /// <summary>
        /// Gets ViewModel for <paramref name="cafe"/>.
        /// </summary>
        /// <param name="cafe">Cafe.</param>
        /// <param name="attachmentUrlRetriever">Attachment URL retriever.</param>
        /// <returns>Hydrated view model.</returns>
        public static CafeCardViewModel GetViewModel(Cafe cafe, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            return cafe == null
                ? new CafeCardViewModel()
                : new CafeCardViewModel
                {
                    Name = cafe.Fields.Name,
                    PhotoPath = cafe.Fields.Photo == null ? null : attachmentUrlRetriever.Retrieve(cafe.Fields.Photo).RelativePath
                };
        }
    }
}