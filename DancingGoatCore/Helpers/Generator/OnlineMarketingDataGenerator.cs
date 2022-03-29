using System;
using System.Linq;

using CMS.Activities;
using CMS.Base;
using CMS.ContactManagement;
using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.FormEngine;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Membership;
using CMS.OnlineForms;
using CMS.WebAnalytics.Internal;

namespace DancingGoat.Helpers.Generator
{
    internal class OnlineMarketingDataGenerator
    {
        private const string CEO_CONTACT_ROLE = "CEO";
        private const string BARISTA_CONTACT_ROLE = "Barista";

        private const string CONTACT_US_FORM_CODE_NAME = "DancingGoatCoreContactUsNew";
        private const string COFFEE_SAMPLE_LIST_FORM_CODE_NAME = "DancingGoatCoreCoffeeSampleList";

        private readonly TreeNode contactsDocument = DocumentHelper.GetDocuments()
                                                                       .All()
                                                                       .Culture("en-US")
                                                                       .Path(ContentItemIdentifiers.CONTACTS)
                                                                       .OnCurrentSite()
                                                                       .TopN(1)
                                                                       .FirstOrDefault();

        private readonly TreeNode coffeeSamplesDocument = DocumentHelper.GetDocuments()
                                                                       .All()
                                                                       .Culture("en-US")
                                                                       .Path(ContentItemIdentifiers.LANDINGPAGE_COFFEESAMPLES)
                                                                       .OnCurrentSite()
                                                                       .TopN(1)
                                                                       .FirstOrDefault();
        private readonly ISiteInfo mSite;
        private readonly IActivityUrlHashService mHashService;
        
        private ActivityTitleBuilder nameBuilder = new ActivityTitleBuilder();


        public OnlineMarketingDataGenerator(ISiteInfo currentSite, IActivityUrlHashService hashService)
        {
            mSite = currentSite;
            mHashService = hashService;
        }


        /// <summary>
        /// Performs contacts and activities generation.
        /// </summary>
        public void Generate()
        {
            GenerateMonicaKing();
            GenerateDustinEvans();
            GenerateHaroldLarson();
            GenerateToddRay();
            GenerateStacyStewart();
        }


        private void GenerateMonicaKing()
        {
            var monicaKing = GenerateContact("Monica", "King", "monica.king@localhost.local", "(595)-721-1648");
            monicaKing.ContactBirthday = DateTime.Today.AddYears(-35);
            monicaKing.ContactGender = (int)UserGenderEnum.Female;
            monicaKing.ContactJobTitle = BARISTA_CONTACT_ROLE;
            monicaKing.ContactMobilePhone = "+420123456789";

            monicaKing.ContactCity = "Brno";
            monicaKing.ContactAddress1 = "New Market 187/5";
            monicaKing.ContactZIP = "602 00";
            monicaKing.ContactCompanyName = "Air Cafe";
            monicaKing.ContactCountryID = CountryInfo.Provider.Get("CzechRepublic").CountryID;
            monicaKing.ContactNotes = "Should be involved in every communication with Air Cafe.";
            ContactInfo.Provider.Set(monicaKing);

            GeneratePageVisitActivity(contactsDocument, monicaKing);
            GeneratePageVisitActivity(coffeeSamplesDocument, monicaKing);
            CreateFormSubmission(coffeeSamplesDocument, COFFEE_SAMPLE_LIST_FORM_CODE_NAME, monicaKing);
            CreateFormSubmission(contactsDocument, CONTACT_US_FORM_CODE_NAME, monicaKing);
            GeneratePurchaseActivity(20, monicaKing);
        }
        

        private void GenerateDustinEvans()
        {
            var dustinEvans = GenerateContact("Dustin", "Evans", "Dustin.Evans@localhost.local", "(808)-139-4639");
            dustinEvans.ContactBirthday = DateTime.Today.AddYears(-40);
            dustinEvans.ContactGender = (int)UserGenderEnum.Male;
            dustinEvans.ContactJobTitle = CEO_CONTACT_ROLE;
            dustinEvans.ContactMobilePhone = "+420123456789";
            dustinEvans.ContactNotes = "Willing to participate in the partnership program - materials sent";

            dustinEvans.ContactCity = "South Yarra";
            dustinEvans.ContactAddress1 = "163 Commercial Road";
            dustinEvans.ContactZIP = "VIC 3141";
            dustinEvans.ContactCompanyName = "Jasper Coffee";
            dustinEvans.ContactCountryID = CountryInfo.Provider.Get("Australia").CountryID;
            dustinEvans.ContactNotes = "Willing to participate in the partnership program - materials sent";
            ContactInfo.Provider.Set(dustinEvans);
        }


        private void GenerateHaroldLarson()
        {
            var haroldLarson = GenerateContact("Harold", "Larson", "Harold.Larson@localhost.local", "(742)-343-5223");
            haroldLarson.ContactGender = (int)UserGenderEnum.Male;
            haroldLarson.ContactCountryID = CountryInfo.Provider.Get("USA").CountryID;
            haroldLarson.ContactCity = "Bedford";
            haroldLarson.ContactBounces = 5;
            haroldLarson.ContactStateID = StateInfo.Provider.Get("NewHampshire").StateID;
            ContactInfo.Provider.Set(haroldLarson);
        }


