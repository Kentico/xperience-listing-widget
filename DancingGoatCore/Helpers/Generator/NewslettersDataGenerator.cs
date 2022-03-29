using System;
using System.Collections.Generic;
using System.Linq;

using CMS.Base;
using CMS.ContactManagement;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Newsletters;

namespace DancingGoat.Helpers.Generator
{
    /// <summary>
    /// Contains methods for generating sample data for the Newsletters module.
    /// </summary>
    internal class NewslettersDataGenerator
    {
        private readonly NewsletterActivityGenerator mNewsletterActivityGenerator = new NewsletterActivityGenerator();

        private const string NEWSLETTER_DANCING_GOAT = "Newsletter";
        private const string NEWSLETTER_COFFEE101 = "Coffee101";

        private readonly ISiteInfo site;
        private readonly Random rand = new Random(DateTime.Now.Millisecond);
        private readonly string[] subscriberNames =
        {
            "Deneen Fernald", "Antonio Buker", "Marlon Loos", "Nolan Steckler", "Johnetta Tall",
            "Florence Ramsdell", "Modesto Speaker", "Alissa Ferguson", "Calvin Hollier", "Diamond Paik",
            "Mardell Dohrmann", "Dinorah Clower", "Andrea Humbert", "Tyrell Galvan", "Yong Inskeep",
            "Tom Goldschmidt", "Kimbery Rincon", "Genaro Kneeland", "Roselyn Mulvey", "Nancee Jacobson",
            "Jaime Link", "Fonda Belnap", "Muoi Ishmael", "Pearlene Minjarez", "Eustolia Studstill",
            "Marilynn Manos", "Pamila Turnbow", "Lieselotte Satcher", "Sharron Mellon", "Bennett Heatherington",
            "Spring Hessel", "Lashay Blazier", "Veronika Lecuyer", "Mark Spitz", "Peggy Olson",
            "Tyron Bednarczyk", "Terese Betty", "Bibi Kling", "Bruno Spier", "Cristen Bussey",
            "Daine Pridemore", "Gerald Turpen", "Lela Briese", "Sharda Bonnie", "Omar Martin",
            "Marlyn Pettie", "Shiela Cleland", "Marica Granada", "Garland Reagan", "Mora Gillmore",
            "Mariana Rossow", "Betty Pollan", "Analisa Costilla", "Evelyn Mendez", "April Rubino",
            "Zachariah Roberson", "Sheilah Steinhauser", "Araceli Vallance", "Lashawna Weise", "Charline Durante",
            "Melania Nightingale", "Ema Stiltner", "Lynelle Threet", "Dorcas Cully", "Gregg Carranco",
            "Karla Heiner", "Judson Siegmund", "Alyson Oday", "Winston Laxton", "Jarod Turrentine",
            "Israel Shanklin", "Miquel Jorstad", "Brianne Darrow", "Tamara Rulison", "Elliot Rameriz",
            "Gearldine Nova", "Debi Fritts", "Leota Cape", "Tyler Saleem", "Starr Hyden",
            "Loreen Spigner", "Raisa Germain", "Grace Vigue", "Maryann Munsch", "Jason Chon",
            "Gisele Mcquillen", "Juliane Comeaux", "Willette Dodrill", "Sherril Weymouth", "Ashleigh Dearman",
            "Bret Bourne", "Brittney Cron", "Lashell Hampson", "Barbie Dinwiddie", "Ricki Wiener",
            "Bess Pedretti", "Lisha Raley", "Edgar Schuetz", "Jettie Boots", "Jefferson Hinkle"
        };


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="site">Site the newsletters data will be generated for</param>
        public NewslettersDataGenerator(ISiteInfo site)
        {
            this.site = site;
        }


        /// <summary>
        /// Performs newsletters sample data generation.
        /// </summary>
        public void Generate()
        {
            GenerateSubscribersForExistingContacts();
            GenerateNewsletterSubscribers();
            CreateSomeGlobalUnsubscriptions();
        }


        private void GenerateSubscribersForExistingContacts()
        {
            SubscribeContactWithEmailToNewsletter("monica.king@localhost.local", NEWSLETTER_DANCING_GOAT);
            SubscribeContactWithEmailToNewsletter("monica.king@localhost.local", NEWSLETTER_COFFEE101);
            SubscribeContactWithEmailToNewsletter("Dustin.Evans@localhost.local", NEWSLETTER_DANCING_GOAT);
            SubscribeContactWithEmailToNewsletter("Dustin.Evans@localhost.local", NEWSLETTER_COFFEE101);
            SubscribeContactWithEmailToNewsletter("Todd.Ray@localhost.local", NEWSLETTER_COFFEE101);
            SubscribeContactWithEmailToNewsletter("Stacy.Stewart@localhost.local", NEWSLETTER_COFFEE101);
            SubscribeContactWithEmailToNewsletter("Harold.Larson@localhost.local", NEWSLETTER_DANCING_GOAT);
        }


