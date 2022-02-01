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
    public class ListingWidgetTransformationsRetrieverTests
    {
        private ListingWidgetTransformationsRetriever transformationsRetriever;


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

            transformationsRetriever = new ListingWidgetTransformationsRetriever(new List<ITransformationService> { articlesTransformationService, coffeesTransformationService, cafesTransformationService });
        }


        [Test]
        public void Retrieve_Article_ReturnsArticlesTransformations()
        {
            var transformations = transformationsRetriever.Retrieve(Article.CLASS_NAME);

            Assert.That(transformations.Count, Is.EqualTo(2));
            Assert.That(transformations.Contains(TransformationsMock.Articles), Is.True);
            Assert.That(transformations.Contains(TransformationsMock.ArticlesWithHeading), Is.True);
        }


        [Test]
        public void Retrieve_Cafe_ReturnsCafesTransformations()
        {
            var transformations = transformationsRetriever.Retrieve(Cafe.CLASS_NAME);

            Assert.That(transformations.Count, Is.EqualTo(1));
            Assert.That(transformations.Contains(TransformationsMock.Cafes), Is.True);
        }


        [Test]
        public void Retrieve_Coffee_ReturnsCoffeesTransformations()
        {
            var transformations = transformationsRetriever.Retrieve(Coffee.CLASS_NAME);

            Assert.That(transformations.Count, Is.EqualTo(1));
            Assert.That(transformations.Contains(TransformationsMock.Coffees), Is.True);
        }


        [Test]
        public void Retrieve_Brewer_ReturnsEmpty()
        {
            var transformations = transformationsRetriever.Retrieve(Brewer.CLASS_NAME);

            Assert.That(transformations, Is.Empty);
        }


        [Test]
        public void IsSupported_ArticlesTransformationAndArticlePageType_ReturnsTrue()
        {
            var supported = transformationsRetriever.IsSupported(TransformationsMock.Articles.View, Article.CLASS_NAME);

            Assert.That(supported, Is.True);
        }


        [Test]
        public void IsSupported_ArticlesWithHeadingTransformationAndArticlePageType_ReturnsTrue()
        {
            var supported = transformationsRetriever.IsSupported(TransformationsMock.ArticlesWithHeading.View, Article.CLASS_NAME);

            Assert.That(supported, Is.True);
        }


        [Test]
        public void IsSupported_CafesTransformationAndCafePageType_ReturnsTrue()
        {
            var supported = transformationsRetriever.IsSupported(TransformationsMock.Cafes.View, Cafe.CLASS_NAME);

            Assert.That(supported, Is.True);
        }


        [Test]
        public void IsSupported_CoffeesTransformationAndCoffeePageType_ReturnsTrue()
        {
            var supported = transformationsRetriever.IsSupported(TransformationsMock.Coffees.View, Coffee.CLASS_NAME);

            Assert.That(supported, Is.True);
        }


        [Test]
        public void IsSupported_SupportedTransformationAndWrongPageType_ReturnsFalse()
        {
            var supportedForArticle = transformationsRetriever.IsSupported(TransformationsMock.Coffees.View, Article.CLASS_NAME);
            var supportedForCafe = transformationsRetriever.IsSupported(TransformationsMock.Coffees.View, Cafe.CLASS_NAME);
            var supportedForBrewer = transformationsRetriever.IsSupported(TransformationsMock.Coffees.View, Brewer.CLASS_NAME);

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
            var supported = transformationsRetriever.IsSupported("~Brewers/_default.cshtml", Brewer.CLASS_NAME);

            Assert.That(supported, Is.False);
        }


        [Test]
        public void IsSupported_EmptyTransformationParameter_ReturnsFalse()
        {
            var supported = transformationsRetriever.IsSupported("", Article.CLASS_NAME);

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
