using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;

using DancingGoat.Models;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Provides methods to get articles transformation model and custom parametrization for page retriever.
    /// </summary>
    public class ArticlesTransformationService : BaseTransformationService
    {
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly IPageAttachmentUrlRetriever attachmentUrlRetriever;


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
                return new ArticlesTransformationViewModel { Articles = Enumerable.Empty<ArticleViewModel>() };
            }
            var articlesList = pages.Select(article => ArticleViewModel.GetViewModel(article as Article, pageUrlRetriever, attachmentUrlRetriever));
            return new ArticlesTransformationViewModel { Articles = articlesList };
        }
    }
}
