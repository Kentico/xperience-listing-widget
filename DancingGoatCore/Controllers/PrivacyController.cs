using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using CMS.Base;
using CMS.ContactManagement;
using CMS.Core;
using CMS.DataProtection;
using CMS.DocumentEngine.Types.DancingGoatCore;
using CMS.Helpers;
using CMS.Membership;
using CMS.Scheduler;

using DancingGoat.Controllers;
using DancingGoat.Helpers.Generator;
using DancingGoat.Models;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.Web.Mvc;

using Microsoft.AspNetCore.Mvc;

[assembly: RegisterPageRoute(Privacy.CLASS_NAME, typeof(PrivacyController))]

namespace DancingGoat.Controllers
{
    public class PrivacyController : Controller
    {
        private readonly IConsentAgreementService consentAgreementService;
        private readonly ICurrentCookieLevelProvider cookieLevelProvider;
        private readonly ICurrentUserContactProvider currentContactProvider;
        private readonly IWebFarmService webFarmService;
        private readonly ISiteService siteService;
        private readonly IConsentInfoProvider consentInfoProvider;
        private readonly ITaskInfoProvider taskInfoProvider;
        private ContactInfo currentContact;

        private const string SUCCESS_RESULT = "success";
        private const string ERROR_RESULT = "error";


        private ContactInfo CurrentContact
        {
            get
            {
                if (currentContact == null)
                {
                    // Try to get contact from cookie
                    currentContact = ContactManagementContext.CurrentContact;

                    // If contact is not found, get the contact for current user regardless of the cookie level set
                    if (currentContact == null)
                    {
                        currentContact = currentContactProvider.GetContactForCurrentUser(MembershipContext.AuthenticatedUser);
                    }
                }

                return currentContact;
            }
        }


        public PrivacyController(IConsentAgreementService consentAgreementService, ICurrentCookieLevelProvider cookieLevelProvider,
            ICurrentUserContactProvider currentContactProvider, IWebFarmService webFarmService, ISiteService siteService, IConsentInfoProvider consentInfoProvider,
            ITaskInfoProvider taskInfoProvider)
        {
            this.consentAgreementService = consentAgreementService;
            this.cookieLevelProvider = cookieLevelProvider;
            this.currentContactProvider = currentContactProvider;
            this.webFarmService = webFarmService;
            this.siteService = siteService;
            this.consentInfoProvider = consentInfoProvider;
            this.taskInfoProvider = taskInfoProvider;
        }


        public ActionResult Index()
        {
            var model = new PrivacyViewModel();

            if (!IsDemoEnabled())
            {
                model.DemoDisabled = true;
            }
            else if (CurrentContact != null)
            {
                model.Constents = GetAgreedConsentsForCurrentContact();
            }

            model.ShowSavedMessage = TempData[SUCCESS_RESULT] != null;
            model.ShowErrorMessage = TempData[ERROR_RESULT] != null;

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Privacy/Revoke")]
        public ActionResult Revoke(string consentName)
        {
            var consentToRevoke = consentInfoProvider.Get(consentName);

            if (consentToRevoke != null && CurrentContact != null)
            {
                consentAgreementService.Revoke(CurrentContact, consentToRevoke);

                if (consentName == TrackingConsentGenerator.CONSENT_NAME)
                {
                    cookieLevelProvider.SetCurrentCookieLevel(cookieLevelProvider.GetDefaultCookieLevel());
                    ExecuteRevokeTrackingConsentTask(siteService.CurrentSite, CurrentContact);
                }

                TempData[SUCCESS_RESULT] = true;
            }
            else
            {
                TempData[ERROR_RESULT] = true;
            }

            return Redirect(Url.Kentico().PageUrl(ContentItemIdentifiers.PRIVACY));
        }


        private IEnumerable<PrivacyConsentViewModel> GetAgreedConsentsForCurrentContact()
        {
            return consentAgreementService.GetAgreedConsents(CurrentContact)
                .Select(consent => new PrivacyConsentViewModel
                {
                    Name = consent.Name,
                    Title = consent.DisplayName,
                    Text = consent.GetConsentText(Thread.CurrentThread.CurrentCulture.Name).ShortText
                });
        }


        private bool IsDemoEnabled()
        {
            return consentInfoProvider.Get(TrackingConsentGenerator.CONSENT_NAME) != null;
        }


        private void ExecuteRevokeTrackingConsentTask(ISiteInfo site, ContactInfo contact)
        {
            const string TASK_NAME_PREFIX = "DataProtectionSampleRevokeTrackingConsentTask";
            string taskName = $"{TASK_NAME_PREFIX}_{contact.ContactID}";

            // Do not create same task if already scheduled
            var scheduledTask = taskInfoProvider.Get(taskName, site.SiteID);
            if (scheduledTask != null)
            {
                return;
            }

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
                TaskClass = "Samples.DancingGoat.RevokeTrackingConsentTask",
                TaskEnabled = true,
                TaskLastResult = string.Empty,
                TaskData = contact.ContactID.ToString(),
                TaskDisplayName = "Data protection sample - Revoke tracking consent",
                TaskName = taskName,
                TaskType = ScheduledTaskTypeEnum.System,
                TaskInterval = SchedulingHelper.EncodeInterval(interval),
                TaskNextRunTime = SchedulingHelper.GetFirstRunTime(interval),
                TaskDeleteAfterLastRun = true,
                TaskRunInSeparateThread = true,
                TaskSiteID = site.SiteID,
                TaskServerName = taskServerName,
                TaskAvailability = TaskAvailabilityEnum.Administration
            };

            taskInfoProvider.Set(task);
        }
    }
}
