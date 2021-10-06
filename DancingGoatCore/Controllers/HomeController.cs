using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CMS.DocumentEngine.Types.DancingGoatCore;

using DancingGoat.Controllers;
using DancingGoat.Models;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using Microsoft.AspNetCore.Mvc;

[assembly: RegisterPageRoute(Home.CLASS_NAME, typeof(HomeController))]

namespace DancingGoat.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPageDataContextRetriever pageDataContextRetriever;
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly IPageAttachmentUrlRetriever attachmentUrlRetriever;
        private readonly ReferenceRepository referenceRepository;
        private readonly HomeRepository homeSectionRepository;

        public HomeController(IPageDataContextRetriever pageDataContextRetriever,
            IPageUrlRetriever pageUrlRetriever,
            IPageAttachmentUrlRetriever attachmentUrlRetriever,
            HomeRepository homeSectionRepository,
            ReferenceRepository referenceRepository)
        {
            this.pageDataContextRetriever = pageDataContextRetriever;
            this.homeSectionRepository = homeSectionRepository;
            this.referenceRepository = referenceRepository;
            this.pageUrlRetriever = pageUrlRetriever;
            this.attachmentUrlRetriever = attachmentUrlRetriever;
        }


        public async Task<ActionResult> Index(CancellationToken cancellationToken)
        {
            var home = pageDataContextRetriever.Retrieve<Home>().Page;
            var homeSections = await homeSectionRepository.GetHomeSectionsAsync(home.NodeAliasPath, cancellationToken);
            var reference = (await referenceRepository.GetReferencesAsync(home.NodeAliasPath, cancellationToken, 1)).FirstOrDefault();

            var viewModel = new HomeIndexViewModel
            {
                HomeSections = homeSections.Select(section => HomeSectionViewModel.GetViewModel(section, pageUrlRetriever, attachmentUrlRetriever)),
                Reference = ReferenceViewModel.GetViewModel(reference, attachmentUrlRetriever)
            };

            return View(viewModel);
        }
    }
}
