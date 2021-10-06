using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;

namespace DancingGoat.PageTemplates
{
    public class ArticleWithSideBarProperties : IPageTemplateProperties
    {
        [EditingComponent(DropDownComponent.IDENTIFIER, Label = "Sidebar location", Order = 1)]
        [EditingComponentProperty(nameof(DropDownProperties.DataSource), "Right;Right\r\nLeft;Left")]
        public string SidebarLocation { get; set; } = "Right";

        [EditingComponent(DropDownComponent.IDENTIFIER, Label = "Article width", Order = 2)]
        [EditingComponentProperty(nameof(DropDownProperties.DataSource), "3;25%\r\n6;50%\r\n9;75%")]
        public string ArticleWidth { get; set; } = "9";
    }
}