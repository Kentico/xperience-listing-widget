using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Search;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    public class SearchResultPageItemModel : SearchResultItemModel
    {
        public string Type { get; set; }


        public string Url { get; set; }
        

        public SearchResultPageItemModel(SearchResultItem resultItem, TreeNode page, IPageUrlRetriever pageUrlRetriever)
            : base(resultItem)
        {
            var className = page.ClassName;
            var dataClassInfo = DataClassInfoProvider.GetDataClassInfo(className);

            Type = dataClassInfo.ClassDisplayName;
            Url = pageUrlRetriever?.Retrieve(page).RelativePath;
        }
    }
}