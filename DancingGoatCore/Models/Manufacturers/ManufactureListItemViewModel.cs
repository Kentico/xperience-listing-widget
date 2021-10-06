using CMS.DocumentEngine.Types.DancingGoatCore;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    public class ManufactureListItemViewModel
    {
        public string Url { get; }


        public string Name { get; }


        public string ImagePath { get; }


        public ManufactureListItemViewModel(Manufacturer manufacturer, IPageUrlRetriever pageUrlRetriever)
        {
            Name = manufacturer.DocumentName;
            ImagePath = new FileUrl(manufacturer.Logo, true).RelativePath;
            Url = pageUrlRetriever.Retrieve(manufacturer).RelativePath;
        }
    }
}