using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Cafe card widget properties.
    /// </summary>
    public class CafeCardProperties : IWidgetProperties
    {
        /// <summary>
        /// Selected cafes.
        /// </summary>
        [EditingComponent(PageSelector.IDENTIFIER, Label = "Cafe", Order = 1)]
        [EditingComponentProperty(nameof(PageSelectorProperties.RootPath), ContentItemIdentifiers.CAFES)]
        [Required]
        public IEnumerable<PageSelectorItem> SelectedCafes { get; set; } = new List<PageSelectorItem>();
    }
}