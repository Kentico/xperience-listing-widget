using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using CMS.DataEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;

namespace DancingGoat.Models
{
    public class GrinderFilterViewModel : IRepositoryFilter
    {
        private readonly string[] grinderClasses = new[] { ManualGrinder.CLASS_NAME, ElectricGrinder.CLASS_NAME };
        private readonly ProductRepository productRepository;

        private enum GrinderTypeEnum
        {
            Electric,
            Manual
        }


        public GrinderFilterViewModel()
        {
        }


        public GrinderFilterViewModel(ProductRepository productRepository)
        {
            this.productRepository = productRepository;            
        }


        [UIHint("GrindersProductFilter")]
        public List<GrindersProductFilterCheckboxViewModel> Manufacturers { get; set; }

        [UIHint("GrindersProductFilter")]
        public List<GrindersProductFilterCheckboxViewModel> Prices { get; set; }

        [UIHint("GrindersProductFilter")]
        public List<GrindersProductFilterCheckboxViewModel> Type { get; set; }


        public void Load()
        {
            Manufacturers = GetManufacturers();
            Prices = GetPrices();
            Type = GetTypes();
        }


        private List<GrindersProductFilterCheckboxViewModel> GetManufacturers()
        {
            var products = productRepository.GetProducts(null, grinderClasses);

            var manufacturers = products.OfType<ManualGrinder>()
                .Select(product => product.Product.Manufacturer)
                .Concat(products.OfType<ElectricGrinder>().Select(product => product.Product.Manufacturer))
                .Distinct()
                .OrderBy(manufacturer => manufacturer.ManufacturerDisplayName);


            return manufacturers.Select(manufacturer => new GrindersProductFilterCheckboxViewModel
            {
                DisplayName = manufacturer.ManufacturerDisplayName,
                Value = manufacturer.ManufacturerID
            }).ToList();
        }


        private List<GrindersProductFilterCheckboxViewModel> GetPrices()
        {
            return new List<GrindersProductFilterCheckboxViewModel>
            {
                new GrindersProductFilterCheckboxViewModel {DisplayName = "$0 - $50", Value = (int)GrinderPriceRangeEnum.ToFifty},
                new GrindersProductFilterCheckboxViewModel {DisplayName = "$50 - $500", Value = (int)GrinderPriceRangeEnum.FromFiftyToFiveHundred},
                new GrindersProductFilterCheckboxViewModel {DisplayName = "$500 - $5000", Value = (int)GrinderPriceRangeEnum.FromFiveHundredToFiveThousand}
            };
        }

                
        private List<GrindersProductFilterCheckboxViewModel> GetTypes()
        {
            return new List<GrindersProductFilterCheckboxViewModel>
            {
                new GrindersProductFilterCheckboxViewModel { DisplayName = "Electric", Value = (int)GrinderTypeEnum.Electric },
                new GrindersProductFilterCheckboxViewModel { DisplayName = "Manual", Value = (int)GrinderTypeEnum.Manual },
            };
        }


        public string GetCacheKey()
        {
            return new GrinderFilterCacheKey(this).GetCacheKey();
        }


        public WhereCondition GetWhereCondition()
        {
            return new GrinderFilterWhereCondition(this).GetWhereCondition();
        }


        public bool DisplayElectricGrinders()
        {
            return Type.Any(box => box.Value.Equals((int)GrinderTypeEnum.Electric) && box.IsChecked)
                || !Type.Any(box => box.IsChecked);
        }


        public bool DisplayManualGrinders()
        {
            return Type.Any(box => box.Value.Equals((int)GrinderTypeEnum.Manual) && box.IsChecked)
                || !Type.Any(box => box.IsChecked);
        }
    }
}