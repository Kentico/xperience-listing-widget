using System.Collections.Generic;

using CMS.Tests;

using Kentico.Xperience.ListingWidget.Tests;

using NSubstitute;

using NUnit.Framework;

namespace Kentico.Xperience.ListingWidget.Widgets.Tests
{
    [TestFixture]
    [Category.Unit]
    public class TransformationStrategyTests
    {
        private TransformationStrategy transformationStrategy;
        private ITransformationService articlesService;
        private ITransformationService cafesService;
        private ITransformationService coffeesService;


        [SetUp]
        public void SetUp()
        {
            articlesService = Substitute.For<ITransformationService>();
            articlesService.PageType.Returns(FakeTransformations.ARTICLE_CLASS_NAME);
            articlesService.Transformations.Returns(new List<Transformation> { FakeTransformations.Articles, FakeTransformations.ArticlesWithHeading });

            cafesService = Substitute.For<ITransformationService>();
            cafesService.PageType.Returns(FakeTransformations.CAFE_CLASS_NAME);
            cafesService.Transformations.Returns(new List<Transformation> { FakeTransformations.Cafes });

            coffeesService = Substitute.For<ITransformationService>();
            coffeesService.PageType.Returns(FakeTransformations.COFFEE_CLASS_NAME);
            coffeesService.Transformations.Returns(new List<Transformation> { FakeTransformations.Coffees });

            transformationStrategy = new TransformationStrategy(new List<ITransformationService> { articlesService, cafesService, coffeesService });
        }


        [Test]
        public void GetService_ArticlesTransformation_ReturnsArticlesTransformationService()
        {
            var service = transformationStrategy.GetService(FakeTransformations.Articles.View);
            Assert.That(service, Is.EqualTo(articlesService));
        }


        [Test]
        public void GetService_ArticlesWithHeadingTransformation_ReturnsArticlesTransformationService()
        {
            var service = transformationStrategy.GetService(FakeTransformations.ArticlesWithHeading.View);
            Assert.That(service, Is.EqualTo(articlesService));
        }


        [Test]
        public void GetService_CafesTransformation_ReturnsCafesTransformationService()
        {
            var service = transformationStrategy.GetService(FakeTransformations.Cafes.View);
            Assert.That(service, Is.EqualTo(cafesService));
        }


        [Test]
        public void GetService_CoffeesTransformation_ReturnsCoffeesTransformationService()
        {
            var service = transformationStrategy.GetService(FakeTransformations.Coffees.View);
            Assert.That(service, Is.EqualTo(coffeesService));
        }
    }
}
