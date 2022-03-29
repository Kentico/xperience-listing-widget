using System;

using CMS.DocumentEngine.Types.DancingGoatCore;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.PageTemplates
{
    public class RelatedArticleViewModel
    {
        public string TeaserPath { get; set; }


        public string Title { get; set; }


        public DateTime PublicationDate { get; set; }


        public string Summary { get; set; }


        public bool HandleArticlePosition { get; set; }


        public string Url { get; set; }


        public static RelatedArticleViewModel GetViewModel(Article article, bool handleArticlePosition, IPageUrlRetriever pageUrlRetriever, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            return new RelatedArticleViewModel
            {
                Summary = article.Fields.Summary,
                TeaserPath = article.Fields.Teaser == null ? null : attachmentUrlRetriever.Retrieve(article.Fields.Teaser).WithSizeConstraint(SizeConstraint.Height(301)).RelativePath,
                Title = article.Fields.Title,
                PublicationDate = article.PublicationDate,
                HandleArticlePosition = handleArticlePosition,
                Url = pageUrlRetriever.Retrieve(article).RelativePath
            };
        }
    }
}