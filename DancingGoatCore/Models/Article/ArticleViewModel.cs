using System;
using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine.Types.DancingGoatCore;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models
{
    public class ArticleViewModel
    {
        public IPageAttachmentUrl TeaserUrl{ get; set; }


        public string Title { get; set; }


        public DateTime PublicationDate { get; set; }


        public string Summary { get; set; }


        public string Text { get; set; }


        public IEnumerable<ArticleViewModel> RelatedArticles { get; set; }


        public string Url { get; set; }

        public static ArticleViewModel GetViewModel(Article article, IPageUrlRetriever pageUrlRetriever, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            return new ArticleViewModel
            {
                PublicationDate = article.PublicationDate,
                RelatedArticles = article.Fields.RelatedArticles.OfType<Article>().Select(relatedArticle => GetViewModel(relatedArticle, pageUrlRetriever, attachmentUrlRetriever)) ?? Enumerable.Empty<ArticleViewModel>(),
                Summary = article.Fields.Summary,
                TeaserUrl = article.Fields.Teaser == null ? null : attachmentUrlRetriever.Retrieve(article.Fields.Teaser),
                Text = article.Fields.Text,
                Title = article.Fields.Title,
                Url = pageUrlRetriever.Retrieve(article).RelativePath
            };
        }
    }
}