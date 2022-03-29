
using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine.Types.DancingGoatCore;

using DancingGoat.Controllers;
using DancingGoat.Models;

using Kentico.Content.Web.Mvc.Routing;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

[assembly: RegisterPageRoute(Contacts.CLASS_NAME, typeof(ContactsController))]

namespace DancingGoat.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ContactRepository contactRepository;
        private readonly CountryRepository countryRepository;
        private readonly CafeRepository cafeRepository;
        private readonly IStringLocalizer<SharedResources> localizer;


        public ContactsController(ContactRepository contactRepository, 
            CountryRepository countryRepository,
            CafeRepository cafeRepository,
            IStringLocalizer<SharedResources> localizer)
        {
            this.countryRepository = countryRepository;
            this.contactRepository = contactRepository;
            this.cafeRepository = cafeRepository;
            this.localizer = localizer;
        }


        public ActionResult Index()
        {
            var model = GetIndexViewModel();

            return View(model);
        }


        private ContactsIndexViewModel GetIndexViewModel()
        {
            var cafes = cafeRepository.GetCompanyCafes(ContentItemIdentifiers.CAFES, 4);

            return new ContactsIndexViewModel
            {
                CompanyContact = GetCompanyContactModel(),
                CompanyCafes = GetCompanyCafesModel(cafes)
            };
        }


        private ContactViewModel GetCompanyContactModel()
        {
            return ContactViewModel.GetViewModel(contactRepository.GetCompanyContact(), countryRepository, localizer);
        }


        private List<ContactViewModel> GetCompanyCafesModel(IEnumerable<Cafe> cafes)
        {
            return cafes.Select(cafe => ContactViewModel.GetViewModel(cafe, countryRepository, localizer)).ToList();
        }
    }
}