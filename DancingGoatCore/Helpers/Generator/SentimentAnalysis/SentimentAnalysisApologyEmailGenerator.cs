using System;
using System.Collections.Specialized;

using CMS.Base;
using CMS.Core;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.Newsletters;
using CMS.Newsletters.Issues.Widgets.Configuration;

namespace DancingGoat.Helpers.Generator
{
    public class SentimentAnalysisApologyEmailGenerator
    {
        internal static readonly Guid ISSUE_GUID = Guid.Parse("d0b8c460-c865-4a5f-8ed5-349ab4fec01b");
        internal const string NEWSLETTER_NAME = "MarketingAutomationEngagement";
        private const string TEMPLATE_NAME = "CoreDemoGeneralEmail";
        private readonly ISiteInfo site;


        public SentimentAnalysisApologyEmailGenerator(ISiteInfo site)
        {
            this.site = site;
        }


        public void Generate()
        {
            var issue = IssueInfo.Provider.Get(ISSUE_GUID, site.SiteID);
            if (issue != null)
            {
                return;
            }

            var newsletter = NewsletterInfo.Provider.Get(NEWSLETTER_NAME, site.SiteID);
            issue = new IssueInfo
            {
                IssueGUID = ISSUE_GUID,
                IssueSiteID = site.SiteID,
                IssueDisplayName = "Unhappy client apology",
                IssueForAutomation = true,
                IssueNewsletterID = newsletter.NewsletterID,
                IssueSenderEmail = "dancinggoat@localhost.local",
                IssueSenderName = "Dancing Goat",
                IssueSubject = "Unhappy client apology",
                IssueTemplateID = GetTemplateID(),
                IssueUseUTM = false,
                IssueText = string.Empty
            };

            issue.Insert();

            SetWidgetsConfiguration(issue);
        }


        private void SetWidgetsConfiguration(IssueInfo issue)
        {
            var factory = Service.Resolve<IZonesConfigurationServiceFactory>();
            var service = factory.Create(issue);

            CreateWidget(service, "Headline", "zone1", 0, "Hello " + SignMacro("{%Recipient.FirstName%}") + ",");
            CreateWidget(service, "Text", "zone1", 1, "Thanks for your message.");
            CreateWidget(service, "Text", "zone1", 2, "We&#39;re deeply sorry if there was anything wrong with your Dancing Goat experience. We hope we&#39;ll figure this out together!");
            CreateWidget(service, "Text", "zone1", 3, "We&#39;ll get back to you soon,");
            CreateWidget(service, "Text", "zone1", 4, "Dancing Goat team.");
        }


        private void CreateWidget(IZonesConfigurationService service, string widgetTypeName, string zoneName, int index, string widgetValue)
        {
            var headline = EmailWidgetInfo.Provider.Get(widgetTypeName, site.SiteID);
            var widget = service.InsertWidget(headline.EmailWidgetGuid, zoneName, index);
            var properties = new NameValueCollection
            {
                { "Text", widgetValue }
            };

            service.StoreWidgetProperties(widget.Identifier, properties);
        }


        private int GetTemplateID()
        {
            return EmailTemplateInfo.Provider.Get(TEMPLATE_NAME, site.SiteID).TemplateID;
        }


        private static string SignMacro(string macro)
        {
            return MacroSecurityProcessor.AddSecurityParameters(macro, MacroIdentityOption.FromUserInfo(UserInfoProvider.AdministratorUser), null);
        }
    }
}
