using System.Collections.Generic;
using System.Linq;

using CMS.Tests;

using NSubstitute;

using NUnit.Framework;

namespace Kentico.Xperience.ListingWidget.Tests
{
    [TestFixture]
    [Category.Unit]
    public class ListingWidgetTransformationsRetrieverTests
    {
        private ListingWidgetTransformationsRetriever transformationsRetriever;


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

            transformationsRetriever = new ListingWidgetTransformationsRetriever(new List<ITransformationService> { articlesTransformationService, coffeesTransformationService, cafesTransformationService });
        }


        [Test]
        public void Retrieve_Article_ReturnsArticlesTransformations()
        {
            var transformations = transformationsRetriever.Retrieve(FakeTransformations.ARTICLE_CLASS_NAME);

            Assert.That(transformations.Count, Is.EqualTo(2));
            Assert.That(transformations.Contains(FakeTransformations.Articles), Is.True);
            Assert.That(transformations.Contains(FakeTransformations.ArticlesWithHeading), Is.True);
        }


        [Test]
        public void Retrieve_Cafe_ReturnsCafesTransformations()
        {
            var transformations = transformationsRetriever.Retrieve(FakeTransformations.CAFE_CLASS_NAME);

            Assert.That(transformations.Count, Is.EqualTo(1));
            Assert.That(transformations.Contains(FakeTransformations.Cafes), Is.True);
        }


        [Test]
        public void Retrieve_Coffee_ReturnsCoffeesTransformations()
        {
            var transformations = transformationsRetriever.Retrieve(FakeTransformations.COFFEE_CLASS_NAME);

            Assert.That(transformations.Count, Is.EqualTo(1));
            Assert.That(transformations.Contains(FakeTransformations.Coffees), Is.True);
        }


        [Test]
        public void Retrieve_Brewer_ReturnsEmpty()
        {
            var transformations = transformationsRetriever.Retrieve(FakeTransformations.BREWER_CLASS_NAME);

            Assert.That(transformations, Is.Empty);
        }


        [Test]
        public void IsSupported_ArticlesTransformationAndArticlePageType_ReturnsTrue()
        {
            var supported = transformationsRetriever.IsSupported(FakeTransformations.Articles.View, FakeTransformations.ARTICLE_CLASS_NAME);

            Assert.That(supported, Is.True);
        }


        [Test]
        public void IsSupported_ArticlesWithHeadingTransformationAndArticlePageType_ReturnsTrue()
        {
            var supported = transformationsRetriever.IsSupported(FakeTransformations.ArticlesWithHeading.View, FakeTransformations.ARTICLE_CLASS_NAME);

            Assert.That(supported, Is.True);
        }


        [Test]
        public void IsSupported_CafesTransformationAndCafePageType_ReturnsTrue()
        {
            var supported = transformationsRetriever.IsSupported(FakeTransformations.Cafes.View, FakeTransformations.CAFE_CLASS_NAME);

            Assert.That(supported, Is.True);
        }


        [Test]
        public void IsSupported_CoffeesTransformationAndCoffeePageType_ReturnsTrue()
        {
            var supported = transformationsRetriever.IsSupported(FakeTransformations.Coffees.View, FakeTransformations.COFFEE_CLASS_NAME);

            Assert.That(supported, Is.True);
        }


        [Test]
        public void IsSupported_SupportedTransformationAndWrongPageType_ReturnsFalse()
        {
            var supportedForArticle = transformationsRetriever.IsSupported(FakeTransformations.Coffees.View, FakeTransformations.ARTICLE_CLASS_NAME);
            var supportedForCafe = transformationsRetriever.IsSupported(FakeTransformations.Coffees.View, FakeTransformations.CAFE_CLASS_NAME);
            var supportedForBrewer = transformationsRetriever.IsSupported(FakeTransformations.Coffees.View, FakeTransformations.BREWER_CLASS_NAME);

            Assert.Multiple(() =>
            {
                Assert.That(supportedForArticle, Is.False);
                Assert.That(supportedForCafe, Is.False);
                Assert.That(supportedForBrewer, Is.False);
            });
        }


        [Test]
        public void IsSupported_NotSupportedTransformation_ReturnsFalse()
        {
            var supported = transformationsRetriever.IsSupported("~Brewers/_default.cshtml", FakeTransformations.BREWER_CLASS_NAME);

            Assert.That(supported, Is.False);
        }


        [Test]
        public void IsSupported_EmptyTransformationParameter_ReturnsFalse()
        {
            var supported = transformationsRetriever.IsSupported("", FakeTransformations.ARTICLE_CLASS_NAME);

            Assert.That(supported, Is.False);
        }


        [Test]
        public void IsSupported_EmptyPageTypeParameter_ReturnsFalse()
        {
            var supported = transformationsRetriever.IsSupported(FakeTransformations.Articles.View, "");

            Assert.That(supported, Is.False);
        }
    }
}
