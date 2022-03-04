using System;
using System.Collections.Generic;
using System.Linq;

using CMS.Tests;

using Kentico.Xperience.ListingWidget.Transformations;

using NSubstitute;

using NUnit.Framework;

namespace Kentico.Xperience.ListingWidget.Services.Tests
{
    [TestFixture]
    [Category.Unit]
    public class ListingWidgetPageTypesRetrieverTests
    {
        private ListingWidgetPageTypesRetriever pageTypesRetriever;


        [SetUp]
        public void SetUp()
        {
            var articlesTransformationService = Substitute.For<ITransformationService>();
            articlesTransformationService.PageType.Returns(TransformationsMock.ARTICLE_CLASS_NAME);
            articlesTransformationService.Transformations.Returns(new List<Transformation> { TransformationsMock.Articles, TransformationsMock.ArticlesWithHeading });

            var coffeesTransformationService = Substitute.For<ITransformationService>();
            coffeesTransformationService.PageType.Returns(TransformationsMock.COFFEE_CLASS_NAME);
            coffeesTransformationService.Transformations.Returns(new List<Transformation> { TransformationsMock.Coffees });

            var cafesTransformationService = Substitute.For<ITransformationService>();
            cafesTransformationService.PageType.Returns(TransformationsMock.CAFE_CLASS_NAME);
            cafesTransformationService.Transformations.Returns(new List<Transformation> { TransformationsMock.Cafes });

            pageTypesRetriever = new ListingWidgetPageTypesRetriever(new List<ITransformationService> { articlesTransformationService, coffeesTransformationService, cafesTransformationService });
        }


        [Test]
        public void Retrieve_ReturnsAllPageTypes()
        {
            var pageTypes = pageTypesRetriever.Retrieve();

            Assert.That(pageTypes.Count(), Is.EqualTo(3));
            Assert.That(pageTypes.Contains(TransformationsMock.ARTICLE_CLASS_NAME), Is.True);
            Assert.That(pageTypes.Contains(TransformationsMock.CAFE_CLASS_NAME), Is.True);
            Assert.That(pageTypes.Contains(TransformationsMock.COFFEE_CLASS_NAME), Is.True);
        }
    }
}
