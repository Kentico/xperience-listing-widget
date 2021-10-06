using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using CMS.DataEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;
using CMS.Helpers;

namespace DancingGoat.Models
{
    public class AccessoryFilterViewModel : IRepositoryFilter
    {
        private enum AccessoryType
        {
            Tableware,
            FilterPack
        }


        [UIHint("AccessoriesProductFilter")]
        public List<AccessoryProductFilterCheckboxViewModel> Type { get; set; }


        public AccessoryFilterViewModel()
        {
        }


        private List<AccessoryProductFilterCheckboxViewModel> GetTypes()
        {
            return new List<AccessoryProductFilterCheckboxViewModel>
            {
                new AccessoryProductFilterCheckboxViewModel { DisplayName = "Tableware", Value = (int)AccessoryType.Tableware },
                new AccessoryProductFilterCheckboxViewModel { DisplayName = "Filter pack", Value = (int)AccessoryType.FilterPack },
            };
        }


        public void Load()
        {
            Type = GetTypes();
        }


        public bool DisplayTablewares()
        {
            return Type.Any(box => box.Value.Equals((int)AccessoryType.Tableware) && box.IsChecked)
                || !Type.Any(box => box.IsChecked);
        }


        public bool DisplayFilterPacks()
        {
            return Type.Any(box => box.Value.Equals((int)AccessoryType.FilterPack) && box.IsChecked)
                || !Type.Any(box => box.IsChecked);
        }


        public string GetCacheKey()
        {
            return Type
               .Select(type => string.Format($"{type.Value}:{type.IsChecked}"))
               .Join(TextHelper.NewLine);            
        }


        public WhereCondition GetWhereCondition()
        {
            var where = new WhereCondition();

            if (DisplayFilterPacks() && !DisplayTablewares())
            {
                where.WhereEquals("ClassName", FilterPack.CLASS_NAME);
            }

            if (!DisplayFilterPacks() && DisplayTablewares())
            {
                where.WhereEquals("ClassName", Tableware.CLASS_NAME);
            }

            return where;
        }
    }
}