using System.Collections.Generic;

using CMS.Tests;

using Kentico.Xperience.ListingWidget.Transformations;

using NSubstitute;

using NUnit.Framework;

namespace Kentico.Xperience.ListingWidget.Services.Tests
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
            articlesService.PageType.Returns(TransformationsMock.ARTICLE_CLASS_NAME);
            articlesService.Transformations.Returns(new List<Transformation> { TransformationsMock.Articles, TransformationsMock.ArticlesWithHeading });

            cafesService = Substitute.For<ITransformationService>();
            cafesService.PageType.Returns(TransformationsMock.CAFE_CLASS_NAME);
            cafesService.Transformations.Returns(new List<Transformation> { TransformationsMock.Cafes });

            coffeesService = Substitute.For<ITransformationService>();
            coffeesService.PageType.Returns(TransformationsMock.COFFEE_CLASS_NAME);
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
