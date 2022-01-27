using CMS.Tests;

using NUnit.Framework;

namespace DancingGoat.Widgets
{
    [TestFixture]
    [Category.Unit]
    public class SupportedTransformationsTests
    {
        private readonly SupportedTransformations supportedTransformations = new SupportedTransformations();

        [Test]
        public void TransformationExists_ArticleTransformation_ReturnsTrue()
        {
            Assert.That(supportedTransformations.IsTransformationSupported(supportedTransformations.Articles.View), Is.True);
        }


        [Test]
        public void TransformationExists_ArticleWithHeadingTransformation_ReturnsTrue()
        {
            Assert.That(supportedTransformations.IsTransformationSupported(supportedTransformations.ArticlesWithHeading.View), Is.True);
        }


        [Test]
        public void TransformationExists_CafeTransformation_ReturnsTrue()
        {
            Assert.That(supportedTransformations.IsTransformationSupported(supportedTransformations.Cafes.View), Is.True);
        }


        [Test]
        public void TransformationExists_CoffeeTransformation_ReturnsTrue()
        {
            Assert.That(supportedTransformations.IsTransformationSupported(supportedTransformations.Coffees.View), Is.True);
        }


        [TestCase(null)]
        [TestCase("")]
        [TestCase("Brewers.cshtml")]
        public void TransformationExists_NotExistingTransformation_ReturnsFalse(string transformation)
        {
            Assert.That(supportedTransformations.IsTransformationSupported(transformation), Is.False);
        }
    }
}
