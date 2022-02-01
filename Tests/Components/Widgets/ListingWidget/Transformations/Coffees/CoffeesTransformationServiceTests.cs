using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;
using CMS.Ecommerce;
using CMS.Tests;

using Kentico.Content.Web.Mvc;

using NSubstitute;

using NUnit.Framework;

using Tests.DocumentEngine;

namespace DancingGoat.Widgets
{
    [TestFixture]
    public class CoffeesTransformationServiceTests : UnitTests
    {
        private CoffeesTransformationService coffeesService;
        private IPageUrlRetriever pageUrlRetriever;


        [SetUp]
        public void SetUp()
        {
            pageUrlRetriever = Substitute.For<IPageUrlRetriever>();
            coffeesService = new CoffeesTransformationService(pageUrlRetriever);

            Fake().DocumentType<Coffee>(Coffee.CLASS_NAME);
        }


        [Test]
        public void GetCustomCacheDependency_CoffeesTransformation_ReturnsNull()
        {
            var customCacheDependency = coffeesService.GetCustomCacheDependency(TransformationsMock.Coffees.View);

            Assert.That(customCacheDependency, Is.Null);
        }


        [Test]
        public void GetCustomCacheDependencyKey_CoffeesTransformation_ReturnsNull()
        {
            var customCacheDependencyKey = coffeesService.GetCustomCacheDependencyKey(TransformationsMock.Coffees.View);

            Assert.That(customCacheDependencyKey, Is.Null);
        }


        [Test]
        public void GetCustomQueryParametrization_CoffeesTransformation_ReturnsNull()
        {
            var customQueryParametrization = coffeesService.GetCustomQueryParametrization(TransformationsMock.Coffees.View);

            Assert.That(customQueryParametrization, Is.Null);
        }


        [Test]
        public void GetModel_NoPages_ReturnsEmptyModel()
        {
            var model = coffeesService.GetModel(Enumerable.Empty<TreeNode>());
            Assert.Multiple(() =>
            {
                Assert.That(model, Is.Not.Null);
                var typedModel = model as CoffeesTransformationViewModel;
                Assert.That(typedModel, Is.Not.Null);
                Assert.That(typedModel.Coffees.Count(), Is.EqualTo(0));
            });
        }


        [Test]
        public void GetModel_MultiplePages_ReturnsHydratedModel()
        {
            var coffee = TreeNode.New<Coffee>();
            coffee.SKU = Substitute.For<SKUInfo>();
            pageUrlRetriever.Retrieve(coffee).Returns(Substitute.For<PageUrl>());

            var pages = new List<TreeNode> { coffee, coffee };
            var model = coffeesService.GetModel(pages);
            Assert.Multiple(() =>
            {
                Assert.That(model, Is.Not.Null);
                var typedModel = model as CoffeesTransformationViewModel;
                Assert.That(typedModel, Is.Not.Null);
                Assert.That(typedModel.Coffees.Count(), Is.EqualTo(2));
                Assert.That(typedModel.Coffees.ElementAtOrDefault(0), Is.Not.Null);
                Assert.That(typedModel.Coffees.ElementAtOrDefault(1), Is.Not.Null);
            });
        }
    }
}
