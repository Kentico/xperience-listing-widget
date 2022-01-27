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
    public class SupportedTransformationsRetrieverTests
    {
        private SupportedTransformationsRetriever supportedTransformations;


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

            supportedTransformations = new SupportedTransformationsRetriever(new List<ITransformationService> { articlesTransformationService, coffeesTransformationService, cafesTransformationService });
        }


        [Test]
        public void GetTransformations_Article_ReturnsArticlesTransformations()
        {
            var transformations = supportedTransformations.GetTransformations(Article.CLASS_NAME);

            Assert.That(transformations.Count, Is.EqualTo(2));
            Assert.That(transformations.Contains(TransformationsMock.Articles), Is.True);
            Assert.That(transformations.Contains(TransformationsMock.ArticlesWithHeading), Is.True);
        }


        [Test]
        public void GetTransformations_Cafe_ReturnsCafesTransformations()
        {
            var transformations = supportedTransformations.GetTransformations(Cafe.CLASS_NAME);

            Assert.That(transformations.Count, Is.EqualTo(1));
            Assert.That(transformations.Contains(TransformationsMock.Cafes), Is.True);
        }


        [Test]
        public void GetTransformations_Coffee_ReturnsCoffeesTransformations()
        {
            var transformations = supportedTransformations.GetTransformations(Coffee.CLASS_NAME);

            Assert.That(transformations.Count, Is.EqualTo(1));
            Assert.That(transformations.Contains(TransformationsMock.Coffees), Is.True);
        }


        [Test]
        public void GetTransformations_Brewer_ReturnsEmpty()
        {
            var transformations = supportedTransformations.GetTransformations(Brewer.CLASS_NAME);

            Assert.That(transformations, Is.Empty);
        }


        [Test]
        public void IsTransformationSupported_ArticlesTransformationAndArticlePageType_ReturnsTrue()
        {
            var supported = supportedTransformations.IsTransformationSupported(TransformationsMock.Articles.View, Article.CLASS_NAME);

            Assert.That(supported, Is.True);
        }


        [Test]
        public void IsTransformationSupported_ArticlesWithHeadingTransformationAndArticlePageType_ReturnsTrue()
        {
            var supported = supportedTransformations.IsTransformationSupported(TransformationsMock.ArticlesWithHeading.View, Article.CLASS_NAME);

            Assert.That(supported, Is.True);
        }


        [Test]
        public void IsTransformationSupported_CafesTransformationAndArticlePageType_ReturnsTrue()
        {
            var supported = supportedTransformations.IsTransformationSupported(TransformationsMock.Cafes.View, Cafe.CLASS_NAME);

            Assert.That(supported, Is.True);
        }


        [Test]
        public void IsTransformationSupported_CoffeesTransformationAndArticlePageType_ReturnsTrue()
        {
            var supported = supportedTransformations.IsTransformationSupported(TransformationsMock.Coffees.View, Coffee.CLASS_NAME);

            Assert.That(supported, Is.True);
        }


        [Test]
        public void IsTransformationSupported_SupportedTransformationAndWrongPageType_ReturnsFalse()
        {
            var supportedForArticle = supportedTransformations.IsTransformationSupported(TransformationsMock.Coffees.View, Article.CLASS_NAME);
            var supportedForCafe = supportedTransformations.IsTransformationSupported(TransformationsMock.Coffees.View, Cafe.CLASS_NAME);
            var supportedForBrewer = supportedTransformations.IsTransformationSupported(TransformationsMock.Coffees.View, Brewer.CLASS_NAME);

            Assert.Multiple(() =>
            {
                Assert.That(supportedForArticle, Is.False);
                Assert.That(supportedForCafe, Is.False);
                Assert.That(supportedForBrewer, Is.False);
            });
        }


        [Test]
        public void IsTransformationSupported_NotSupportedTransformation_ReturnsFalse()
        {
            var supported = supportedTransformations.IsTransformationSupported("~Brewers/_default.cshtml", Brewer.CLASS_NAME);

            Assert.That(supported, Is.False);
        }


        [Test]
        public void IsTransformationSupported_EmptyTransformationParameter_ReturnsFalse()
        {
            var supported = supportedTransformations.IsTransformationSupported("", Article.CLASS_NAME);

            Assert.That(supported, Is.False);
        }


        [Test]
        public void IsTransformationSupported_EmptyPageTypeParameter_ReturnsFalse()
        {
            var supported = supportedTransformations.IsTransformationSupported(TransformationsMock.Articles.View, "");

            Assert.That(supported, Is.False);
        }
    }
}
