using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;

using DancingGoat.Models;

using Kentico.Content.Web.Mvc;

using Kentico.Xperience.ListingWidget.Widgets;

namespace DancingGoat.Widgets
{
    /// <inheritdoc/>
    public class ArticlesTransformationService : BaseTransformationService
    {
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly IPageAttachmentUrlRetriever attachmentUrlRetriever;


        /// <inheritdoc/>
        public override string PageType { get; } = Article.CLASS_NAME;


        /// <inheritdoc/>
        public override IEnumerable<Transformation> Transformations { get; } = new List<Transformation>
        {
            new Transformation
            {
                Name = "Articles",
                View = "Transformations/Articles/_Articles.cshtml",
                Description = "Transformation displays articles in 4 column grid.",
            },
            new Transformation
            {
                Name = "Articles with heading",
                View = "Transformations/Articles/_ArticlesWithHeading.cshtml",
                Description = "Transformation displays articles in 4 column grid with first large heading article.",
            }
        };


        /// <summary>
        /// Creates an instance of <see cref="ArticlesTransformationService"/> class.
        /// </summary>
        /// <param name="pageUrlRetriever">Page URL retriever.</param>
        /// <param name="attachmentUrlRetriever">Attachment URL retriever.</param>
        public ArticlesTransformationService(IPageUrlRetriever pageUrlRetriever, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            this.pageUrlRetriever = pageUrlRetriever;
            this.attachmentUrlRetriever = attachmentUrlRetriever;
        }


        /// <summary>
        /// Returns hydrated <see cref="ArticlesTransformationViewModel"/> for pages.
        /// </summary>
        /// <param name="pages">Pages to be listed in view model.</param>
        public override ITransformationViewModel GetModel(IEnumerable<TreeNode> pages)
        {
            if (pages == null)
            {
                return new ArticlesTransformationViewModel();
            }
            var articlesList = pages.Select(article => ArticleViewModel.GetViewModel(article as Article, pageUrlRetriever, attachmentUrlRetriever));
            return new ArticlesTransformationViewModel { Articles = articlesList };
        }
    }
}
