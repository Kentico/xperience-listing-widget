using System;

using CMS.Helpers;
using CMS.Newsletters;
using CMS.SiteProvider;

using DancingGoat.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace DancingGoat.Web.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionService subscriptionService;
        private readonly ISubscriptionApprovalService subscriptionApprovalService;
        private readonly IUnsubscriptionProvider unsubscriptionProvider;
        private readonly IEmailHashValidator emailHashValidator;
        private readonly IIssueInfoProvider issueInfoProvider;
        private readonly INewsletterInfoProvider newsletterInfoProvider;
        private readonly IStringLocalizer<SharedResources> localizer;

        public SubscriptionController(ISubscriptionService subscriptionService, ISubscriptionApprovalService subscriptionApprovalService, IUnsubscriptionProvider unsubscriptionProvider,
            IEmailHashValidator emailHashValidator, IIssueInfoProvider issueInfoProvider, INewsletterInfoProvider newsletterInfoProvider, IStringLocalizer<SharedResources> localizer)
        {
            this.subscriptionService = subscriptionService;
            this.subscriptionApprovalService = subscriptionApprovalService;
            this.unsubscriptionProvider = unsubscriptionProvider;
            this.emailHashValidator = emailHashValidator;
            this.issueInfoProvider = issueInfoProvider;
            this.newsletterInfoProvider = newsletterInfoProvider;
            this.localizer = localizer;
        }


        // GET: Subscription/ConfirmSubscription
        public ActionResult ConfirmSubscription(ConfirmSubscriptionModel model)
        {
            if (!ModelState.IsValid)
            {
                AddError("The confirmation link is not valid.");
                
                return View(model);
            }

            DateTime parsedDateTime = DateTimeHelper.ZERO_TIME;

            // Parse date and time from query string, if present
            if (!string.IsNullOrEmpty(model.DateTime) && !DateTimeUrlFormatter.TryParse(model.DateTime, out parsedDateTime))
            {
                AddError("The confirmation link is not valid.");

                return View(model);
            }

            var result = subscriptionApprovalService.ApproveSubscription(model.SubscriptionHash, false, SiteContext.CurrentSiteName, parsedDateTime);

            switch (result)
            {
                case ApprovalResult.Success:
                    model.ConfirmationResult = localizer["Your subscription has been confirmed."].Value;
                    break;

                case ApprovalResult.AlreadyApproved:
                    model.ConfirmationResult = localizer["You are already subscribed."].Value;
                    break;

                case ApprovalResult.TimeExceeded:
                    AddError("Your subscription confirmation link has expired. Please subscribe again.");
                    break;

                case ApprovalResult.NotFound:
                    AddError("The confirmation link is not valid.");
                    break;

                default:
                    AddError("The subscription confirmation has failed. Please try again later.");

                    break;
            }

            return View(model);
        }


        // GET: Subscription/Unsubscribe
        public ActionResult Unsubscribe(UnsubscriptionModel model)
        {
            var invalidUrlMessage = "You weren't unsubscribed because your unsubscription link is not valid.";

            if (ModelState.IsValid)
            {
                bool emailIsValid = emailHashValidator.ValidateEmailHash(model.Hash, model.Email);
                
                if (emailIsValid)
                {
                    try
                    {
                        var issue = issueInfoProvider.Get(model.IssueGuid, SiteContext.CurrentSiteID);

                        if (model.UnsubscribeFromAll)
                        {
                            // Unsubscribes if not already unsubscribed
                            if (!unsubscriptionProvider.IsUnsubscribedFromAllNewsletters(model.Email))
                            {
                                subscriptionService.UnsubscribeFromAllNewsletters(model.Email, issue?.IssueID);
                            }

                            model.UnsubscriptionResult = localizer["You've been unsubscribed from all email marketing campaigns."].Value;
                        }
                        else
                        {
                            var newsletter = newsletterInfoProvider.Get(model.NewsletterGuid, SiteContext.CurrentSiteID);
                            if (newsletter != null)
                            {
                                // Unsubscribes if not already unsubscribed
                                if (!unsubscriptionProvider.IsUnsubscribedFromSingleNewsletter(model.Email, newsletter.NewsletterID))
                                {
                                    subscriptionService.UnsubscribeFromSingleNewsletter(model.Email, newsletter.NewsletterID, issue?.IssueID);
                                }

                                model.UnsubscriptionResult = localizer["We would like to offer you a coffee to go, but we can't, because we have just removed your email from the list."].Value;
                            }
                            else
                            {
                                AddError(invalidUrlMessage);
                            }
                        }
                    }
                    catch (ArgumentException)
                    {
                        AddError(invalidUrlMessage);
                    }
                }
                else
                {
                    AddError(invalidUrlMessage);
                }
            }
            else
            {
                AddError(invalidUrlMessage);
            }

            return View(model);
        }


        private void AddError(string message)
        {
            ModelState.AddModelError("ErrorMessage", localizer[message].Value);
        }
    }
}