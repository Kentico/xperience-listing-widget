using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace DancingGoat.Sections
{
    /// <summary>
    /// Section properties to define the theme.
    /// </summary>
    public class ThemeSectionProperties : ISectionProperties
    {
        /// <summary>
        /// Theme of the section.
        /// </summary>
        [EditingComponent(DropDownComponent.IDENTIFIER, Label = "Color scheme", Order = 1)]
        [EditingComponentProperty(nameof(DropDownProperties.DataSource), ";None\r\nsection-white;Flat white\r\nsection-cappuccino;Cappuccino")]
        public string Theme { get; set; }
    }
}