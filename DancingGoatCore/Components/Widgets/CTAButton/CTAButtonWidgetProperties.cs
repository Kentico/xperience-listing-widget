using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// CTA button widget properties.
    /// </summary>
    public class CTAButtonWidgetProperties : IWidgetProperties
    {
        /// <summary>
        /// Button text.
        /// </summary>
        public string Text { get; set; }


        /// <summary>
        /// Page where the button points to.
        /// </summary>
        [EditingComponent(UrlSelector.IDENTIFIER, Order = 1, Label = "Link URL")]
        [EditingComponentProperty(nameof(UrlSelectorProperties.Placeholder), "Please enter a URL or select a page...")]
        [EditingComponentProperty(nameof(UrlSelectorProperties.Tabs), ContentSelectorTabs.Page)]
        public string LinkUrl { get; set; }


        /// <summary>
        /// Indicates if link should be opened in a new tab.
        /// </summary>
        [EditingComponent(CheckBoxComponent.IDENTIFIER, Order = 2, Label = "Open in a new tab")]
        public bool OpenInNewTab { get; set; }
    }
}
