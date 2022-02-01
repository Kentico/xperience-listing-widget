using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;
using CMS.Tests;

using DancingGoat.Models;

using Kentico.Content.Web.Mvc;

using Microsoft.Extensions.Localization;

using NSubstitute;

using NUnit.Framework;

using Tests.DocumentEngine;

namespace DancingGoat.Widgets
{
    [TestFixture]
    public class CafesTransformationServiceTests : UnitTests
    {
        private CafesTransformationService cafesService;
        private ICountryRepository countryRepository;


        [SetUp]
        public void SetUp()
        {
            var pageAttachmentUrlRetriever = Substitute.For<IPageAttachmentUrlRetriever>();
            var localizer = Substitute.For<IStringLocalizer<SharedResources>>();
            countryRepository = Substitute.For<ICountryRepository>();
            cafesService = new CafesTransformationService(pageAttachmentUrlRetriever, localizer, countryRepository);

            Fake().DocumentType<Cafe>(Cafe.CLASS_NAME);
        }


        [Test]
        public void GetCustomCacheDependency_CafesTransformation_ReturnsNull()
        {
            var customCacheDependency = cafesService.GetCustomCacheDependency(TransformationsMock.Cafes.View);

            Assert.That(customCacheDependency, Is.Null);
        }


        [Test]
        public void GetCustomCacheDependencyKey_CafesTransformation_ReturnsNull()
        {
            var customCacheDependencyKey = cafesService.GetCustomCacheDependencyKey(TransformationsMock.Cafes.View);

            Assert.That(customCacheDependencyKey, Is.Null);
        }


        [Test]
        public void GetCustomQueryParametrization_CafesTransformation_ReturnsActionForSelectingCompanyCafes()
        {
            var customQueryParametrization = cafesService.GetCustomQueryParametrization(TransformationsMock.Cafes.View);
            var query = new DocumentQuery(Cafe.CLASS_NAME);
            customQueryParametrization.Invoke(query);

            Assert.That(customQueryParametrization, Is.Not.Null);
            Assert.That(query, Is.EqualTo(new DocumentQuery(Cafe.CLASS_NAME).WhereTrue("CafeIsCompanyCafe")));
        }


        [Test]
        public void GetModel_NoPages_ReturnsEmptyModel()
        {
            var model = cafesService.GetModel(Enumerable.Empty<TreeNode>());
            Assert.Multiple(() =>
            {
                Assert.That(model, Is.Not.Null);
                var typedModel = model as CafesTransformationViewModel;
                Assert.That(typedModel, Is.Not.Null);
                Assert.That(typedModel.Cafes.Count(), Is.EqualTo(0));
            });
        }


        [Test]
        public void GetModel_MultiplePages_ReturnsHydratedModel()
        {
            countryRepository.GetCountry((string)default).ReturnsForAnyArgs(Substitute.For<CMS.Globalization.CountryInfo>());
            var cafe = TreeNode.New<Cafe>();

            var pages = new List<TreeNode> { cafe, cafe };
            var model = cafesService.GetModel(pages);
            Assert.Multiple(() =>
            {
                Assert.That(model, Is.Not.Null);
                var typedModel = model as CafesTransformationViewModel;
                Assert.That(typedModel, Is.Not.Null);
                Assert.That(typedModel.Cafes.Count(), Is.EqualTo(2));
                Assert.That(typedModel.Cafes.ElementAtOrDefault(0), Is.Not.Null);
                Assert.That(typedModel.Cafes.ElementAtOrDefault(1), Is.Not.Null);
            });
        }
    }
}