        private void SubscribeContactWithEmailToNewsletter(string contactEmail, string newsletterCodeName)
        {
            var contact = ContactInfoProvider.GetContactInfo(contactEmail);
            var newsletter = NewsletterInfo.Provider.Get(newsletterCodeName, site.SiteID);
            var fullName = string.Format("{0} {1}", contact.ContactFirstName, contact.ContactLastName);

            var subscriber = CreateSubscriber(contact.ContactEmail, contact.ContactFirstName, contact.ContactLastName, fullName, contact);
            AssignSubscriberToNewsletter(newsletter.NewsletterID, subscriber);
            mNewsletterActivityGenerator.GenerateNewsletterSubscribeActivity(newsletter, subscriber.SubscriberID, contact.ContactID, site.SiteID);
        }


        private void GenerateNewsletterSubscribers()
        {
            var coffee101Newsletter = NewsletterInfo.Provider.Get("Coffee101", site.SiteID);
            if (coffee101Newsletter == null)
            {
                return;
            }

            var subscribers = GenerateSubscribers();
            AssignAllSubscribersToNewsletter(subscribers, coffee101Newsletter.NewsletterID);
            AdjustExistingIssues(subscribers);
        }


        private IList<SubscriberInfo> GenerateSubscribers()
        {
            var generatedSubscribers = new List<SubscriberInfo>();

            for (int i = 0; i < subscriberNames.Count(); i++)
            {
                var subscriberData = SubscriberData.CreateFromFullName(subscriberNames[i]);

                var contact = CreateContact(subscriberData.FirstName, subscriberData.LastName, subscriberData.Email);
                var subscriber = CreateSubscriber(subscriberData.Email, subscriberData.FirstName, subscriberData.LastName, subscriberNames[i], contact);
                generatedSubscribers.Add(subscriber);
            }

            return generatedSubscribers;
        }


        private SubscriberInfo CreateSubscriber(string email, string firstName, string lastName, string fullName, ContactInfo contact)
        {
            var subscriber = SubscriberInfoProvider.GetSubscriberByEmail(email, site.SiteID);
            if (subscriber != null)
            {
                return subscriber;
            }

            subscriber = new SubscriberInfo
            {
                SubscriberEmail = email,
                SubscriberFirstName = firstName,
                SubscriberLastName = lastName,
                SubscriberSiteID = site.SiteID,
                SubscriberFullName = fullName,
                SubscriberRelatedID = contact.ContactID,
                SubscriberType = PredefinedObjectType.CONTACT
            };
            SubscriberInfo.Provider.Set(subscriber);

            return subscriber;
        }


        private ContactInfo CreateContact(string firstName, string lastName, string subscriberEmail)
        {
            var contact = new ContactInfo
            {
                ContactFirstName = firstName,
                ContactLastName = lastName,
                ContactEmail = subscriberEmail
            };
            ContactInfo.Provider.Set(contact);
            return contact;
        }


        private void AssignAllSubscribersToNewsletter(IEnumerable<SubscriberInfo> subscribers, int newsletterId)
        {
            foreach (var subscriber in subscribers)
            {
                AssignSubscriberToNewsletter(newsletterId, subscriber);
            }
        }


        private void AssignSubscriberToNewsletter(int newsletterId, SubscriberInfo subscriber)
        {
            var subscription = new SubscriberNewsletterInfo
            {
                SubscriberID = subscriber.SubscriberID,
                NewsletterID = newsletterId,
                SubscriptionApproved = true,
                SubscribedWhen = DateTime.Now,
            };

            SubscriberNewsletterInfo.Provider.Set(subscription);
        }


        private void AdjustExistingIssues(IList<SubscriberInfo> subscribers)
        {
            var lesson1 = IssueInfo.Provider.Get().OnSite(site.SiteID).WhereEquals("IssueSubject", "Coffee 101 - Lesson 1").FirstOrDefault();
            var lesson2 = IssueInfo.Provider.Get().OnSite(site.SiteID).WhereEquals("IssueSubject", "Coffee 101 - Lesson 2").FirstOrDefault();

            if ((lesson1 == null) || (lesson2 == null))
            {
                return;
            }

            SettingsKeyInfoProvider.SetValue("CMSMonitorBouncedEmails", site.SiteName, true);

            const int uniqueOpensLesson1 = 26;
            const int uniqueOpensLesson2 = 14;

            lesson1.IssueSentEmails = 98;
            lesson1.IssueBounces = 2;
            lesson1.IssueOpenedEmails = uniqueOpensLesson1;
            lesson1.IssueUnsubscribed = 5;
            IssueInfo.Provider.Set(lesson1);

            lesson2.IssueSentEmails = 98;
            lesson2.IssueBounces = 0;
            lesson2.IssueOpenedEmails = uniqueOpensLesson2;
            lesson2.IssueUnsubscribed = 3;
            IssueInfo.Provider.Set(lesson2);

            var subscribersEmails = subscribers.Select(s => s.SubscriberEmail).ToList();

            var lessonLinkTarget = string.Format("http://{0}{1}/en-US/Product/580275af-66b4-4c94-8887-e871d00c139c/Ethiopia-Yirgacheffe", RequestContext.CurrentDomain, SystemContext.ApplicationPath);

            GenerateClickedLinksToIssue(lesson1.IssueID, lessonLinkTarget, 17, 12, subscribersEmails);
            GenerateClickedLinksToIssue(lesson2.IssueID, lessonLinkTarget, 5, 2, subscribersEmails);

            GenerateOpenedEmailToIssue(lesson1.IssueID, uniqueOpensLesson1, subscribersEmails);
            GenerateOpenedEmailToIssue(lesson2.IssueID, uniqueOpensLesson2, subscribersEmails);

            GenerateUnsubscriptionsFromIssue(lesson1.IssueID, lesson1.IssueNewsletterID, 5, subscribers);
            GenerateUnsubscriptionsFromIssue(lesson2.IssueID, lesson2.IssueNewsletterID, 3, subscribers);
        }


