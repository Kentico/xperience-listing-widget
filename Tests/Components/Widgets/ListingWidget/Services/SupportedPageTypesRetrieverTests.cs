using System;
using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine.Types.DancingGoatCore;
using CMS.Tests;

using NSubstitute;

using NUnit.Framework;

namespace DancingGoat.Widgets
{
    [TestFixture]
    [Category.Unit]
    public class SupportedPageTypesRetrieverTests
    {
        private SupportedPageTypesRetriever pageTypesRetriever;


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

            pageTypesRetriever = new SupportedPageTypesRetriever(new List<ITransformationService> { articlesTransformationService, coffeesTransformationService, cafesTransformationService });
        }


        [Test]
        public void GetSupportedPagetypes_ReturnsAllPageTypes()
        {
            var pageTypes = pageTypesRetriever.GetSupportedPageTypes();

            Assert.That(pageTypes.Count(), Is.EqualTo(3));
            Assert.That(pageTypes.Contains(Article.CLASS_NAME), Is.True);
            Assert.That(pageTypes.Contains(Cafe.CLASS_NAME), Is.True);
            Assert.That(pageTypes.Contains(Coffee.CLASS_NAME), Is.True);
        }
    }
}
