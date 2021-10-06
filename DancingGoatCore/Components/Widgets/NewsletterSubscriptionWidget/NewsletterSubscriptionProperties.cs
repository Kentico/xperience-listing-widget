using Kentico.PageBuilder.Web.Mvc;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Newsletter subscription widget properties.
    /// </summary>
    public class NewsletterSubscriptionProperties : IWidgetProperties
    {
        /// <summary>
        /// Newsletter code name.
        /// </summary>
        public string Newsletter { get; set; } = "Newsletter";
    }
}