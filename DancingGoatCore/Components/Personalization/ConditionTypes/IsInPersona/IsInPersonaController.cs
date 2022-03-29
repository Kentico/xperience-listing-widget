using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using CMS.Personas;

using Kentico.PageBuilder.Web.Mvc.Personalization;

namespace DancingGoat.Personalization
{
    public class IsInPersonaController : ConditionTypeController<IsInPersonaConditionType>
    {
        private readonly IPersonaInfoProvider mPersonaInfoProvider;
        private readonly IPersonaPictureUrlCreator mPictureCreator;


        public IsInPersonaController(IPersonaInfoProvider personaInfoProvider, IPersonaPictureUrlCreator pictureCreator)
        {
            mPersonaInfoProvider = personaInfoProvider;
            mPictureCreator = pictureCreator;
        }


        [HttpPost]
        public ActionResult Index()
        {
            var conditionTypeParameters = GetParameters();
   
            var viewModel = new IsInPersonaViewModel
            {
                PersonaCodeName = conditionTypeParameters.PersonaName,
                AllPersonas = GetAllPersonasViewModel(conditionTypeParameters.PersonaName)
            };
            return PartialView("~/Components/Personalization/ConditionTypes/IsInPersona/_IsInPersonaConfiguration.cshtml", viewModel);
        }


        [HttpPost]
        public ActionResult Validate(IsInPersonaViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.AllPersonas = GetAllPersonasViewModel();
                return PartialView("~/Components/Personalization/ConditionTypes/IsInPersona/_IsInPersonaConfiguration.cshtml", viewModel);
            }

            var parameters = new IsInPersonaConditionType
            {
                PersonaName = viewModel.PersonaCodeName
            };

            return new ConditionTypeValidationResult(parameters);
        }


        private List<IsInPersonaListItemViewModel> GetAllPersonasViewModel(string selectedPersonaName = "")
        {
            var allPersonas = GetAllPersonas().Select(persona => new IsInPersonaListItemViewModel
            {
                CodeName = persona.PersonaName,
                DisplayName = persona.PersonaDisplayName,
                ImagePath = mPictureCreator.CreatePersonaPictureUrl(persona, 50),
                Selected = persona.PersonaName == selectedPersonaName
            }).ToList();

            if (allPersonas.Count > 0 && !allPersonas.Exists(x => x.Selected))
            {
                allPersonas.First().Selected = true;
            }

            return allPersonas;
        }


        private IEnumerable<PersonaInfo> GetAllPersonas()
        {
            return mPersonaInfoProvider.Get();
        }
    }
}