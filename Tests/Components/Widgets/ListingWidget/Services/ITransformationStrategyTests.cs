using System.Collections.Generic;

using CMS.DocumentEngine.Types.DancingGoatCore;
using CMS.Tests;

using DancingGoat.Models;
using DancingGoat.Widgets;

using Kentico.Content.Web.Mvc;

using Microsoft.Extensions.Localization;

using NSubstitute;

using NUnit.Framework;

using Kentico.Xperience.ListingWidget;
using Kentico.Xperience.ListingWidget.Widgets;

namespace DancingGoat.Widgets
{
    [TestFixture]
    [Category.Unit]
    public class ITransformationStrategyTests
    {
        private ITransformationStrategy transformationStrategy;
        private ITransformationService articlesService;
        private ITransformationService cafesService;
        private ITransformationService coffeesService;


        [SetUp]
        public void SetUp()
        {
            articlesService = Substitute.For<ITransformationService>();
            articlesService.PageType.Returns(Article.CLASS_NAME);
            articlesService.Transformations.Returns(new List<Transformation> { TransformationsMock.Articles, TransformationsMock.ArticlesWithHeading });

            cafesService = Substitute.For<ITransformationService>();
            cafesService.PageType.Returns(Cafe.CLASS_NAME);
            cafesService.Transformations.Returns(new List<Transformation> { TransformationsMock.Cafes });

            coffeesService = Substitute.For<ITransformationService>();
            coffeesService.PageType.Returns(Coffee.CLASS_NAME);
            coffeesService.Transformations.Returns(new List<Transformation> { TransformationsMock.Coffees });

            transformationStrategy = new TransformationStrategy(new List<ITransformationService> { articlesService, cafesService, coffeesService });
        }


        [Test]
        public void GetService_ArticlesTransformation_ReturnsArticlesTransformationService()
        {
            var service = transformationStrategy.GetService(TransformationsMock.Articles.View);
            Assert.That(service, Is.EqualTo(articlesService));
        }


        [Test]
        public void GetService_ArticlesWithHeadingTransformation_ReturnsArticlesTransformationService()
        {
            var service = transformationStrategy.GetService(TransformationsMock.ArticlesWithHeading.View);
            Assert.That(service, Is.EqualTo(articlesService));
        }


        [Test]
        public void GetService_CafesTransformation_ReturnsCafesTransformationService()
        {
            var service = transformationStrategy.GetService(TransformationsMock.Cafes.View);
            Assert.That(service, Is.EqualTo(cafesService));
        }


        [Test]
        public void GetService_CoffeesTransformation_ReturnsCoffeesTransformationService()
        {
            var service = transformationStrategy.GetService(TransformationsMock.Coffees.View);
            Assert.That(service, Is.EqualTo(coffeesService));
        }
    }
}
