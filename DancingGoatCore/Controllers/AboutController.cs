using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CMS.DocumentEngine.Types.DancingGoatCore;

using DancingGoat.Controllers;
using DancingGoat.Models;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using Microsoft.AspNetCore.Mvc;

[assembly: RegisterPageRoute(AboutUs.CLASS_NAME, typeof(AboutController))]

namespace DancingGoat.Controllers
{
    public class AboutController : Controller
    {
        private readonly IPageDataContextRetriever dataRetriever;
        private readonly IPageAttachmentUrlRetriever pageAttachmentUrlRetriever;
        private readonly AboutUsRepository aboutUsRepository;
        private readonly ReferenceRepository referenceRepository;


        public AboutController(IPageDataContextRetriever dataRetriever, 
            IPageAttachmentUrlRetriever pageAttachmentUrlRetriever,
            AboutUsRepository aboutUsRepository, 
            ReferenceRepository referenceRepository)
        {
            this.dataRetriever = dataRetriever;
            this.pageAttachmentUrlRetriever = pageAttachmentUrlRetriever;
            this.aboutUsRepository = aboutUsRepository;
            this.referenceRepository = referenceRepository;
        }


        public async Task<ActionResult> Index(CancellationToken cancellationToken)
        {
            var aboutUs = dataRetriever.Retrieve<AboutUs>().Page;

            var sideStories = await aboutUsRepository.GetSideStoriesAsync(aboutUs.NodeAliasPath, cancellationToken);

            var reference = (await referenceRepository.GetReferencesAsync($"{aboutUs.NodeAliasPath}/References", cancellationToken, 1)).FirstOrDefault();

            AboutUsViewModel mode = new AboutUsViewModel()
            {
                Sections = sideStories.Select(s => AboutUsSectionViewModel.GetViewModel(s, pageAttachmentUrlRetriever)).ToList(),
                Reference = ReferenceViewModel.GetViewModel(reference, pageAttachmentUrlRetriever)
            };

            return View(mode);
        }
    }
}