        /// <summary>
        /// Total clicks value must not be higher then the count of subscribers.
        /// </summary>
        private void GenerateClickedLinksToIssue(int issueId, string linkTarget, int totalClicks, int uniqueClicks, IList<string> subscribersEmails)
        {
            var link = new LinkInfo
            {
                LinkIssueID = issueId,
                LinkTarget = linkTarget,
                LinkDescription = "Try Ethiopian Coffee"
            };
            LinkInfo.Provider.Set(link);

            for (var i = 0; i < totalClicks; i++)
            {
                // Simulate non-unique clicks
                var subscriberIndex = (i <= (totalClicks - uniqueClicks)) ? 0 : i;

                var clickedLink = new ClickedLinkInfo
                {
                    ClickedLinkEmail = subscribersEmails[subscriberIndex],
                    ClickedLinkTime = GetRandomDate(DateTime.Now.AddMonths(-1), DateTime.Now.AddDays(-1)),
                    ClickedLinkNewsletterLinkID = link.LinkID,
                };
                ClickedLinkInfo.Provider.Set(clickedLink);
            }
        }


        /// <summary>
        /// Unique opens value must not be higher then the count of subscribers.
        /// </summary>
        private void GenerateOpenedEmailToIssue(int issueId, int uniqueOpens, IList<string> subscribersEmails)
        {
            for (var i = 0; i < uniqueOpens; i++)
            {
                var openedEmail = new OpenedEmailInfo
                {
                    OpenedEmailEmail = subscribersEmails[i],
                    OpenedEmailIssueID = issueId,
                    OpenedEmailTime = GetRandomDate(DateTime.Now.AddMonths(-1), DateTime.Now.AddDays(-1)),
                };
                OpenedEmailInfo.Provider.Set(openedEmail);
            }
        }


        /// <summary>
        /// Unsubscribed value must not be higher then the count of subscribers.
        /// </summary>
        private void GenerateUnsubscriptionsFromIssue(int issueId, int newsletterId, int unsubscribed, IList<SubscriberInfo> subscribers)
        {
            for (var i = 0; i < unsubscribed; i++)
            {
                CreateUnsubscription(issueId, newsletterId, subscribers[i].SubscriberEmail);
            }
        }


        private void CreateUnsubscription(int issueId, int newsletterId, string unsubscriptionEmail)
        {
            var unsubscription = new UnsubscriptionInfo
            {
                UnsubscriptionEmail = unsubscriptionEmail,
                UnsubscriptionNewsletterID = newsletterId,
                UnsubscriptionFromIssueID = issueId,
            };
            unsubscription.Insert();
        }


        private DateTime GetRandomDate(DateTime from, DateTime to)
        {
            var range = to - from;
            var randTimeSpan = new TimeSpan((long)(rand.NextDouble() * range.Ticks));
            return from + randTimeSpan;
        }


        private void CreateSomeGlobalUnsubscriptions()
        {
            for (int index = 0; index < subscriberNames.Length; index++)
            {
                // some of them unsubscribe
                if (index % 10 == 0)
                {
                    var subscriberData = SubscriberData.CreateFromFullName(subscriberNames[index]);
                    CreateGlobalUnsubscription(subscriberData.Email);
                }
            }
        }


        private void CreateGlobalUnsubscription(string unsubscriptionEmail)
        {
            var unsubscription = new UnsubscriptionInfo
            {
                UnsubscriptionEmail = unsubscriptionEmail,
            };
            unsubscription.Insert();
        }


        private class SubscriberData
        {
            public string FirstName
            {
                get; private set;
            }


            public string LastName
            {
                get; private set;
            }


            public string Email
            {
                get;
                private set;
            }


            public static SubscriberData CreateFromFullName(string fullName)
            {
                var words = fullName.Trim().Split(' ');
                var firstName = words[0];
                var lastName = words[1];
                var email = string.Format("{0}@{1}.local", firstName.ToLowerInvariant(), lastName.ToLowerInvariant());

                return new SubscriberData()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email
                };
            }
        }
    }
}
