using Microsoft.AspNetCore.Mvc;

namespace Kentico.Xperience.ListingWidget.InlineEditors
{
    /// <summary>
    /// ViewComponent for path inline editor.
    /// </summary>
    public class PathEditorViewComponent : ViewComponent
    {
        private readonly IPageRepository repository;


        /// <summary>
        /// Creates an instance of <see cref="PathEditorViewComponent"/> class.
        /// </summary>
        /// <param name="repository">Page repository.</param>
        public PathEditorViewComponent(IPageRepository repository)
        {
            this.repository = repository;
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
                viewModel.Value = "No page selected";
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
            viewModel.Value = "Page does not exist";
            viewModel.Title = "The selected page has been deleted or its alias path has been changed.";
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
