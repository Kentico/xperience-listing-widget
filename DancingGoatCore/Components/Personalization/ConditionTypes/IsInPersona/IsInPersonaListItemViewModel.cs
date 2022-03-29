using Kentico.Content.Web.Mvc;

namespace DancingGoat.Personalization
{
    public class IsInPersonaListItemViewModel
    {
        public string DisplayName
        {
            get;
            set;
        }


        public string CodeName
        {
            get;
            set;
        }


        public string ImagePath
        {
            get;
            set;
        }


        public bool Selected
        {
            get;
            set;
        }
    }
}