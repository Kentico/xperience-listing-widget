using System.Collections.Generic;

using DancingGoat.Models;

using Kentico.Xperience.ListingWidget;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// View model for articles transformations.
    /// </summary>
    public class ArticlesTransformationViewModel : ITransformationViewModel
    {
        /// <summary>
        /// Articles to display.
        /// </summary>
        public IEnumerable<ArticleViewModel> Articles { get; set; }
    }
}