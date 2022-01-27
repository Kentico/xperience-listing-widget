using System.Collections.Generic;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// View model for coffees transformations.
    /// </summary>
    public class CoffeesTransformationViewModel : ITransformationViewModel
    {
        /// <summary>
        /// Coffees to display.
        /// </summary>
        public IEnumerable<CoffeeCardViewModel> Coffees { get; set; }
    }
}
