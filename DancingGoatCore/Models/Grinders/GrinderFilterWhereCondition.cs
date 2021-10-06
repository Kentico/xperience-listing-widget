using System;
using System.Collections.Generic;
using System.Linq;

using CMS.DataEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;

namespace DancingGoat.Models
{
    public class GrinderFilterWhereCondition
    {
        private readonly GrinderFilterViewModel grinderFilter;


        public GrinderFilterWhereCondition(GrinderFilterViewModel grinderFilter)
        {
            this.grinderFilter = grinderFilter;
        }


        public WhereCondition  GetWhereCondition()
        {
            var where = new WhereCondition()
                .And(GetManufacturerWhereCondition())
                .And(GetPriceWhereCondition());

            if (grinderFilter.DisplayElectricGrinders() && !grinderFilter.DisplayManualGrinders())
            {
                where = where.WhereEquals("ClassName", ElectricGrinder.CLASS_NAME);
            }

            if (!grinderFilter.DisplayElectricGrinders() && grinderFilter.DisplayManualGrinders())
            {
                where = where.WhereEquals("ClassName", ManualGrinder.CLASS_NAME);
            }

            return where;
        }


        private WhereCondition GetManufacturerWhereCondition()
        {
            var where = new WhereCondition();

            var selectedManufacturerIds = GetSelectedManufacturerIds();

            if (selectedManufacturerIds.Any())
            {
                where.WhereIn("SKUManufacturerID", selectedManufacturerIds);
            }

            return where;
        }


        private WhereCondition GetPriceWhereCondition()
        {
            var where = new WhereCondition();

            foreach (var range in GetSelectedRanges())
            {
                where.Or(GetWhereConditionForPriceRange(range));
            }

            return where;
        }


        private IEnumerable<GrinderPriceRangeEnum> GetSelectedRanges()
        {
            return grinderFilter.Prices
                .Where(price => price.IsChecked)
                .Select(GetRangeFromSelectedValue);
        }


        private static GrinderPriceRangeEnum GetRangeFromSelectedValue(GrindersProductFilterCheckboxViewModel price)
        {
            return (GrinderPriceRangeEnum)price.Value;
        }


        private static WhereCondition GetWhereConditionForPriceRange(GrinderPriceRangeEnum range)
        {
            switch (range)
            {
                case GrinderPriceRangeEnum.ToFifty:
                    return GetPriceRangeWhereCondition(0, 50);

                case GrinderPriceRangeEnum.FromFiftyToFiveHundred:
                    return GetPriceRangeWhereCondition(50, 500);

                case GrinderPriceRangeEnum.FromFiveHundredToFiveThousand:
                    return GetPriceRangeWhereCondition(500, 5000);

                default:
                    throw new ArgumentOutOfRangeException(nameof(range));
            }
        }


        private static WhereCondition GetPriceRangeWhereCondition(int from, int to)
        {
            return new WhereCondition()
                .WhereGreaterOrEquals("SKUPrice", from)
                .And().WhereLessThan("SKUPrice", to);
        }


        private List<int> GetSelectedManufacturerIds()
        {
            return grinderFilter.Manufacturers
                .Where(x => x.IsChecked)
                .Select(x => x.Value)
                .ToList();
        }
    }
}