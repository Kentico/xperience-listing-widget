using System.Collections.Generic;
using System.Linq;

using CMS.Helpers;

namespace DancingGoat.Models
{
    public class GrinderFilterCacheKey
    {
        private readonly GrinderFilterViewModel grinderFilter;


        public GrinderFilterCacheKey(GrinderFilterViewModel grinderFilter)
        {
            this.grinderFilter = grinderFilter;
        }


        public string GetCacheKey()
        {
            var manufacturers = GetCacheKeyForCollection(grinderFilter.Manufacturers);
            var prices = GetCacheKeyForCollection(grinderFilter.Prices);
            var types = GetCacheKeyForCollection(grinderFilter.Type);

            return string.Format($"{manufacturers}{TextHelper.NewLine}" +
                                 $"{prices}{TextHelper.NewLine}" +
                                 $"{types}");
        }


        private string GetCacheKeyForCollection(IEnumerable<GrindersProductFilterCheckboxViewModel> checkboxViewModels)
        {
            return checkboxViewModels
                .Select(type => string.Format($"{type.Value}:{type.IsChecked}"))
                .Join(TextHelper.NewLine);
        }
    }
}