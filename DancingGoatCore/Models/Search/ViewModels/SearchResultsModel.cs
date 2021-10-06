using System.Collections.Generic;

namespace DancingGoat.Models
{
    public class SearchResultsModel
    {
        public string Query { get; set; }

        public List<SearchResultItemModel> Items { get; set; }


        public int CurrentPage { get; set; }


        public int NumberOfPages { get; set; }


        public Dictionary<string, string> GetRouteData(int page) =>
            new Dictionary<string, string>() { { "searchText", Query }, { "page", page.ToString() } };
    }
}