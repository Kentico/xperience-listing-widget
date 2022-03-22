using System;
using System.Collections.Generic;
using System.Linq;

using CMS.Tests;

using NSubstitute;

using NUnit.Framework;

namespace Kentico.Xperience.ListingWidget.Tests
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
            articlesTransformationService.PageType.Returns(FakeTransformations.ARTICLE_CLASS_NAME);
            articlesTransformationService.Transformations.Returns(new List<Transformation> { FakeTransformations.Articles, FakeTransformations.ArticlesWithHeading });

            var coffeesTransformationService = Substitute.For<ITransformationService>();
            coffeesTransformationService.PageType.Returns(FakeTransformations.COFFEE_CLASS_NAME);
            coffeesTransformationService.Transformations.Returns(new List<Transformation> { FakeTransformations.Coffees });

            var cafesTransformationService = Substitute.For<ITransformationService>();
            cafesTransformationService.PageType.Returns(FakeTransformations.CAFE_CLASS_NAME);
            cafesTransformationService.Transformations.Returns(new List<Transformation> { FakeTransformations.Cafes });

            pageTypesRetriever = new ListingWidgetPageTypesRetriever(new List<ITransformationService> { articlesTransformationService, coffeesTransformationService, cafesTransformationService });
        }


        [Test]
        public void Retrieve_ReturnsAllPageTypes()
        {
            var pageTypes = pageTypesRetriever.Retrieve();

            Assert.That(pageTypes.Count(), Is.EqualTo(3));
            Assert.That(pageTypes.Contains(FakeTransformations.ARTICLE_CLASS_NAME), Is.True);
            Assert.That(pageTypes.Contains(FakeTransformations.CAFE_CLASS_NAME), Is.True);
            Assert.That(pageTypes.Contains(FakeTransformations.COFFEE_CLASS_NAME), Is.True);
        }
    }
}
