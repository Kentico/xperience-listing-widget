using DancingGoat.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace DancingGoat.InlineEditors
{
    /// <summary>
    /// ViewComponent for path inline editor.
    /// </summary>
    public class PathEditorViewComponent : ViewComponent
    {
        private readonly IPageRepository repository;
        private readonly IStringLocalizer<SharedResources> localizer;


        /// <summary>
        /// Creates an instance of <see cref="PathEditorViewComponent"/> class.
        /// </summary>
        /// <param name="repository">Page repository.</param>
        /// <param name="localizer">Represents an <see cref="IStringLocalizer"/> that provides localized strings.</param>
        public PathEditorViewComponent(IPageRepository repository, IStringLocalizer<SharedResources> localizer)
        {
            this.repository = repository;
            this.localizer = localizer;
        }


        /// <summary>
        /// Returns instance of Path inline editors view.
        /// </summary>
        /// <param name="properties">Properties of path editor.</param>
        public IViewComponentResult Invoke(PathEditorProperties properties)
        {
            var model = new PathEditorViewModel();
            InitializeEditorViewModel(properties.PageAliasPath, model);
            model.PropertyName = properties.PropertyName;

            return View("~/Components/InlineEditors/PathEditor/_PathEditor.cshtml", model);
        }


        private void InitializeEditorViewModel(string pageAliasPath, PathEditorViewModel viewModel)
        {
            if (pageAliasPath == null)
            {
                viewModel.PageSelectionState = PageSelectionState.NotSelected;
                viewModel.Value = localizer["No page selected"].Value;
                viewModel.Title = "";
                return;
            }

            var pageName = GetPageName(pageAliasPath);
            if (!string.IsNullOrEmpty(pageName))
            {
                viewModel.PageSelectionState = PageSelectionState.Valid;
                viewModel.Value = pageName;
                viewModel.Title = pageAliasPath;
                return;
            }

            viewModel.PageSelectionState = PageSelectionState.Inaccessible;
            viewModel.Value = localizer["Page does not exist"].Value;
            viewModel.Title = localizer["The selected page has been deleted or its alias path has been changed."].Value;
        }


        private string GetPageName(string pageAliasPath)
        {
            if (string.IsNullOrWhiteSpace(pageAliasPath))
            {
                return null;
            }
            return repository.GetPageName(pageAliasPath);
        }
    }
}
