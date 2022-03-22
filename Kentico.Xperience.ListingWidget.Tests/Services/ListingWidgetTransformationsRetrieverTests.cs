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
            articlesTransformationService.PageType.Returns(TransformationsMock.ARTICLE_CLASS_NAME);
            articlesTransformationService.Transformations.Returns(new List<Transformation> { TransformationsMock.Articles, TransformationsMock.ArticlesWithHeading });

            var coffeesTransformationService = Substitute.For<ITransformationService>();
            coffeesTransformationService.PageType.Returns(TransformationsMock.COFFEE_CLASS_NAME);
            coffeesTransformationService.Transformations.Returns(new List<Transformation> { TransformationsMock.Coffees });

            var cafesTransformationService = Substitute.For<ITransformationService>();
            cafesTransformationService.PageType.Returns(TransformationsMock.CAFE_CLASS_NAME);
            cafesTransformationService.Transformations.Returns(new List<Transformation> { TransformationsMock.Cafes });

            transformationsRetriever = new ListingWidgetTransformationsRetriever(new List<ITransformationService> { articlesTransformationService, coffeesTransformationService, cafesTransformationService });
        }


        [Test]
        public void Retrieve_Article_ReturnsArticlesTransformations()
        {
            var transformations = transformationsRetriever.Retrieve(TransformationsMock.ARTICLE_CLASS_NAME);

            Assert.That(transformations.Count, Is.EqualTo(2));
            Assert.That(transformations.Contains(TransformationsMock.Articles), Is.True);
            Assert.That(transformations.Contains(TransformationsMock.ArticlesWithHeading), Is.True);
        }


        [Test]
        public void Retrieve_Cafe_ReturnsCafesTransformations()
        {
            var transformations = transformationsRetriever.Retrieve(TransformationsMock.CAFE_CLASS_NAME);

            Assert.That(transformations.Count, Is.EqualTo(1));
            Assert.That(transformations.Contains(TransformationsMock.Cafes), Is.True);
        }


        [Test]
        public void Retrieve_Coffee_ReturnsCoffeesTransformations()
        {
            var transformations = transformationsRetriever.Retrieve(TransformationsMock.COFFEE_CLASS_NAME);

            Assert.That(transformations.Count, Is.EqualTo(1));
            Assert.That(transformations.Contains(TransformationsMock.Coffees), Is.True);
        }


        [Test]
        public void Retrieve_Brewer_ReturnsEmpty()
        {
            var transformations = transformationsRetriever.Retrieve(TransformationsMock.BREWER_CLASS_NAME);

            Assert.That(transformations, Is.Empty);
        }


        [Test]
        public void IsSupported_ArticlesTransformationAndArticlePageType_ReturnsTrue()
        {
            var supported = transformationsRetriever.IsSupported(TransformationsMock.Articles.View, TransformationsMock.ARTICLE_CLASS_NAME);

            Assert.That(supported, Is.True);
        }


        [Test]
        public void IsSupported_ArticlesWithHeadingTransformationAndArticlePageType_ReturnsTrue()
        {
            var supported = transformationsRetriever.IsSupported(TransformationsMock.ArticlesWithHeading.View, TransformationsMock.ARTICLE_CLASS_NAME);

            Assert.That(supported, Is.True);
        }


        [Test]
        public void IsSupported_CafesTransformationAndCafePageType_ReturnsTrue()
        {
            var supported = transformationsRetriever.IsSupported(TransformationsMock.Cafes.View, TransformationsMock.CAFE_CLASS_NAME);

            Assert.That(supported, Is.True);
        }


        [Test]
        public void IsSupported_CoffeesTransformationAndCoffeePageType_ReturnsTrue()
        {
            var supported = transformationsRetriever.IsSupported(TransformationsMock.Coffees.View, TransformationsMock.COFFEE_CLASS_NAME);

            Assert.That(supported, Is.True);
        }


        [Test]
        public void IsSupported_SupportedTransformationAndWrongPageType_ReturnsFalse()
        {
            var supportedForArticle = transformationsRetriever.IsSupported(TransformationsMock.Coffees.View, TransformationsMock.ARTICLE_CLASS_NAME);
            var supportedForCafe = transformationsRetriever.IsSupported(TransformationsMock.Coffees.View, TransformationsMock.CAFE_CLASS_NAME);
            var supportedForBrewer = transformationsRetriever.IsSupported(TransformationsMock.Coffees.View, TransformationsMock.BREWER_CLASS_NAME);

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
            var supported = transformationsRetriever.IsSupported("~Brewers/_default.cshtml", TransformationsMock.BREWER_CLASS_NAME);

            Assert.That(supported, Is.False);
        }


        [Test]
        public void IsSupported_EmptyTransformationParameter_ReturnsFalse()
        {
            var supported = transformationsRetriever.IsSupported("", TransformationsMock.ARTICLE_CLASS_NAME);

            Assert.That(supported, Is.False);
        }


        [Test]
        public void IsSupported_EmptyPageTypeParameter_ReturnsFalse()
        {
            var supported = transformationsRetriever.IsSupported(TransformationsMock.Articles.View, "");

            Assert.That(supported, Is.False);
        }
    }
}
