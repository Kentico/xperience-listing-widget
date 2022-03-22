using System;
using System.Collections.Generic;
using System.Linq;

using CMS.Tests;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

using NSubstitute;

using NUnit.Framework;

namespace Kentico.Xperience.ListingWidget.Tests
{
    [TestFixture]
    [Category.Unit]
    public class TransformationEditorServiceTests
    {
        private TransformationEditorService service;


        [SetUp]
        public void SetUp()
        {
            var articlesTransformationService = Substitute.For<ITransformationService>();
            articlesTransformationService.PageType.Returns(FakeTransformations.ARTICLE_CLASS_NAME);
            articlesTransformationService.Transformations.Returns(new List<Transformation> { FakeTransformations.Articles, FakeTransformations.ArticlesWithHeading });

            var coffeesTransformationService = Substitute.For<ITransformationService>();
            coffeesTransformationService.PageType.Returns(FakeTransformations.COFFEE_CLASS_NAME);
            coffeesTransformationService.Transformations.Returns(new List<Transformation> { FakeTransformations.Coffees });

            var cafesTransformationService = Substitute.For<ITransformationService>();
            cafesTransformationService.PageType.Returns(FakeTransformations.CAFE_CLASS_NAME);
            cafesTransformationService.Transformations.Returns(new List<Transformation> { FakeTransformations.Cafes });

            var transformationsRetriever = new ListingWidgetTransformationsRetriever(new List<ITransformationService> { articlesTransformationService, coffeesTransformationService, cafesTransformationService });

            var localizer = Substitute.For<IStringLocalizer<ListingWidgetResources>>();
            localizer[FakeTransformations.Articles.Name].Returns(new LocalizedString(FakeTransformations.Articles.Name, FakeTransformations.Articles.Name));
            localizer[FakeTransformations.ArticlesWithHeading.Name].Returns(new LocalizedString(FakeTransformations.ArticlesWithHeading.Name, FakeTransformations.ArticlesWithHeading.Name));
            localizer[FakeTransformations.Cafes.Name].Returns(new LocalizedString(FakeTransformations.Cafes.Name, FakeTransformations.Cafes.Name));
            localizer[FakeTransformations.Coffees.Name].Returns(new LocalizedString(FakeTransformations.Coffees.Name, FakeTransformations.Coffees.Name));
            localizer[FakeTransformations.Articles.Description].Returns(new LocalizedString(FakeTransformations.Articles.Description, FakeTransformations.Articles.Description));
            localizer[FakeTransformations.ArticlesWithHeading.Description].Returns(new LocalizedString(FakeTransformations.ArticlesWithHeading.Description, FakeTransformations.ArticlesWithHeading.Description));
            localizer[FakeTransformations.Cafes.Description].Returns(new LocalizedString(FakeTransformations.Cafes.Description, FakeTransformations.Cafes.Description));
            localizer[FakeTransformations.Coffees.Description].Returns(new LocalizedString(FakeTransformations.Coffees.Description, FakeTransformations.Coffees.Description));

            service = new TransformationEditorService(localizer, transformationsRetriever);
        }


        [Test]
        public void GetEditorModel_ArticlesWithNoSelectedOption_ReturnsArticleModelWithPagesAndTooltip()
        {
            var expectedTooltip = $"{FakeTransformations.Articles.Name}:\n{FakeTransformations.Articles.Description}\n\n{FakeTransformations.ArticlesWithHeading.Name}:\n{FakeTransformations.ArticlesWithHeading.Description}";
            var articleOption = new SelectListItem(FakeTransformations.Articles.Name, FakeTransformations.Articles.View);
            var articleWithHeadingOption = new SelectListItem(FakeTransformations.ArticlesWithHeading.Name, FakeTransformations.ArticlesWithHeading.View);

            var articlesModel = service.GetEditorModel(null, FakeTransformations.ARTICLE_CLASS_NAME);

            Assert.Multiple(() =>
            {
                Assert.That(articlesModel.Options.Count(), Is.EqualTo(2));
                Assert.That(articlesModel.Options.Contains(articleOption, new SelectListItemComparer()), Is.True);
                Assert.That(articlesModel.Options.Contains(articleWithHeadingOption, new SelectListItemComparer()), Is.True);
                Assert.That(articlesModel.Tooltip, Is.EqualTo(expectedTooltip));
            });
        }


