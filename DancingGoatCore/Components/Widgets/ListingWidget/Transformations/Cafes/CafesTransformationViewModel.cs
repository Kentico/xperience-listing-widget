using System.Collections.Generic;

using DancingGoat.Models;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// View model for cafes transformations.
    /// </summary>
    public class CafesTransformationViewModel : ITransformationViewModel
    {
        /// <summary>
        /// Cafes to display.
        /// </summary>
        public IEnumerable<CafeViewModel> Cafes { get; set; }
    }
}
