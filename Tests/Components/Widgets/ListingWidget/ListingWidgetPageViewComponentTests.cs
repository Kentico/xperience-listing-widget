﻿using System.Collections.Generic;
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
    public class ListingWidgetPageViewComponentTests : UnitTests
    {
        private const string VIEW_NAME = "~/Components/Widgets/ListingWidget/_ListingWidget.cshtml";

        private PageRepository pageRepository;
        private ListingWidgetViewComponent listingWidgetViewComponent;
        private ComponentViewModel<ListingWidgetProperties> componentViewModel;
        private Article article;


        [SetUp]
        public void SetUp()
        {
            pageRepository = Substitute.For<PageRepository>(Substitute.For<IPageRetriever>());
            var pageBuilderDataContextRetriever = Substitute.For<IPageBuilderDataContextRetriever>();
            pageBuilderDataContextRetriever.Retrieve().EditMode.Returns(true);
            listingWidgetViewComponent = new ListingWidgetViewComponent(pageRepository, pageBuilderDataContextRetriever);

            var page = Substitute.For<TreeNode>();
            var properties = new ListingWidgetProperties();
            componentViewModel = ComponentViewModel<ListingWidgetProperties>.Create(page, properties);

            Fake().DocumentType<Article>(Article.CLASS_NAME);
            article = TreeNode.New<Article>();
            article.DocumentName = "Article page";
        }


        [Test]
        public void Invoke_PropertiesWithSelectedPageType_ReturnsCorrectViewWithPagesOfSelectedPageType()
        {
            var testPageType = Article.CLASS_NAME;
            componentViewModel.Properties.SelectedPageType = testPageType;
            pageRepository.GetPages(Article.CLASS_NAME).Returns(new List<TreeNode> { article });

            var viewResult = listingWidgetViewComponent.Invoke(componentViewModel) as ViewViewComponentResult;
            var viewModel = viewResult.ViewData.Model as ListingWidgetViewModel;

            Assert.Multiple(() =>
            {
                Assert.That(viewResult.ViewName, Is.EqualTo(VIEW_NAME));
                Assert.That(viewModel.Pages.Count(), Is.EqualTo(1));
                Assert.That(viewModel.Pages.First().DocumentName, Is.EqualTo("Article page"));
                Assert.That(viewModel.PageTypeSelectorViewModel.SelectedOption, Is.EqualTo(testPageType));
            });
        }


        [Test]
        public void Invoke_PropertiesWithoutSelectedPageType_ReturnsCorrectViewWithoutPages()
        {
            pageRepository.GetPages(Article.CLASS_NAME).Returns(new List<TreeNode> { article });

            var viewResult = listingWidgetViewComponent.Invoke(componentViewModel) as ViewViewComponentResult;
            var viewModel = viewResult.ViewData.Model as ListingWidgetViewModel;

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.Pages.Count(), Is.EqualTo(0));
                Assert.That(viewModel.PageTypeSelectorViewModel.SelectedOption, Is.EqualTo(null));
            });
        }


        [Test]
        public void Invoke_PropertiesWithSelectedPageTypeAndEmptyRepository_ReturnsCorrectViewModelWithoutPages()
        {
            var testPageType = "Test.ClassName";
            componentViewModel.Properties.SelectedPageType = testPageType;

            var viewResult = listingWidgetViewComponent.Invoke(componentViewModel) as ViewViewComponentResult;
            var viewModel = viewResult.ViewData.Model as ListingWidgetViewModel;

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.Pages.Count(), Is.EqualTo(0));
                Assert.That(viewModel.PageTypeSelectorViewModel.SelectedOption, Is.EqualTo(testPageType));
            });
        }
    }
}