        [Test]
        public void GetEditorModel_ArticlesWithSelectedOption_ReturnsArticlesModelWithPagesAndTooltip()
        {
            var expectedTooltip = $"{FakeTransformations.Articles.Name}:\n{FakeTransformations.Articles.Description}\n\n{FakeTransformations.ArticlesWithHeading.Name}:\n{FakeTransformations.ArticlesWithHeading.Description}";
            var articleOption = new SelectListItem(FakeTransformations.Articles.Name, FakeTransformations.Articles.View);
            var articleWithHeadingOption = new SelectListItem(FakeTransformations.ArticlesWithHeading.Name, FakeTransformations.ArticlesWithHeading.View);

            var articlesModel = service.GetEditorModel(FakeTransformations.Articles.View, FakeTransformations.ARTICLE_CLASS_NAME);

            Assert.Multiple(() =>
            {
                Assert.That(articlesModel.Options.Count(), Is.EqualTo(2));
                Assert.That(articlesModel.Options.Contains(articleOption, new SelectListItemComparer()), Is.True);
                Assert.That(articlesModel.Options.Contains(articleWithHeadingOption, new SelectListItemComparer()), Is.True);
                Assert.That(articlesModel.Tooltip, Is.EqualTo(expectedTooltip));
                Assert.That(articlesModel.SelectedOption, Is.EqualTo(FakeTransformations.Articles.View));
            });
        }


        [Test]
        public void GetEditorModel_CafesWithNoSelectedOption_ReturnsCafesModelWithPagesAndTooltip()
        {
            var expectedTooltip = $"{FakeTransformations.Cafes.Name}:\n{FakeTransformations.Cafes.Description}";
            var cafeOption = new SelectListItem(FakeTransformations.Cafes.Name, FakeTransformations.Cafes.View);

            var cafesModel = service.GetEditorModel(null, FakeTransformations.CAFE_CLASS_NAME);

            Assert.Multiple(() =>
            {
                Assert.That(cafesModel.Options.Count(), Is.EqualTo(1));
                Assert.That(cafesModel.Options.Contains(cafeOption, new SelectListItemComparer()), Is.True);
                Assert.That(cafesModel.Tooltip, Is.EqualTo(expectedTooltip));
            });
        }


        [Test]
        public void GetEditorModel_CafesWithSelectedOption_ReturnsCafesModelWithPagesAndTooltip()
        {
            var expectedTooltip = $"{FakeTransformations.Cafes.Name}:\n{FakeTransformations.Cafes.Description}";
            var cafeOption = new SelectListItem(FakeTransformations.Cafes.Name, FakeTransformations.Cafes.View);

            var cafesModel = service.GetEditorModel(FakeTransformations.Cafes.View, FakeTransformations.CAFE_CLASS_NAME);

            Assert.Multiple(() =>
            {
                Assert.That(cafesModel.Options.Count(), Is.EqualTo(1));
                Assert.That(cafesModel.Options.Contains(cafeOption, new SelectListItemComparer()), Is.True);
                Assert.That(cafesModel.Tooltip, Is.EqualTo(expectedTooltip));
                Assert.That(cafesModel.SelectedOption, Is.EqualTo(FakeTransformations.Cafes.View));
            });
        }


        [Test]
        public void GetEditorModel_CoffeesWithNoSelectedOption_ReturnsCoffeesModelWithPagesAndTooltip()
        {
            var expectedTooltip = $"{FakeTransformations.Coffees.Name}:\n{FakeTransformations.Coffees.Description}";
            var coffeeOption = new SelectListItem(FakeTransformations.Coffees.Name, FakeTransformations.Coffees.View);

            var coffeesModel = service.GetEditorModel(null, FakeTransformations.COFFEE_CLASS_NAME);

            Assert.Multiple(() =>
            {
                Assert.That(coffeesModel.Options.Count(), Is.EqualTo(1));
                Assert.That(coffeesModel.Options.Contains(coffeeOption, new SelectListItemComparer()), Is.True);
                Assert.That(coffeesModel.Tooltip, Is.EqualTo(expectedTooltip));
            });
        }


        [Test]
        public void GetEditorModel_CoffeesWithSelectedOption_ReturnsCoffeesModelWithPagesAndTooltip()
        {
            var expectedTooltip = $"{FakeTransformations.Coffees.Name}:\n{FakeTransformations.Coffees.Description}";
            var coffeeOption = new SelectListItem(FakeTransformations.Coffees.Name, FakeTransformations.Coffees.View);

            var coffeesModel = service.GetEditorModel(FakeTransformations.Coffees.View, FakeTransformations.COFFEE_CLASS_NAME);

            Assert.Multiple(() =>
            {
                Assert.That(coffeesModel.Options.Count(), Is.EqualTo(1));
                Assert.That(coffeesModel.Options.Contains(coffeeOption, new SelectListItemComparer()), Is.True);
                Assert.That(coffeesModel.Tooltip, Is.EqualTo(expectedTooltip));
                Assert.That(coffeesModel.SelectedOption, Is.EqualTo(FakeTransformations.Coffees.View));
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
