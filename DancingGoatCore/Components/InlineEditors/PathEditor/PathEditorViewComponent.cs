﻿using CMS.DocumentEngine;

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
        /// <param name="propertyName">Name of property edited by inline editor.</param>
        /// <param name="page">Selected page.</param>
        public IViewComponentResult Invoke(string propertyName, SelectedPage page)
        {
            var model = new PathEditorViewModel();
            InitializeEditorViewModel(page, model);
            model.PropertyName = propertyName;

            return View("~/Components/InlineEditors/PathEditor/_PathEditor.cshtml", model);
        }


        private void InitializeEditorViewModel(SelectedPage page, PathEditorViewModel viewModel)
        {
            if (page == null)
            {
                viewModel.PageSelectionState = PageSelectionState.NotSelected;
                viewModel.Value = localizer["No page selected"].Value;
                viewModel.Title = "";
            }
            else if (IsPageValid(page.Path))
            {
                viewModel.PageSelectionState = PageSelectionState.Valid;
                viewModel.Value = page.Name;
                viewModel.Title = page.Path;
            }
            else
            {
                viewModel.PageSelectionState = PageSelectionState.Inacessible;
                viewModel.Value = localizer["Invalid page"].Value;
                viewModel.Title = localizer["The selected page has been deleted or you don't have permissions to select it."].Value;
            }
        }


        private bool IsPageValid(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }
            var page = repository.GetPage<TreeNode>(path);
            return page != null;
        }
    }
}