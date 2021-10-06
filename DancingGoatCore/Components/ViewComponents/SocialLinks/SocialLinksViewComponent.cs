using System.Linq;

using DancingGoat.Models;

using Kentico.Content.Web.Mvc;

using Microsoft.AspNetCore.Mvc;

namespace DancingGoat.ViewComponents
{
    public class SocialLinksViewComponent : ViewComponent
    {
        private readonly SocialLinkRepository socialLinkRepository;
        private readonly IPageAttachmentUrlRetriever attachmentUrlRetriever;


        public SocialLinksViewComponent(SocialLinkRepository socialLinkRepository, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            this.socialLinkRepository = socialLinkRepository;
            this.attachmentUrlRetriever = attachmentUrlRetriever;
        }


        public IViewComponentResult Invoke()
        {
            var socialLinks = socialLinkRepository.GetSocialLinks().Select(link => SocialLinkViewModel.GetViewModel(link, attachmentUrlRetriever));

            return View("~/Components/ViewComponents/SocialLinks/Default.cshtml", socialLinks);
        }
    }
}
