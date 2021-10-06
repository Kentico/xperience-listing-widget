using System.Threading.Tasks;

using CMS.Helpers;
using CMS.Newsletters;
using CMS.SiteProvider;

using Kentico.Membership;
using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Controller for newsletter subscription widget.
    /// </summary>
    public class NewsletterSubscriptionWidgetController : Controller
    {
        private readonly ApplicationUserManager<ApplicationUser> userManager;
        private SubscribeSettings mNewsletterSubscriptionSettings;
        private readonly ISubscriptionService subscriptionService;
        private readonly IContactProvider contactProvider;
        private readonly INewsletterInfoProvider newsletterInfoProvider;
        private readonly IComponentPropertiesRetriever componentPropertiesRetriever;
        private readonly IStringLocalizer<NewsletterSubscriptionWidgetController> localizer;


        private SubscribeSettings NewsletterSubscriptionSettings
        {
            get
            {
                return mNewsletterSubscriptionSettings ?? (mNewsletterSubscriptionSettings = new SubscribeSettings
                {
                    AllowOptIn = true,
                    RemoveUnsubscriptionFromNewsletter = true,
                    RemoveAlsoUnsubscriptionFromAllNewsletters = true,
                    SendConfirmationEmail = true
                });
            }
        }


        /// <summary>
        /// Creates an instance of <see cref="NewsletterSubscriptionWidgetController"/> class.
        /// </summary>
        /// <param name="subscriptionService">Service for newsletter subscription.</param>
        /// <param name="contactProvider">Provider for contact retrieving.</param>
        /// <param name="newsletterInfoProvider">Provider for <see cref="NewsletterInfo"/> management.</param>
        public NewsletterSubscriptionWidgetController(
            ApplicationUserManager<ApplicationUser> userManager, 
            ISubscriptionService subscriptionService, 
            IContactProvider contactProvider,
            INewsletterInfoProvider newsletterInfoProvider,
            IComponentPropertiesRetriever componentPropertiesRetriever,
            IStringLocalizer<NewsletterSubscriptionWidgetController> localizer)
        {
            this.userManager = userManager;
            this.subscriptionService = subscriptionService;
            this.contactProvider = contactProvider;
            this.newsletterInfoProvider = newsletterInfoProvider;
            this.componentPropertiesRetriever = componentPropertiesRetriever;
            this.localizer = localizer;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Subscribe(NewsletterSubscriptionSubscribeModel model)
        {
            if (!ValidationHelper.IsEmail(model.Email))
            {
                ModelState.AddModelError("Email", "Invalid email.");

                return PartialView("~/Components/Widgets/NewsletterSubscriptionWidget/_Subscribe.cshtml", model);
            }

            var properties = componentPropertiesRetriever.Retrieve<NewsletterSubscriptionProperties>();
            var newsletter = newsletterInfoProvider.Get(properties.Newsletter, SiteContext.CurrentSiteID);
            var contact = contactProvider.GetContactForSubscribing(model.Email);

            string resultMessage;
            if (!subscriptionService.IsMarketable(contact, newsletter))
            {
                subscriptionService.Subscribe(contact, newsletter, NewsletterSubscriptionSettings);

                // The subscription service is configured to use double opt-in, but newsletter must allow for it
                resultMessage = localizer[newsletter.NewsletterEnableOptIn ?
                    "A confirmation link has been sent to your email." : 
                    "You have been subscribed."];
            }
            else
            {
                resultMessage = localizer["You are already subscribed."];
            }

            return Content(resultMessage);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubscribeAuthenticated()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return ViewComponent(typeof(NewsletterSubscriptionViewComponent));
            }

            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var contact = contactProvider.GetContactForSubscribing(user.Email, user.FirstName, user.LastName);
            var properties = componentPropertiesRetriever.Retrieve<NewsletterSubscriptionProperties>();
            var newsletter = newsletterInfoProvider.Get(properties.Newsletter, SiteContext.CurrentSiteID);

            string resultMessage;
            if (!subscriptionService.IsMarketable(contact, newsletter))
            {
                subscriptionService.Subscribe(contact, newsletter, NewsletterSubscriptionSettings);

                // The subscription service is configured to use double opt-in, but newsletter must allow for it
                resultMessage = localizer[newsletter.NewsletterEnableOptIn ?
                    "A confirmation link has been sent to your email." :
                    "You have been subscribed."];
            }
            else
            {
                resultMessage = localizer["You are already subscribed."];
            }

            return Content(resultMessage);
        }
    }
}