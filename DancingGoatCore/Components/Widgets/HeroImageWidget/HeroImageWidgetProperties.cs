using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Hero image widget properties.
    /// </summary>
    public class HeroImageWidgetProperties : IWidgetProperties
    {
        /// <summary>
        /// Media library name.
        /// </summary>
        public const string MEDIA_LIBRARY_NAME = "Graphics";


        /// <summary>
        /// Guid of background image.
        /// </summary>
        [EditingComponent(MediaFilesSelector.IDENTIFIER, Label = "Background image", Order = 1)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.LibraryName), MEDIA_LIBRARY_NAME)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.MaxFilesLimit), 1)]
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.AllowedExtensions), ".gif;.png;.jpg;.jpeg")]
        [Required]
        public IEnumerable<MediaFilesSelectorItem> Image { get; set; } = new List<MediaFilesSelectorItem>();


        /// <summary>
        /// Text to be displayed.
        /// </summary>
        public string Text { get; set; }


        /// <summary>
        /// Button text.
        /// </summary>
        public string ButtonText { get; set; }


        /// <summary>
        /// Target of button link.
        /// </summary>
        [EditingComponent(TextInputComponent.IDENTIFIER, Label = "Button target", Order = 2)]
        public string ButtonTarget { get; set; }


        /// <summary>
        /// Theme of the widget.
        /// </summary>
        [EditingComponent(DropDownComponent.IDENTIFIER, Label = "Color scheme", Order = 3)]
        [EditingComponentProperty(nameof(DropDownProperties.DataSource), "light;Light\r\ndark;Dark")]
        public string Theme { get; set; } = "dark";
    }
}