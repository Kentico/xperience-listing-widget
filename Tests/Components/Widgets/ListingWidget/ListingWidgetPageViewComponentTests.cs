using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc.ViewComponents;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;
using CMS.Tests;

using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

using DancingGoat.Models;
using DancingGoat.Widgets;

using Tests.DocumentEngine;

using NSubstitute;
using NUnit.Framework;

namespace DancingGoatCore.Widgets.Tests
{
    class ListingWidgetPageViewComponentTests : UnitTests
    {
        private const string VIEW_NAME = "~/Components/Widgets/ListingWidget/_ListingWidget.cshtml";

        private PageRepository pageRepository;
        private ListingWidgetViewComponent listingWidgetViewComponent;
        private ComponentViewModel<ListingWidgetProperties> componentViewModel;


        [SetUp]
        public void SetUp()
        {
            pageRepository = Substitute.ForPartsOf<PageRepository>(Substitute.For<IPageRetriever>());
            listingWidgetViewComponent = new ListingWidgetViewComponent(pageRepository);

            var page = Substitute.For<TreeNode>();
            var properties = Substitute.For<ListingWidgetProperties>();
            componentViewModel = ComponentViewModel<ListingWidgetProperties>.Create(page, properties);
        }


        [Test]
        public void Invoke_RepositoryWithoutPages_ReturnsCorrectViewModelWithoutPages()
        {
            var viewResult = listingWidgetViewComponent.Invoke(componentViewModel) as ViewViewComponentResult;
            var viewModel = viewResult.ViewData.Model as ListingWidgetViewModel;

            Assert.Multiple(() =>
            {
                Assert.That(viewResult.ViewName, Is.EqualTo(VIEW_NAME));
                Assert.That(viewModel.Pages.Count(), Is.EqualTo(0));
                Assert.That(viewModel.SupportedPageTypes, Is.EqualTo(ListingWidgetProperties.SupportedPageTypes));
                Assert.That(viewModel.SelectedPageType, Is.EqualTo(null));
            });
        }


        [Test]
        public void Invoke_RepositoryWithTwoPages_ReturnsCorrectViewModelWithTwoPages()
        {
            Fake().DocumentType<Coffee>(Coffee.CLASS_NAME);
            Fake().DocumentType<Article>(Article.CLASS_NAME);
            var pagesList = new List<TreeNode> { TreeNode.New<Article>(), TreeNode.New<Coffee>() };
            pageRepository.GetAllPages<TreeNode>().Returns(pagesList);

            var viewResult = listingWidgetViewComponent.Invoke(componentViewModel) as ViewViewComponentResult;
            var viewModel = viewResult.ViewData.Model as ListingWidgetViewModel;

            Assert.Multiple(() =>
            {
                Assert.That(viewResult.ViewName, Is.EqualTo(VIEW_NAME));
                Assert.That(viewModel.Pages.Count(), Is.EqualTo(2));
                Assert.That(viewModel.SupportedPageTypes, Is.EqualTo(ListingWidgetProperties.SupportedPageTypes));
                Assert.That(viewModel.SelectedPageType, Is.EqualTo(null));
            });
        }


        [Test]
        public void Invoke_PropertiesWithSelectedPageType_ReturnsCorrectViewModelWithSelectedPageType()
        {
            var testPageType = "Article";
            componentViewModel.Properties.SelectedPageType = testPageType;

            var viewResult = listingWidgetViewComponent.Invoke(componentViewModel) as ViewViewComponentResult;
            var viewModel = viewResult.ViewData.Model as ListingWidgetViewModel;

            Assert.Multiple(() =>
            {
                Assert.That(viewResult.ViewName, Is.EqualTo(VIEW_NAME));
                Assert.That(viewModel.Pages.Count(), Is.EqualTo(0));
                Assert.That(viewModel.SupportedPageTypes, Is.EqualTo(ListingWidgetProperties.SupportedPageTypes));
                Assert.That(viewModel.SelectedPageType, Is.EqualTo(testPageType));
            });
        }

    }
}
