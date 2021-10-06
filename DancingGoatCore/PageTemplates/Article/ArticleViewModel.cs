using System;
using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine.Types.DancingGoatCore;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.PageTemplates
{
    public class ArticleViewModel
    {
        public string TeaserPath { get; set; }


        public string Title { get; set; }


        public DateTime PublicationDate { get; set; }


        public string Text { get; set; }


        public IEnumerable<RelatedArticleViewModel> RelatedArticles { get; set; }


        public static ArticleViewModel GetViewModel(Article article, IPageUrlRetriever pageUrlRetriever, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            return new ArticleViewModel
            {
                PublicationDate = article.PublicationDate,
                RelatedArticles = article.Fields.RelatedArticles.OfType<Article>().Select(relatedArticle => RelatedArticleViewModel.GetViewModel(relatedArticle, true, pageUrlRetriever, attachmentUrlRetriever)),
                TeaserPath = article.Fields.Teaser == null ? null : attachmentUrlRetriever.Retrieve(article.Fields.Teaser).RelativePath,
                Text = article.Fields.Text,
                Title = article.Fields.Title
            };
        }
    }
}