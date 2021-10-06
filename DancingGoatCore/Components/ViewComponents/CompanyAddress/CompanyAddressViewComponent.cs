using DancingGoat.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace DancingGoat.ViewComponents
{
    public class CompanyAddressViewComponent : ViewComponent
    {
        private readonly ContactRepository contactRepository;
        private readonly CountryRepository countryRepository;
        private readonly IStringLocalizer<SharedResources> localizer;


        public CompanyAddressViewComponent(ContactRepository contactRepository,
            CountryRepository countryRepository,
            IStringLocalizer<SharedResources> localizer)
        {
            this.contactRepository = contactRepository;
            this.countryRepository = countryRepository;
            this.localizer = localizer;
        }


        public IViewComponentResult Invoke()
        {
            var contact = contactRepository.GetCompanyContact();
            var model = ContactViewModel.GetViewModel(contact, countryRepository, localizer);
            
            return View("~/Components/ViewComponents/CompanyAddress/Default.cshtml", model);
        }
    }
}
