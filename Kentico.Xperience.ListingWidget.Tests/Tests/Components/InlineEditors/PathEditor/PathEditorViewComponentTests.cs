﻿using CMS.Tests;

using Kentico.Xperience.ListingWidget.Widgets;

using Microsoft.AspNetCore.Mvc.ViewComponents;

using NSubstitute;

using NUnit.Framework;

namespace Kentico.Xperience.ListingWidget.InlineEditors.Tests
{
    [TestFixture]
    [Category.Unit]
    public class PathEditorViewComponentTests
    {
        private const string NAME = "Chemex";
        private const string PATH = "/Store/Brewers/Chemex";
        private const string INVALID_PATH = "/Store/Brewers/Aeropress";
        private const string INVALID_PAGE_MESSAGE = "Page does not exist";
        private const string NO_PAGE_SELECTED_MESSAGE = "No page selected";
        private const string PAGE_INACCESSIBLE_MESSAGE = "The selected page has been deleted or its alias path has been changed.";

        private IPageRepository repository;
        private PathEditorViewComponent component;


        [SetUp]
        public void SetUp()
        {
            repository = Substitute.For<IPageRepository>();
            component = new PathEditorViewComponent(repository);
            repository.GetPageName(PATH).Returns(NAME);
            repository.GetPageName(INVALID_PATH).Returns((string)null);
        }


        [Test]
        public void Invoke_NullPage_ReturnsModelWithNotSelectedPageState()
        {
            var result = component.Invoke(nameof(ListingWidgetProperties.ParentPageAliasPath), null) as ViewViewComponentResult;
            var viewModel = result.ViewData.Model as PathEditorViewModel;

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(viewModel, Is.Not.Null);
                Assert.That(viewModel.PageSelectionState, Is.EqualTo(PageSelectionState.NotSelected));
                Assert.That(viewModel.PropertyName, Is.EqualTo(nameof(ListingWidgetProperties.ParentPageAliasPath)));
                Assert.That(viewModel.Title, Is.EqualTo(""));
                Assert.That(viewModel.Value, Is.EqualTo(NO_PAGE_SELECTED_MESSAGE));
            });
        }


        [Test]
        public void Invoke_NonExistingPage_ReturnsModelWithInvalidPageState()
        {
            var result = component.Invoke(nameof(ListingWidgetProperties.ParentPageAliasPath), INVALID_PATH) as ViewViewComponentResult;
            var viewModel = result.ViewData.Model as PathEditorViewModel;

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(viewModel, Is.Not.Null);
                Assert.That(viewModel.PageSelectionState, Is.EqualTo(PageSelectionState.Inaccessible));
                Assert.That(viewModel.PropertyName, Is.EqualTo(nameof(ListingWidgetProperties.ParentPageAliasPath)));
                Assert.That(viewModel.Title, Is.EqualTo(PAGE_INACCESSIBLE_MESSAGE));
                Assert.That(viewModel.Value, Is.EqualTo(INVALID_PAGE_MESSAGE));
            });
        }


        [Test]
        public void Invoke_ExistingPage_ReturnsModelWithSelectedValidPageState()
        {
            var result = component.Invoke(nameof(ListingWidgetProperties.ParentPageAliasPath), PATH) as ViewViewComponentResult;
            var viewModel = result.ViewData.Model as PathEditorViewModel;

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(viewModel, Is.Not.Null);
                Assert.That(viewModel.PageSelectionState, Is.EqualTo(PageSelectionState.Valid));
                Assert.That(viewModel.PropertyName, Is.EqualTo(nameof(ListingWidgetProperties.ParentPageAliasPath)));
                Assert.That(viewModel.Title, Is.EqualTo(PATH));
                Assert.That(viewModel.Value, Is.EqualTo(NAME));
            });
        }
    }
}
