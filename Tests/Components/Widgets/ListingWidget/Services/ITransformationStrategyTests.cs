using System.Collections.Generic;

using CMS.DocumentEngine.Types.DancingGoatCore;
using CMS.Tests;

using DancingGoat.Models;

using Kentico.Content.Web.Mvc;

using Microsoft.Extensions.Localization;

using NSubstitute;

using NUnit.Framework;

namespace DancingGoat.Widgets
{
    [TestFixture]
    [Category.Unit]
    public class ITransformationStrategyTests
    {
        private ITransformationStrategy transformationStrategy;
        private ArticlesTransformationService articlesService;
        private CafesTransformationService cafesService;
        private CoffeesTransformationService coffeesService;


        [SetUp]
        public void SetUp()
        {
            articlesService = Substitute.For<ArticlesTransformationService>(Substitute.For<IPageUrlRetriever>(), Substitute.For<IPageAttachmentUrlRetriever>());
            articlesService.PageType.Returns(Article.CLASS_NAME);
            articlesService.Transformations.Returns(new List<Transformation> { TransformationsMock.Articles, TransformationsMock.ArticlesWithHeading });

            cafesService = Substitute.For<CafesTransformationService>(Substitute.For<IPageAttachmentUrlRetriever>(), Substitute.For<IStringLocalizer<SharedResources>>(), Substitute.For<ICountryRepository>());
            cafesService.PageType.Returns(Cafe.CLASS_NAME);
            cafesService.Transformations.Returns(new List<Transformation> { TransformationsMock.Cafes });

            coffeesService = Substitute.For<CoffeesTransformationService>(Substitute.For<IPageUrlRetriever>());
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
