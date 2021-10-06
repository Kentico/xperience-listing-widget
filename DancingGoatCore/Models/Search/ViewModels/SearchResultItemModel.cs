using System;

using CMS.Helpers;
using CMS.Search;

namespace DancingGoat.Models
{
    public class SearchResultItemModel
    {
        public string Title { get; set; }


        public string Content { get; set; }


        public DateTime Date { get; set; }


        public string ObjectType { get; set; }


        public string ImagePath { get; set; }


        public SearchResultItemModel(SearchResultItem resultItem)
        {
            Title = resultItem.Title;
            Content = HTMLHelper.StripTags(resultItem.Content, false);
            Date = resultItem.Created;
            ImagePath = resultItem.GetImageUrl();
            ObjectType = resultItem.Type;
        }
    }
}