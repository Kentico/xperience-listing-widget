﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Kentico.Xperience.ListingWidget.InlineEditors
{
    /// <summary>
    /// ViewComponent for path inline editor.
    /// </summary>
    public class PathEditorViewComponent : ViewComponent
    {
        private readonly IPageRepository repository;
        private readonly IStringLocalizer<ListingWidgetResources> localizer;


        /// <summary>
        /// Creates an instance of <see cref="PathEditorViewComponent"/> class.
        /// </summary>
        /// <param name="repository">Page repository.</param>
        /// <param name="localizer">Represents an <see cref="IStringLocalizer"/> that provides localized strings.</param>
        public PathEditorViewComponent(IPageRepository repository, IStringLocalizer<ListingWidgetResources> localizer)
        {
            this.repository = repository;
            this.localizer = localizer;
        }


        /// <summary>
        /// Returns instance of Path inline editors view.
        /// </summary>
        /// <param name="propertyName">Name of property edited by inline editor.</param>
        /// <param name="pageAliasPath">Parent page alias path.</param>
        public IViewComponentResult Invoke(string propertyName, string pageAliasPath)
        {
            var model = new PathEditorViewModel();
            InitializeEditorViewModel(pageAliasPath, model);
            model.PropertyName = propertyName;

            return View("~/Components/InlineEditors/PathEditor/_PathEditor.cshtml", model);
        }


        private void InitializeEditorViewModel(string pageAliasPath, PathEditorViewModel viewModel)
        {
            if (string.IsNullOrEmpty(pageAliasPath))
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