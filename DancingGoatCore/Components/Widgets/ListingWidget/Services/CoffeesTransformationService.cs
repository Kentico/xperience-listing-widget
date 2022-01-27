using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Provides methods to get coffees transformation model and custom parametrization for page retriever.
    /// </summary>
    public class CoffeesTransformationService : BaseTransformationService
    {
        private readonly IPageUrlRetriever pageUrlRetriever;


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
                return new CoffeesTransformationViewModel { Coffees = Enumerable.Empty<CoffeeCardViewModel>() };
            }
            var coffeesList = pages.Select(coffee => CoffeeCardViewModel.GetViewModel(coffee as Coffee, pageUrlRetriever));
            return new CoffeesTransformationViewModel { Coffees = coffeesList };
        }
    }
}
