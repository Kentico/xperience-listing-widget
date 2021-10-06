using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;

namespace DancingGoat.PageTemplates
{
    public class LandingPageSingleColumnProperties : IPageTemplateProperties
    {
        /// <summary>
        /// Indicates if logo should be shown.
        /// </summary>
        [EditingComponent(CheckBoxComponent.IDENTIFIER, Label = "Display logo", Order = 1)]
        public bool ShowLogo { get; set; } = true;


        /// <summary>
        /// Background color CSS class of the header.
        /// </summary>
        [EditingComponent(DropDownComponent.IDENTIFIER, Label = "Background color of header", Order = 2)]
        [EditingComponentProperty(nameof(DropDownProperties.DataSource), "first-color;Chocolate\r\nsecond-color;Gold\r\nthird-color;Espresso")]
        public string HeaderColorCssClass { get; set; } = "first-color";
    }
}