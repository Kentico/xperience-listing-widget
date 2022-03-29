using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;

using DancingGoat.Controllers;
using DancingGoat.Models;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

[assembly: RegisterPageRoute(CafeSection.CLASS_NAME, typeof(CafesController))]

namespace DancingGoat.Controllers
{
    public class CafesController : Controller
    {
        private readonly IPageDataContextRetriever dataContextRetriever;
        private readonly CafeRepository cafeRepository;
        private readonly CountryRepository countryRepository;
        private readonly IStringLocalizer<SharedResources> localizer;
        private readonly IPageAttachmentUrlRetriever attachmentUrlRetriever;


        public CafesController(IPageDataContextRetriever dataContextRetriever, 
            CafeRepository cafeRepository, 
            CountryRepository countryRepository,
            IStringLocalizer<SharedResources> localizer,
            IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            this.dataContextRetriever = dataContextRetriever;
            this.cafeRepository = cafeRepository;
            this.countryRepository = countryRepository;
            this.localizer = localizer;
            this.attachmentUrlRetriever = attachmentUrlRetriever;
        }


        public ActionResult Index()
        {
            var section = dataContextRetriever.Retrieve<TreeNode>().Page;
            var companyCafes = cafeRepository.GetCompanyCafes(section.NodeAliasPath, 4);
            var partnerCafes = cafeRepository.GetPartnerCafes(section.NodeAliasPath);

            var model = new CafesIndexViewModel
            {
                CompanyCafes = GetCompanyCafesModel(companyCafes),
                PartnerCafes = GetPartnerCafesModel(partnerCafes)
            };

            return View(model);
        }


        private Dictionary<string, List<ContactViewModel>> GetPartnerCafesModel(IEnumerable<Cafe> cafes)
        {
            var cityCafes = new Dictionary<string, List<ContactViewModel>>();

            // Group partner cafes by their location
            foreach (var cafe in cafes)
            {
                var city = cafe.City.ToLowerInvariant();
                var contact = ContactViewModel.GetViewModel(cafe, countryRepository, localizer);

                if (cityCafes.ContainsKey(city))
                {
                    cityCafes[city].Add(contact);
                }
                else
                {
                    cityCafes.Add(city, new List<ContactViewModel> {contact});
                }
            }

            return cityCafes;
        }


        private IEnumerable<CafeViewModel> GetCompanyCafesModel(IEnumerable<Cafe> cafes)
        {
            return cafes.Select(cafe => CafeViewModel.GetViewModel(cafe, countryRepository, localizer, attachmentUrlRetriever));
        }
    }
}