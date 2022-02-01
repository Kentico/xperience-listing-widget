using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;
using CMS.Tests;

using Kentico.Content.Web.Mvc;

using NSubstitute;

using NUnit.Framework;

using Tests.DocumentEngine;

namespace DancingGoat.Widgets
{
    [TestFixture]
    public class ArticlesTransformationServiceTests : UnitTests
    {
        private ArticlesTransformationService articlesService;
        private IPageUrlRetriever pageUrlRetriever;


        [SetUp]
        public void SetUp()
        {
            pageUrlRetriever = Substitute.For<IPageUrlRetriever>();
            var pageAttachmentUrlRetriever = Substitute.For<IPageAttachmentUrlRetriever>();
            articlesService = new ArticlesTransformationService(pageUrlRetriever, pageAttachmentUrlRetriever);

            Fake().DocumentType<Article>(Article.CLASS_NAME);
        }


        [Test]
        public void GetCustomCacheDependency_ArticlesTransformation_ReturnsNull()
        {
            var customCacheDependency = articlesService.GetCustomCacheDependency(TransformationsMock.Articles.View);

            Assert.That(customCacheDependency, Is.Null);
        }


        [Test]
        public void GetCustomCacheDependency_ArticlesWithHeadingTransformation_ReturnsNull()
        {
            var customCacheDependency = articlesService.GetCustomCacheDependency(TransformationsMock.ArticlesWithHeading.View);

            Assert.That(customCacheDependency, Is.Null);
        }


        [Test]
        public void GetCustomCacheDependencyKey_ArticlesTransformation_ReturnsNull()
        {
            var customCacheDependencyKey = articlesService.GetCustomCacheDependencyKey(TransformationsMock.Articles.View);

            Assert.That(customCacheDependencyKey, Is.Null);
        }


        [Test]
        public void GetCustomCacheDependencyKey_ArticlesWithHeadingTransformation_ReturnsNull()
        {
            var customCacheDependencyKey = articlesService.GetCustomCacheDependencyKey(TransformationsMock.ArticlesWithHeading.View);

            Assert.That(customCacheDependencyKey, Is.Null);
        }


        [Test]
        public void GetCustomQueryParametrization_ArticlesTransformation_ReturnsNull()
        {
            var customQueryParametrization = articlesService.GetCustomQueryParametrization(TransformationsMock.Articles.View);

            Assert.That(customQueryParametrization, Is.Null);
        }


        [Test]
        public void GetCustomQueryParametrization_ArticlesWithHeadingTransformation_ReturnsNull()
        {
            var customQueryParametrization = articlesService.GetCustomQueryParametrization(TransformationsMock.ArticlesWithHeading.View);

            Assert.That(customQueryParametrization, Is.Null);
        }


        [Test]
        public void GetModel_NoPages_ReturnsEmptyModel()
        {
            var model = articlesService.GetModel(Enumerable.Empty<TreeNode>());
            Assert.Multiple(() =>
            {
                Assert.That(model, Is.Not.Null);
                var typedModel = model as ArticlesTransformationViewModel;
                Assert.That(typedModel, Is.Not.Null);
                Assert.That(typedModel.Articles.Count(), Is.EqualTo(0));
            });
        }


        [Test]
        public void GetModel_MultiplePages_ReturnsHydratedModel()
        {
            var article = TreeNode.New<Article>();
            pageUrlRetriever.Retrieve(article).Returns(Substitute.For<PageUrl>());

            var pages = new List<TreeNode> { article, article };
            var model = articlesService.GetModel(pages);
            Assert.Multiple(() =>
            {
                Assert.That(model, Is.Not.Null);
                var typedModel = model as ArticlesTransformationViewModel;
                Assert.That(typedModel, Is.Not.Null);
                Assert.That(typedModel.Articles.Count(), Is.EqualTo(2));
                Assert.That(typedModel.Articles.ElementAtOrDefault(0), Is.Not.Null);
                Assert.That(typedModel.Articles.ElementAtOrDefault(1), Is.Not.Null);
            });
        }
    }
}
