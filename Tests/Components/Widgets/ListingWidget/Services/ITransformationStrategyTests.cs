using System.Collections.Generic;

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
        private SupportedTransformations supportedTransformations;


        [SetUp]
        public void SetUp()
        {
            supportedTransformations = new SupportedTransformations();
            articlesService = Substitute.For<ArticlesTransformationService>(Substitute.For<IPageUrlRetriever>(), Substitute.For<IPageAttachmentUrlRetriever>());
            cafesService = Substitute.For<CafesTransformationService>(Substitute.For<IPageAttachmentUrlRetriever>(), Substitute.For<IStringLocalizer<SharedResources>>(), Substitute.For<ICountryRepository>());
            coffeesService = Substitute.For<CoffeesTransformationService>(Substitute.For<IPageUrlRetriever>());
            transformationStrategy = new TransformationStrategy(new List<ITransformationService> { articlesService, cafesService, coffeesService }, supportedTransformations);

        }


        [Test]
        public void GetService_ArticlesTransformation_ReturnsArticlesTransformationService()
        {
            var service = transformationStrategy.GetService(supportedTransformations.Articles.View);
            Assert.That(service, Is.EqualTo(articlesService));
        }


        [Test]
        public void GetService_ArticlesWithHeadingTransformation_ReturnsArticlesTransformationService()
        {
            var service = transformationStrategy.GetService(supportedTransformations.ArticlesWithHeading.View);
            Assert.That(service, Is.EqualTo(articlesService));
        }


        [Test]
        public void GetService_CafesTransformation_ReturnsCafesTransformationService()
        {
            var service = transformationStrategy.GetService(supportedTransformations.Cafes.View);
            Assert.That(service, Is.EqualTo(cafesService));
        }


        [Test]
        public void GetService_CoffeesTransformation_ReturnsCoffeesTransformationService()
        {
            var service = transformationStrategy.GetService(supportedTransformations.Coffees.View);
            Assert.That(service, Is.EqualTo(coffeesService));
        }
    }
}
