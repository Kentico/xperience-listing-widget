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
    public class ITransformationEditorServiceTests
    {
        private ITransformationEditorService service;


        [SetUp]
        public void SetUp()
        {
            var articlesTransformationService = Substitute.For<ITransformationService>();
            articlesTransformationService.PageType.Returns(Article.CLASS_NAME);
            articlesTransformationService.Transformations.Returns(new List<Transformation> { TransformationsMock.Articles, TransformationsMock.ArticlesWithHeading });

            var coffeesTransformationService = Substitute.For<ITransformationService>();
            coffeesTransformationService.PageType.Returns(Coffee.CLASS_NAME);
            coffeesTransformationService.Transformations.Returns(new List<Transformation> { TransformationsMock.Coffees });

            var cafesTransformationService = Substitute.For<ITransformationService>();
            cafesTransformationService.PageType.Returns(Cafe.CLASS_NAME);
            cafesTransformationService.Transformations.Returns(new List<Transformation> { TransformationsMock.Cafes });

            var transformationsRetriever = new ListingWidgetTransformationsRetriever(new List<ITransformationService> { articlesTransformationService, coffeesTransformationService, cafesTransformationService });

            var localizer = Substitute.For<IStringLocalizer<SharedResources>>();
            localizer[TransformationsMock.Articles.Name].Returns(new LocalizedString(TransformationsMock.Articles.Name, TransformationsMock.Articles.Name));
            localizer[TransformationsMock.ArticlesWithHeading.Name].Returns(new LocalizedString(TransformationsMock.ArticlesWithHeading.Name, TransformationsMock.ArticlesWithHeading.Name));
            localizer[TransformationsMock.Cafes.Name].Returns(new LocalizedString(TransformationsMock.Cafes.Name, TransformationsMock.Cafes.Name));
            localizer[TransformationsMock.Coffees.Name].Returns(new LocalizedString(TransformationsMock.Coffees.Name, TransformationsMock.Coffees.Name));
            localizer[TransformationsMock.Articles.Description].Returns(new LocalizedString(TransformationsMock.Articles.Description, TransformationsMock.Articles.Description));
            localizer[TransformationsMock.ArticlesWithHeading.Description].Returns(new LocalizedString(TransformationsMock.ArticlesWithHeading.Description, TransformationsMock.ArticlesWithHeading.Description));
            localizer[TransformationsMock.Cafes.Description].Returns(new LocalizedString(TransformationsMock.Cafes.Description, TransformationsMock.Cafes.Description));
            localizer[TransformationsMock.Coffees.Description].Returns(new LocalizedString(TransformationsMock.Coffees.Description, TransformationsMock.Coffees.Description));

            service = new TransformationEditorService(localizer, transformationsRetriever);
        }


        [Test]
        public void GetEditorModel_ArticlesWithNoSelectedOption_ReturnsArticleModelWithPagesAndTooltip()
        {
            var expectedTooltip = $"{TransformationsMock.Articles.Name}:\n{TransformationsMock.Articles.Description}\n\n{TransformationsMock.ArticlesWithHeading.Name}:\n{TransformationsMock.ArticlesWithHeading.Description}";
            var articleOption = new SelectListItem(TransformationsMock.Articles.Name, TransformationsMock.Articles.View);
            var articleWithHeadingOption = new SelectListItem(TransformationsMock.ArticlesWithHeading.Name, TransformationsMock.ArticlesWithHeading.View);

            var articlesModel = service.GetEditorModel(null, Article.CLASS_NAME);

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
            var expectedTooltip = $"{TransformationsMock.Articles.Name}:\n{TransformationsMock.Articles.Description}\n\n{TransformationsMock.ArticlesWithHeading.Name}:\n{TransformationsMock.ArticlesWithHeading.Description}";
            var articleOption = new SelectListItem(TransformationsMock.Articles.Name, TransformationsMock.Articles.View);
            var articleWithHeadingOption = new SelectListItem(TransformationsMock.ArticlesWithHeading.Name, TransformationsMock.ArticlesWithHeading.View);

            var articlesModel = service.GetEditorModel(TransformationsMock.Articles.View, Article.CLASS_NAME);

            Assert.Multiple(() =>
            {
                Assert.That(articlesModel.Options.Count(), Is.EqualTo(2));
                Assert.That(articlesModel.Options.Contains(articleOption, new SelectListItemComparer()), Is.True);
                Assert.That(articlesModel.Options.Contains(articleWithHeadingOption, new SelectListItemComparer()), Is.True);
                Assert.That(articlesModel.Tooltip, Is.EqualTo(expectedTooltip));
                Assert.That(articlesModel.SelectedOption, Is.EqualTo(TransformationsMock.Articles.View));
            });
        }


        [Test]
        public void GetEditorModel_CafesWithNoSelectedOption_ReturnsCafesModelWithPagesAndTooltip()
        {
            var expectedTooltip = $"{TransformationsMock.Cafes.Name}:\n{TransformationsMock.Cafes.Description}";
            var cafeOption = new SelectListItem(TransformationsMock.Cafes.Name, TransformationsMock.Cafes.View);

            var cafesModel = service.GetEditorModel(null, Cafe.CLASS_NAME);

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
            var expectedTooltip = $"{TransformationsMock.Cafes.Name}:\n{TransformationsMock.Cafes.Description}";
            var cafeOption = new SelectListItem(TransformationsMock.Cafes.Name, TransformationsMock.Cafes.View);

            var cafesModel = service.GetEditorModel(TransformationsMock.Cafes.View, Cafe.CLASS_NAME);

            Assert.Multiple(() =>
            {
                Assert.That(cafesModel.Options.Count(), Is.EqualTo(1));
                Assert.That(cafesModel.Options.Contains(cafeOption, new SelectListItemComparer()), Is.True);
                Assert.That(cafesModel.Tooltip, Is.EqualTo(expectedTooltip));
                Assert.That(cafesModel.SelectedOption, Is.EqualTo(TransformationsMock.Cafes.View));
            });
        }


        [Test]
        public void GetEditorModel_CoffeesWithNoSelectedOption_ReturnsCoffeesModelWithPagesAndTooltip()
        {
            var expectedTooltip = $"{TransformationsMock.Coffees.Name}:\n{TransformationsMock.Coffees.Description}";
            var coffeeOption = new SelectListItem(TransformationsMock.Coffees.Name, TransformationsMock.Coffees.View);

            var coffeesModel = service.GetEditorModel(null, Coffee.CLASS_NAME);

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
            var expectedTooltip = $"{TransformationsMock.Coffees.Name}:\n{TransformationsMock.Coffees.Description}";
            var coffeeOption = new SelectListItem(TransformationsMock.Coffees.Name, TransformationsMock.Coffees.View);

            var coffeesModel = service.GetEditorModel(TransformationsMock.Coffees.View, Coffee.CLASS_NAME);

            Assert.Multiple(() =>
            {
                Assert.That(coffeesModel.Options.Count(), Is.EqualTo(1));
                Assert.That(coffeesModel.Options.Contains(coffeeOption, new SelectListItemComparer()), Is.True);
                Assert.That(coffeesModel.Tooltip, Is.EqualTo(expectedTooltip));
                Assert.That(coffeesModel.SelectedOption, Is.EqualTo(TransformationsMock.Coffees.View));
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
