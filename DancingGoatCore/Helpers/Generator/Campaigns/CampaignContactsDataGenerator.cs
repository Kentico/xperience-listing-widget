using System.Linq;

using CMS.ContactManagement;

namespace DancingGoat.Helpers.Generator
{
    internal class CampaignContactsDataGenerator
    {
        private const int NUMBER_OF_GENERATED_CONTACTS = 531;
        private readonly string mContactFirstNamePrefix;
        private readonly string mContactLastNamePrefix;


        /// <summary>
        /// Constructor.
        /// </summary>        
        public CampaignContactsDataGenerator(string contactFirstNamePrefix, string contactLastNamePrefix)
        {
            mContactFirstNamePrefix = contactFirstNamePrefix;
            mContactLastNamePrefix = contactLastNamePrefix;
        }

        /// <summary>
        /// Performs campaign contacts sample data generating.
        /// </summary>
        public void Generate()
        {
            DeleteOldContacts();
            GenerateContacts();
        }


        private void DeleteOldContacts()
        {
            ContactInfo.Provider.Get()
                               .WhereStartsWith("ContactFirstName", mContactFirstNamePrefix)
                               .ToList()
                               .ForEach(ContactInfo.Provider.Delete);
        }


        private void GenerateContacts()
        {
            for (var i = 0; i < NUMBER_OF_GENERATED_CONTACTS; i++)
            {
                var contact = new ContactInfo()
                {
                    ContactFirstName = mContactFirstNamePrefix + i,
                    ContactLastName = mContactLastNamePrefix + i,
                    ContactEmail = string.Format("{0}{1}@localhost.local", mContactFirstNamePrefix, i)
                };
                ContactInfo.Provider.Set(contact);
            }
        }
    }
}