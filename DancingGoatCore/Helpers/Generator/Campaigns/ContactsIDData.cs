using System.Linq;

using CMS.DataEngine;

namespace DancingGoat.Helpers.Generator
{
    /// <summary>
    /// Represents IDs of all contacts whose first name starts with the same prefix.
    /// </summary>
    internal class ContactsIDData
    {
        private int mCurrectContact;
        private readonly int mContactsCount;
        private readonly int[] mContactIDs;


        /// <summary>
        /// Creates the instance of <see cref="ContactsIDData"/>.
        /// </summary>
        /// <param name="firstNamePrefix">Commont first name prefix of contacts.</param>
        /// <param name="contactsCount">Represents number of contact IDs to retrieve. If value is <c>0</c> then all contact IDs with <paramref name="firstNamePrefix"/> are retrieved.</param>
        public ContactsIDData(string firstNamePrefix, int contactsCount)
        {

            mContactIDs = new ObjectQuery(PredefinedObjectType.CONTACT)
                .Column("ContactID")
                .WhereStartsWith("ContactFirstName", firstNamePrefix)
                .TopN(contactsCount)
                .GetListResult<int>()
                .ToArray();

            mCurrectContact = 0;
            mContactsCount = mContactIDs.Length;
        }



        /// <summary>
        /// Returns ID of the next contact.
        /// </summary>
        /// <returns>Contact ID.</returns>
        public int GetNextContactID()
        {
            var contactID = mContactIDs[mCurrectContact];
            mCurrectContact = (mCurrectContact + 1) % mContactsCount;

            return contactID;
        }
    }
}