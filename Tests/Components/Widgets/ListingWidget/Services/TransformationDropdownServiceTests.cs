using System;
using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine.Types.DancingGoatCore;
using CMS.Tests;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

using NSubstitute;

using NUnit.Framework;


namespace DancingGoat.Widgets
{
    [TestFixture]
    [Category.Unit]
    public class TransformationDropDownServiceTests
    {
        private SupportedTransformations supportedTransformations;
        private TransformationDropDownService service;


        [SetUp]
        public void SetUp()
        {
            supportedTransformations = new SupportedTransformations();
            var localizer = Substitute.For<IStringLocalizer<SharedResources>>();
            localizer[supportedTransformations.Articles.Name].Returns(new LocalizedString(supportedTransformations.Articles.Name, supportedTransformations.Articles.Name));
            localizer[supportedTransformations.ArticlesWithHeading.Name].Returns(new LocalizedString(supportedTransformations.ArticlesWithHeading.Name, supportedTransformations.ArticlesWithHeading.Name));
            localizer[supportedTransformations.Cafes.Name].Returns(new LocalizedString(supportedTransformations.Cafes.Name, supportedTransformations.Cafes.Name));
            localizer[supportedTransformations.Coffees.Name].Returns(new LocalizedString(supportedTransformations.Coffees.Name, supportedTransformations.Coffees.Name));
            localizer[supportedTransformations.Articles.ToolTip].Returns(new LocalizedString(supportedTransformations.Articles.ToolTip, supportedTransformations.Articles.ToolTip));
            localizer[supportedTransformations.ArticlesWithHeading.ToolTip].Returns(new LocalizedString(supportedTransformations.ArticlesWithHeading.ToolTip, supportedTransformations.ArticlesWithHeading.ToolTip));
            localizer[supportedTransformations.Cafes.ToolTip].Returns(new LocalizedString(supportedTransformations.Cafes.ToolTip, supportedTransformations.Cafes.ToolTip));
            localizer[supportedTransformations.Coffees.ToolTip].Returns(new LocalizedString(supportedTransformations.Coffees.ToolTip, supportedTransformations.Coffees.ToolTip));

            service = new TransformationDropDownService(localizer, supportedTransformations);
        }


        [Test]
        public void GetDrowDownModel_ArticlesWithNoSelectedOption_ReturnsArticleModelWithPagesAndTooltip()
        {
            var expectedTooltip = $"{supportedTransformations.Articles.Name}:\n{supportedTransformations.Articles.ToolTip}\n\n{supportedTransformations.ArticlesWithHeading.Name}:\n{supportedTransformations.ArticlesWithHeading.ToolTip}";
            var articleOption = new SelectListItem(supportedTransformations.Articles.Name, supportedTransformations.Articles.View);
            var articleWithHeadingOption = new SelectListItem(supportedTransformations.ArticlesWithHeading.Name, supportedTransformations.ArticlesWithHeading.View);

            var articlesModel = service.GetDropDownModel(null, Article.CLASS_NAME);

            Assert.Multiple(() =>
            {
                Assert.That(articlesModel.Options.Count(), Is.EqualTo(2));
                Assert.That(articlesModel.Options.Contains(articleOption, new SelectListItemComparer()), Is.True);
                Assert.That(articlesModel.Options.Contains(articleWithHeadingOption, new SelectListItemComparer()), Is.True);
                Assert.That(articlesModel.Tooltip, Is.EqualTo(expectedTooltip));
            });
        }


        [Test]
        public void GetDrowDownModel_ArticlesWithSelectedOption_ReturnsArticlesModelWithPagesAndTooltip()
        {
            var expectedTooltip = $"{supportedTransformations.Articles.Name}:\n{supportedTransformations.Articles.ToolTip}\n\n{supportedTransformations.ArticlesWithHeading.Name}:\n{supportedTransformations.ArticlesWithHeading.ToolTip}";
            var articleOption = new SelectListItem(supportedTransformations.Articles.Name, supportedTransformations.Articles.View);
            var articleWithHeadingOption = new SelectListItem(supportedTransformations.ArticlesWithHeading.Name, supportedTransformations.ArticlesWithHeading.View);

            var articlesModel = service.GetDropDownModel(supportedTransformations.Articles.View, Article.CLASS_NAME);

            Assert.Multiple(() =>
            {
                Assert.That(articlesModel.Options.Count(), Is.EqualTo(2));
                Assert.That(articlesModel.Options.Contains(articleOption, new SelectListItemComparer()), Is.True);
                Assert.That(articlesModel.Options.Contains(articleWithHeadingOption, new SelectListItemComparer()), Is.True);
                Assert.That(articlesModel.Tooltip, Is.EqualTo(expectedTooltip));
                Assert.That(articlesModel.SelectedOption, Is.EqualTo(supportedTransformations.Articles.View));
            });
        }


