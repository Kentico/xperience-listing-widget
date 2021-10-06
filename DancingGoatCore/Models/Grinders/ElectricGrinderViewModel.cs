using CMS.DocumentEngine.Types.DancingGoatCore;

namespace DancingGoat.Models
{
    public class ElectricGrinderViewModel : ITypedProductViewModel
    {
        public int Power { get; set; }


        public static ElectricGrinderViewModel GetViewModel(ElectricGrinder grinder)
        {
            return new ElectricGrinderViewModel
            {
                Power = grinder.ElectricGrinderPower
            };
        }
    }
}