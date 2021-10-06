using System;
using System.Linq;

using CMS.Base;
using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;
using CMS.Helpers;
using CMS.Membership;
using CMS.OnlineMarketing;
using CMS.Scheduler;
using CMS.WebAnalytics;
using CMS.WebAnalytics.Internal;

using DancingGoat.Controllers;
using DancingGoat.Helpers.Generator;
using DancingGoat.Models;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.Forms.Web.Mvc.Internal;
using Kentico.Web.Mvc;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[assembly: RegisterPageRoute(Generator.CLASS_NAME, typeof(GeneratorController))]

namespace DancingGoat.Controllers
{
    [Authorize]
    public class GeneratorController : Controller
    {
        private const string FORM_NAME = "DancingGoatCoreCoffeeSampleList";
        private const string FORM_FIELD_NAME = "Consent";


        /// <summary>
        /// First name prefix of contacts generated for sample campaigns.
        /// </summary>
        private const string CONTACT_FIRST_NAME_PREFIX = "GeneratedMvcCampaignContact";


        /// <summary>
        /// Last name prefix of contacts generated for sample campaigns.
        /// </summary>
        private const string CONTACT_LAST_NAME_PREFIX = "GeneratedMvcCampaignContactLastName";


        private const string DATA_PROTECTION_SETTINGS_KEY = "DataProtectionSamplesEnabled";

        
        private readonly ISiteService siteService;
        private readonly IWebFarmService webFarmService;


        public GeneratorController(ISiteService siteService, IWebFarmService webFarmService)
        {
            this.siteService = siteService;
            this.webFarmService = webFarmService;
        }


        public ActionResult Index()
        {
            if (!IsAdmin())
            {
                return UnauthorizedView();
            }

            var model = new GeneratorIndexViewModel();
            return View(model);
        }


        [HttpGet, ActionName("Generate")]
        public ActionResult GenerateGet()
        {
            return Redirect(Url.Kentico().PageUrl(ContentItemIdentifiers.GENERATOR));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Generator/Generate")]
        public ActionResult Generate([FromServices] IActivityUrlHashService hashService)
        {
            if (!IsAdmin())
            {
                return UnauthorizedView();
            }

            new PersonaGenerator().Generate();

            new ContactGroupGenerator().Generate();

            new CampaignContactsDataGenerator(CONTACT_FIRST_NAME_PREFIX, CONTACT_LAST_NAME_PREFIX).Generate();

            var site = siteService.CurrentSite;
            new CampaignDataGenerator(site, CONTACT_FIRST_NAME_PREFIX, hashService).Generate();

            new OnlineMarketingDataGenerator(site, hashService).Generate();
            new NewslettersDataGenerator(site).Generate();

            var model = new GeneratorIndexViewModel
            {
                DisplaySuccessMessage = true
            };

            return View("Index", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Generator/GenerateABTestData")]
        public ActionResult GenerateABTestData([FromServices] IABTestManager abTestManager, [FromServices] IAnalyticsLogger analyticsLogger)
        {
            if (!IsAdmin())
            {
                return UnauthorizedView();
            }

            var model = new GeneratorIndexViewModel();

            try
            {
                var testGenerator = new ABTestConversionGenerator(abTestManager, analyticsLogger);
                testGenerator.StartTestAndGenerateData();

                model.DisplaySuccessMessage = true;
            }
            catch (ABTestConversionGeneratorException ex)
            {
                model.ABTestErrorMessage = ex.DisplayMessage;
            }

            return View("Index", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Generator/GenerateDataProtectionDemo")]
        public ActionResult GenerateDataProtectionDemo([FromServices] IFormBuilderConfigurationSerializer formBuilderConfigurationSerializer)
        {
            if (!IsAdmin())
            {
                return UnauthorizedView();
            }

            var site = siteService.CurrentSite;

            new TrackingConsentGenerator(site).Generate();
            new FormConsentGenerator(site, formBuilderConfigurationSerializer).Generate(FORM_NAME, FORM_FIELD_NAME);
            new FormContactGroupGenerator().Generate();

            EnableDataProtectionSamples(site);

            var model = new GeneratorIndexViewModel
            {
                DisplaySuccessMessage = true
            };

            return View("Index", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Generator/GenerateSentimentAnalysisDemo")]
        public ActionResult GenerateSentimentAnalysisDemo()
        {
            if (!IsAdmin())
            {
                return UnauthorizedView();
            }

            var site = siteService.CurrentSite;

            new SentimentAnalysisDataGenerator(site).Generate();

            var model = new GeneratorIndexViewModel
            {
                DisplaySuccessMessage = true
            };

            return View("Index", model);
        }


        private void EnableDataProtectionSamples(ISiteInfo site)
        {
            var dataProtectionSamplesEnabledSettingsKey = SettingsKeyInfoProvider.GetSettingsKeyInfo(DATA_PROTECTION_SETTINGS_KEY);
            if (dataProtectionSamplesEnabledSettingsKey?.KeyValue.ToBoolean(false) ?? false)
            {
                return;
            }

            var keyInfo = new SettingsKeyInfo
            {
                KeyName = DATA_PROTECTION_SETTINGS_KEY,
                KeyDisplayName = DATA_PROTECTION_SETTINGS_KEY,
                KeyType = "boolean",
                KeyValue = "True",
                KeyDefaultValue = "False",
                KeyIsGlobal = true,
                KeyIsHidden = true
            };

            SettingsKeyInfoProvider.SetSettingsKeyInfo(keyInfo);
            EnsureTask(site);
        }


        private void EnsureTask(ISiteInfo site)
        {
            var currentServerName = WebFarmHelper.ServerName;

            var taskServerName = webFarmService.GetEnabledServerNames().First(serverName => !currentServerName.Equals(serverName, StringComparison.Ordinal));

            TaskInterval interval = new TaskInterval
            {
                StartTime = DateTime.Now,
                Period = SchedulingHelper.PERIOD_ONCE
            };

            var task = new TaskInfo
            {
                TaskAssemblyName = "CMS.UIControls", 
                TaskClass = "Samples.DancingGoat.EnableDataProtectionSampleTask",
                TaskEnabled = true,
                TaskLastResult = string.Empty,
                TaskData = string.Empty,
                TaskDisplayName = "Data protection sample",
                TaskName = "EnableDataProtectionSampleTask",
                TaskType = ScheduledTaskTypeEnum.System,
                TaskInterval = SchedulingHelper.EncodeInterval(interval),
                TaskNextRunTime = SchedulingHelper.GetFirstRunTime(interval),
                TaskDeleteAfterLastRun = true,
                TaskRunInSeparateThread = true,
                TaskSiteID = site.SiteID,
                TaskServerName = taskServerName,
                TaskAvailability = TaskAvailabilityEnum.Administration
            };

            TaskInfo.Provider.Set(task);
        }

        private bool IsAdmin()
        {
            return MembershipContext.AuthenticatedUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Admin);
        }


        private ActionResult UnauthorizedView()
        {
            var model = new GeneratorIndexViewModel
            {
                IsAuthorized = false
            };

            return View("Index", model);
        }
    }
}