        [Test]
        public void GetDrowDownModel_CafesWithNoSelectedOption_ReturnsCafesModelWithPagesAndTooltip()
        {
            var expectedTooltip = $"{supportedTransformations.Cafes.Name}:\n{supportedTransformations.Cafes.ToolTip}";
            var cafeOption = new SelectListItem(supportedTransformations.Cafes.Name, supportedTransformations.Cafes.View);

            var cafesModel = service.GetDropDownModel(null, Cafe.CLASS_NAME);

            Assert.Multiple(() =>
            {
                Assert.That(cafesModel.Options.Count(), Is.EqualTo(1));
                Assert.That(cafesModel.Options.Contains(cafeOption, new SelectListItemComparer()), Is.True);
                Assert.That(cafesModel.Tooltip, Is.EqualTo(expectedTooltip));
            });
        }


        [Test]
        public void GetDrowDownModel_CafesWithSelectedOption_ReturnsCafesModelWithPagesAndTooltip()
        {
            var expectedTooltip = $"{supportedTransformations.Cafes.Name}:\n{supportedTransformations.Cafes.ToolTip}";
            var cafeOption = new SelectListItem(supportedTransformations.Cafes.Name, supportedTransformations.Cafes.View);

            var cafesModel = service.GetDropDownModel(supportedTransformations.Cafes.View, Cafe.CLASS_NAME);

            Assert.Multiple(() =>
            {
                Assert.That(cafesModel.Options.Count(), Is.EqualTo(1));
                Assert.That(cafesModel.Options.Contains(cafeOption, new SelectListItemComparer()), Is.True);
                Assert.That(cafesModel.Tooltip, Is.EqualTo(expectedTooltip));
                Assert.That(cafesModel.SelectedOption, Is.EqualTo(supportedTransformations.Cafes.View));
            });
        }


        [Test]
        public void GetDrowDownModel_CoffeesWithNoSelectedOption_ReturnsCoffeesModelWithPagesAndTooltip()
        {
            var expectedTooltip = $"{supportedTransformations.Coffees.Name}:\n{supportedTransformations.Coffees.ToolTip}";
            var coffeeOption = new SelectListItem(supportedTransformations.Coffees.Name, supportedTransformations.Coffees.View);

            var coffeesModel = service.GetDropDownModel(null, Coffee.CLASS_NAME);

            Assert.Multiple(() =>
            {
                Assert.That(coffeesModel.Options.Count(), Is.EqualTo(1));
                Assert.That(coffeesModel.Options.Contains(coffeeOption, new SelectListItemComparer()), Is.True);
                Assert.That(coffeesModel.Tooltip, Is.EqualTo(expectedTooltip));
            });
        }


        [Test]
        public void GetDrowDownModel_CoffeesWithSelectedOption_ReturnsCoffeesModelWithPagesAndTooltip()
        {
            var expectedTooltip = $"{supportedTransformations.Coffees.Name}:\n{supportedTransformations.Coffees.ToolTip}";
            var coffeeOption = new SelectListItem(supportedTransformations.Coffees.Name, supportedTransformations.Coffees.View);

            var coffeesModel = service.GetDropDownModel(supportedTransformations.Coffees.View, Coffee.CLASS_NAME);

            Assert.Multiple(() =>
            {
                Assert.That(coffeesModel.Options.Count(), Is.EqualTo(1));
                Assert.That(coffeesModel.Options.Contains(coffeeOption, new SelectListItemComparer()), Is.True);
                Assert.That(coffeesModel.Tooltip, Is.EqualTo(expectedTooltip));
                Assert.That(coffeesModel.SelectedOption, Is.EqualTo(supportedTransformations.Coffees.View));
            });
        }


        public class SelectListItemComparer : IEqualityComparer<SelectListItem>
        {
            public bool Equals(SelectListItem s1, SelectListItem s2)
            {
                if (s1 == null && s2 == null)
                    return true;
                else if (s1 == null || s2 == null)
                    return false;
                else if (s1.Value == s2.Value && s1.Text == s2.Text)
                    return true;
                else
                    return false;
            }

            public int GetHashCode(SelectListItem item)
            {
                return HashCode.Combine(item.Text, item.Value);
            }
        }
    }
}
