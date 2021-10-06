using DancingGoat;
using DancingGoat.Sections;
using DancingGoat.PageTemplates;
using DancingGoat.Widgets;

using Kentico.PageBuilder.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;

// Widgets
[assembly: RegisterWidget(ComponentIdentifiers.TESTIMONIAL_WIDGET, "Testimonial", typeof(TestimonialWidgetProperties), "~/Components/Widgets/TestimonialWidget/_DancingGoat_LandingPage_TestimonialWidget.cshtml", Description = "Displays a quotation with its author.", IconClass = "icon-right-double-quotation-mark")]
[assembly: RegisterWidget(ComponentIdentifiers.CTA_BUTTON_WIDGET, "CTA button", typeof(CTAButtonWidgetProperties), "~/Components/Widgets/CTAButton/_DancingGoat_General_CTAButtonWidget.cshtml", Description = "Call to action button with configurable target page.", IconClass = "icon-rectangle-a")]

// Sections
[assembly: RegisterSection(ComponentIdentifiers.SINGLE_COLUMN_SECTION, "Single column", typeof(ThemeSectionProperties), "~/Components/Sections/_DancingGoat_SingleColumnSection.cshtml", Description = "Single-column section with one zone.", IconClass = "icon-square")]
[assembly: RegisterSection(ComponentIdentifiers.TWO_COLUMN_SECTION, "Two columns", typeof(ThemeSectionProperties), "~/Components/Sections/_DancingGoat_TwoColumnSection.cshtml", Description = "Two-column section with two zones.", IconClass = "icon-l-cols-2")]
[assembly: RegisterSection(ComponentIdentifiers.THREE_COLUMN_SECTION, "Three columns", typeof(ThreeColumnSectionProperties), "~/Components/Sections/ThreeColumnSection/_DancingGoat_ThreeColumnSection.cshtml", Description = "Three-column section with three zones.", IconClass = "icon-l-cols-3")]

// Page templates
[assembly: RegisterPageTemplate(ComponentIdentifiers.LANDING_PAGE_SINGLE_COLUMN_TEMPLATE, "Single column landing page", typeof(LandingPageSingleColumnProperties), "~/PageTemplates/LandingPage/_DancingGoat_LandingPageSingleColumn.cshtml", Description = "A default single column page template with two sections differentiated by a background color.")]