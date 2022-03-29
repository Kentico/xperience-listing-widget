using CMS.ContactManagement;

namespace DancingGoat.Helpers.Generator
{
    /// <summary>
    /// Contains methods for generating sample contact groups.
    /// </summary>
    internal class ContactGroupGenerator
    {
        /// <summary>
        /// Generates three empty contact groups.
        /// </summary>
        public void Generate()
        {
            CreateContactGroup("CoffeeClubMembershipRecipients", "Coffee club membership recipients");
            CreateContactGroup("ProspectiveClients", "Prospective clients");
            CreateContactGroup("ImportedContacts", "Imported contacts");
        }


        private static void CreateContactGroup(string contactGroupCodeName, string contactGroupName)
        {
            var contactGroup = ContactGroupInfo.Provider.Get(contactGroupCodeName);
            if (contactGroup != null)
            {
                return;
            }

            contactGroup = new ContactGroupInfo
            {
                ContactGroupDisplayName = contactGroupName,
                ContactGroupName = contactGroupCodeName,
                ContactGroupEnabled = true
            };
            ContactGroupInfo.Provider.Set(contactGroup);
        }
    }
}