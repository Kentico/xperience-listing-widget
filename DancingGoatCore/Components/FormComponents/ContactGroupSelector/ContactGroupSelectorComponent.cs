using System.Collections.Generic;
using System.Linq;

using CMS.ContactManagement;
using Kentico.Forms.Web.Mvc;

using DancingGoat.FormComponents;


[assembly: RegisterFormComponent("DancingGoat.ContactGroupSelector", typeof(ContactGroupSelectorComponent), "Contact group selector", IsAvailableInFormBuilderEditor = false, ViewName = "~/Components/FormComponents/ContactGroupSelector/_ContactGroupSelector.cshtml")]
namespace DancingGoat.FormComponents
{
    /// <summary>
    /// Represents a contact group selector
    /// </summary>
    public class ContactGroupSelectorComponent : FormComponent<ContactGroupSelectorProperties, List<string>>
    {
        [BindableProperty]
        public List<ContactGroupSelectorListItem> Items
        {
            get;
            set;
        } = GetItems();


        public override List<string> GetValue()
        {
            var selectedCodeName = Items.Where(x => x.Checked)
                                        .Select(x => x.CodeName).ToList();

            return selectedCodeName;
        }


        public override void SetValue(List<string> value)
        {           
            var items = GetItems();
            if (value != null)
            {
                items.ForEach(x => x.Checked = value.Contains(x.CodeName));
            }

            Items = items;
        }


        private static List<ContactGroupSelectorListItem> GetItems()
        {
            return ContactGroupInfo.Provider.Get().Select(x => new ContactGroupSelectorListItem
            {
                CodeName = x.ContactGroupName,
                DisplayName = x.ContactGroupDisplayName,
                Checked = false
            }).ToList();
        }
    }
}