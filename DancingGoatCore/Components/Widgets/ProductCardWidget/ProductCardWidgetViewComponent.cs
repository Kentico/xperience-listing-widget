using System.Linq;

using DancingGoat.Models;
using DancingGoat.Widgets;

using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

[assembly: RegisterWidget(ProductCardWidgetViewComponent.IDENTIFIER, typeof(ProductCardWidgetViewComponent), "Product card", typeof(ProductCardProperties), Description = "Displays a product.", IconClass = "icon-box")]

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Controller for product card widget.
    /// </summary>
    public class ProductCardWidgetViewComponent : ViewComponent
    {
        /// <summary>
        /// Widget identifier.
        /// </summary>
        public const string IDENTIFIER = "DancingGoat.LandingPage.ProductCardWidget";


        private readonly ProductRepository repository;


        /// <summary>
        /// Creates an instance of <see cref="ProductCardWidgetViewComponent"/> class.
        /// </summary>
        /// <param name="repository">Repository for retrieving products.</param>
        public ProductCardWidgetViewComponent(ProductRepository repository)
        {
            this.repository = repository;
        }


        public ViewViewComponentResult Invoke(ProductCardProperties properties)
        {
            var selectedPage = properties.SelectedProducts.FirstOrDefault();

            var product = (selectedPage != null) ? repository.GetProduct(selectedPage.NodeGuid) : null;

            return View("~/Components/Widgets/ProductCardWidget/_ProductCardWidget.cshtml", ProductCardViewModel.GetViewModel(product));
        }
    }
}