        private void GenerateStacyStewart()
        {
            var stacyStewart = GenerateContact("Stacy", "Stewart", "Stacy.Stewart@localhost.local", null);
            stacyStewart.ContactCountryID = CountryInfo.Provider.Get("Germany").CountryID;
            stacyStewart.ContactCity = "Berlin";
            stacyStewart.ContactNotes = "Contact acquired at CoffeeExpo2015";
            ContactInfo.Provider.Set(stacyStewart);
        }


        private void GenerateToddRay()
        {
            var toddRay = GenerateContact("Todd", "Ray", "Todd.Ray@localhost.local", "(808)-289-4459");
            toddRay.ContactBirthday = DateTime.Today.AddYears(-42);
            toddRay.ContactGender = (int)UserGenderEnum.Male;
            toddRay.ContactMobilePhone = "+420123456789";

            toddRay.ContactCity = "Brno";
            toddRay.ContactAddress1 = "Benesova 13";
            toddRay.ContactZIP = "612 00";
            toddRay.ContactCompanyName = "Air Cafe";
            toddRay.ContactCountryID = CountryInfo.Provider.Get("CzechRepublic").CountryID;
            toddRay.ContactNotes = "Should be involved in every communication with Air Cafe.";
            ContactInfo.Provider.Set(toddRay);
        }


        private ContactInfo GenerateContact(string firstName, string lastName, string email, string businessPhone)
        {
            var contact = new ContactInfo
            {
                ContactFirstName = firstName,
                ContactLastName = lastName,
                ContactEmail = email,
                ContactBusinessPhone = businessPhone,
                ContactMonitored = true
            };
            ContactInfo.Provider.Set(contact);
            return contact;
        }


        private void GeneratePageVisitActivity(TreeNode document, ContactInfo contact)
        {
            var nameBuilder = new ActivityTitleBuilder();
            var url = DocumentURLProvider.GetAbsoluteUrl(document);

            var activity = new ActivityInfo
            {
                ActivityContactID = contact.ContactID,
                ActivitySiteID = mSite.SiteID,
                ActivityTitle = nameBuilder.CreateTitle(PredefinedActivityType.PAGE_VISIT, document.GetDocumentName()),
                ActivityType = PredefinedActivityType.PAGE_VISIT,
                ActivityURL = url,
                ActivityURLHash = mHashService.GetActivityUrlHash(url)
            };

            ActivityInfo.Provider.Set(activity);
        }


        private void CreateFormSubmission(TreeNode document, string formName, ContactInfo contact)
        {
            
            var form = BizFormInfo.Provider.Get(formName, mSite.SiteID);
            var classInfo = DataClassInfoProvider.GetDataClassInfo(form.FormClassID);
            var formItem = new BizFormItem(classInfo.ClassName);
            var url = DocumentURLProvider.GetAbsoluteUrl(document);

            CopyDataFromContactToForm(contact, classInfo, formItem);
            SetFormSpecificData(formName, contact, formItem);
            formItem.Insert();

            var activity = new ActivityInfo
            {
                ActivityContactID = contact.ContactID,
                ActivityItemID = form.FormID,
                ActivityItemDetailID = formItem.ItemID,
                ActivitySiteID = mSite.SiteID,
                ActivityTitle = nameBuilder.CreateTitle(PredefinedActivityType.BIZFORM_SUBMIT, form.FormDisplayName),
                ActivityType = PredefinedActivityType.BIZFORM_SUBMIT,
                ActivityURL = url
            };

            ActivityInfo.Provider.Set(activity);
        }


        private void CopyDataFromContactToForm(ContactInfo contact, DataClassInfo classInfo, BizFormItem formItem)
        {
            var mapInfo = new FormInfo(classInfo.ClassContactMapping);
            var fields = mapInfo.GetFields(true, true);
            foreach (FormFieldInfo ffi in fields)
            {
                formItem.SetValue(ffi.MappedToField, contact.GetStringValue(ffi.Name, string.Empty));
            }
        }


        private void SetFormSpecificData(string formName, ContactInfo contact, BizFormItem formItem)
        {
            if (formName == COFFEE_SAMPLE_LIST_FORM_CODE_NAME)
            {
                formItem.SetValue("Country", CountryInfo.Provider.Get(contact.ContactCountryID).CountryThreeLetterCode);
                var state = StateInfo.Provider.Get(contact.ContactStateID);
                var stateName = state != null ? state.StateDisplayName : string.Empty;
                formItem.SetValue("State", stateName);
            }
            if (formName == CONTACT_US_FORM_CODE_NAME)
            {
                formItem.SetValue("UserMessage", "Message");
            }
        }


        private void GeneratePurchaseActivity(double spent, ContactInfo contact)
        {            
            var activity = new ActivityInfo
            {
                ActivityTitle = nameBuilder.CreateTitle(PredefinedActivityType.PURCHASE, "$" + spent),
                ActivityValue = spent.ToString(CultureHelper.EnglishCulture),
                ActivityItemID = 0,
                ActivityType = PredefinedActivityType.PURCHASE,
                ActivitySiteID = mSite.SiteID,
                ActivityContactID = contact.ContactID
            };

            ActivityInfo.Provider.Set(activity);
        }
    }
}