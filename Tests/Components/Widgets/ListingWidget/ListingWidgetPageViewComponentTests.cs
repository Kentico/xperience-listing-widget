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
        private List<TreeNode> pages;


        [SetUp]
        public void SetUp()
        {
            pageRepository = Substitute.ForPartsOf<PageRepository>(Substitute.For<IPageRetriever>());
            listingWidgetViewComponent = new ListingWidgetViewComponent(pageRepository);

            var page = Substitute.For<TreeNode>();
            var properties = Substitute.For<ListingWidgetProperties>();
            componentViewModel = ComponentViewModel<ListingWidgetProperties>.Create(page, properties);

            Fake().DocumentType<Article>(Article.CLASS_NAME);
            var article = TreeNode.New<Article>();
            article.DocumentName = "Article page";
            
            Fake().DocumentType<Coffee>(Coffee.CLASS_NAME);
            var coffee = TreeNode.New<Coffee>();
            coffee.DocumentName = "Coffee page";
            
            Fake().DocumentType<Brewer>(Brewer.CLASS_NAME);
            var brewer = TreeNode.New<Brewer>();
            brewer.DocumentName = "Brewer page";
            
            pages = new List<TreeNode> { article, coffee, brewer };
        }


        [Test]
        public void Invoke_EmptyRepository_ReturnsCorrectViewModelWithoutPages()
        {
            var viewResult = listingWidgetViewComponent.Invoke(componentViewModel) as ViewViewComponentResult;
            var viewModel = viewResult.ViewData.Model as ListingWidgetViewModel;

            Assert.Multiple(() =>
            {
                Assert.That(viewResult.ViewName, Is.EqualTo(VIEW_NAME));
                Assert.That(viewModel.Pages.Count(), Is.EqualTo(0));
                Assert.That(viewModel.SelectedPageType, Is.EqualTo(null));
            });
        }

        [Test]
        public void Invoke_PropertiesWithoutSlectedPageType_ReturnsCorrectViewWithoutPages()
        {
            pageRepository.GetAllPages<TreeNode>().Returns(pages);

            var viewResult = listingWidgetViewComponent.Invoke(componentViewModel) as ViewViewComponentResult;
            var viewModel = viewResult.ViewData.Model as ListingWidgetViewModel;

            Assert.Multiple(() =>
            {
                Assert.That(viewResult.ViewName, Is.EqualTo(VIEW_NAME));
                Assert.That(viewModel.Pages.Count(), Is.EqualTo(0));
                Assert.That(viewModel.SelectedPageType, Is.EqualTo(null));
            });
        }

        
        [Test]
        public void Invoke_PropertiesWithSelectedArticlePageType_ReturnsCorrectViewWithPagesOfArticlePageType()
        {
            pageRepository.GetAllPages<TreeNode>().Returns(pages);
            var testPageType = Article.CLASS_NAME;
            componentViewModel.Properties.SelectedPageType = testPageType;

            var viewResult = listingWidgetViewComponent.Invoke(componentViewModel) as ViewViewComponentResult;
            var viewModel = viewResult.ViewData.Model as ListingWidgetViewModel;

            Assert.Multiple(() =>
            {
                Assert.That(viewResult.ViewName, Is.EqualTo(VIEW_NAME));
                Assert.That(viewModel.Pages.Count(), Is.EqualTo(1));
                Assert.That(viewModel.Pages.First().DocumentName, Is.EqualTo("Article page"));
                Assert.That(viewModel.SelectedPageType, Is.EqualTo(testPageType));
            });
        }
    }
}
