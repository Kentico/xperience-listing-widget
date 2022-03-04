using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;

using Kentico.Content.Web.Mvc;
using Kentico.Xperience.ListingWidget.Transformations;

namespace DancingGoat.Widgets
{
    /// <inheritdoc/>
    public class CoffeesTransformationService : BaseTransformationService
    {
        private readonly IPageUrlRetriever pageUrlRetriever;


        /// <inheritdoc/>
        public override string PageType { get; } = Coffee.CLASS_NAME;


        /// <inheritdoc/>
        public override IEnumerable<Transformation> Transformations { get; } = new List<Transformation>
        {
            new Transformation
            {
                Name = "Coffees",
                View = "~/Components/Widgets/ListingWidget/Transformations/Coffees/_Coffees.cshtml",
                Description = "Transformation displays coffees in 4 column grid.",
            }
        };


        /// <summary>
        /// Creates an instance of <see cref="CoffeesTransformationService"/> class.
        /// </summary>
        /// <param name="pageUrlRetriever">Page URL retriever.</param>
        public CoffeesTransformationService(IPageUrlRetriever pageUrlRetriever)
        {
            this.pageUrlRetriever = pageUrlRetriever;
        }


        /// <summary>
        /// Returns hydrated <see cref="CoffeesTransformationViewModel"/> for pages.
        /// </summary>
        /// <param name="pages">Pages to be listed in view model.</param>
        public override ITransformationViewModel GetModel(IEnumerable<TreeNode> pages)
        {
            if (pages == null)
            {
                return new CoffeesTransformationViewModel();
            }
            var coffeesList = pages.Select(coffee => CoffeeCardViewModel.GetViewModel(coffee as Coffee, pageUrlRetriever));
            return new CoffeesTransformationViewModel { Coffees = coffeesList };
        }
    }
}
