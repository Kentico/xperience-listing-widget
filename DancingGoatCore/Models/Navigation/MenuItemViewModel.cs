using CMS.DocumentEngine;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    public class MenuItemViewModel
    {
        public string Caption { get; set; }


        public string Url { get; set; }


        public static MenuItemViewModel GetViewModel(TreeNode menuItem, IPageUrlRetriever pageUrlRetriever)
        {
            return new MenuItemViewModel
            {
                Caption = menuItem.DocumentName,
                Url = pageUrlRetriever.Retrieve(menuItem).RelativePath
            };
        }
    }
}