using System.Collections.Generic;

using Kentico.Xperience.ListingWidget.Widgets;

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
