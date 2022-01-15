using CMS.Tests;

using DancingGoat.Models;
using DancingGoat.Widgets;

using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Localization;

using NSubstitute;

using NUnit.Framework;

namespace DancingGoat.InlineEditors
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
        private const string PAGE_INACCESSIBLE_MESSAGE = "The selected page has been deleted or changed alias path.";

        private IPageRepository repository;
        private PathEditorViewComponent component;


        [SetUp]
        public void SetUp()
        {
            repository = Substitute.For<IPageRepository>();
            var localizer = Substitute.For<IStringLocalizer<SharedResources>>();
            component = new PathEditorViewComponent(repository, localizer);
            repository.GetPageName(PATH).Returns(NAME);
            repository.GetPageName(INVALID_PATH).Returns((string)null);
            localizer[INVALID_PAGE_MESSAGE].Returns(new LocalizedString(INVALID_PAGE_MESSAGE, INVALID_PAGE_MESSAGE));
            localizer[NO_PAGE_SELECTED_MESSAGE].Returns(new LocalizedString(NO_PAGE_SELECTED_MESSAGE, NO_PAGE_SELECTED_MESSAGE));
            localizer[PAGE_INACCESSIBLE_MESSAGE].Returns(new LocalizedString(PAGE_INACCESSIBLE_MESSAGE, PAGE_INACCESSIBLE_MESSAGE));
        }


        [Test]
        public void Invoke_NullPage_ReturnsModelWithNotSelectedPageState()
        {
            var result = component.Invoke(nameof(ListingWidgetProperties.SelectedPage), null) as ViewViewComponentResult;
            var viewModel = result.ViewData.Model as PathEditorViewModel;

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(viewModel, Is.Not.Null);
                Assert.That(viewModel.PageSelectionState, Is.EqualTo(PageSelectionState.NotSelected));
                Assert.That(viewModel.PropertyName, Is.EqualTo(nameof(ListingWidgetProperties.SelectedPage)));
                Assert.That(viewModel.Title, Is.EqualTo(""));
                Assert.That(viewModel.Value, Is.EqualTo(NO_PAGE_SELECTED_MESSAGE));
            });
        }


        [Test]
        public void Invoke_NonExistingPage_ReturnsModelWithInvalidPageState()
        {
            var invalidPage = new SelectedPage { Name = "Aeropress", Path = INVALID_PATH };
            var result = component.Invoke(nameof(ListingWidgetProperties.SelectedPage), invalidPage) as ViewViewComponentResult;
            var viewModel = result.ViewData.Model as PathEditorViewModel;

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(viewModel, Is.Not.Null);
                Assert.That(viewModel.PageSelectionState, Is.EqualTo(PageSelectionState.Inaccessible));
                Assert.That(viewModel.PropertyName, Is.EqualTo(nameof(ListingWidgetProperties.SelectedPage)));
                Assert.That(viewModel.Title, Is.EqualTo(PAGE_INACCESSIBLE_MESSAGE));
                Assert.That(viewModel.Value, Is.EqualTo(INVALID_PAGE_MESSAGE));
            });
        }


        [Test]
        public void Invoke_ExistingPage_ReturnsModelWithSelectedValidPageState()
        {
            var page = new SelectedPage { Name = NAME, Path = PATH };
            var result = component.Invoke(nameof(ListingWidgetProperties.SelectedPage), page) as ViewViewComponentResult;
            var viewModel = result.ViewData.Model as PathEditorViewModel;

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(viewModel, Is.Not.Null);
                Assert.That(viewModel.PageSelectionState, Is.EqualTo(PageSelectionState.Valid));
                Assert.That(viewModel.PropertyName, Is.EqualTo(nameof(ListingWidgetProperties.SelectedPage)));
                Assert.That(viewModel.Title, Is.EqualTo(PATH));
                Assert.That(viewModel.Value, Is.EqualTo(NAME));
            });
        }
    }
}
