using System.Collections.Generic;

using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Product card widget properties.
    /// </summary>
    public class ProductCardProperties : IWidgetProperties
    {
        /// <summary>
        /// Selected product.
        /// </summary>
        [EditingComponent(PageSelector.IDENTIFIER, Label = "Selected product", Order = 1)]
        [EditingComponentProperty(nameof(PageSelectorProperties.RootPath), ContentItemIdentifiers.STORE)]
        public IEnumerable<PageSelectorItem> SelectedProducts { get; set; } = new List<PageSelectorItem>();
    }
}