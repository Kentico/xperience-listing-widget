using System;
using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine.Types.DancingGoatCore;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.PageTemplates
{
    public class ArticleWithSideBarViewModel
    {
        public string TeaserPath { get; set; }


        public string Title { get; set; }


        public DateTime PublicationDate { get; set; }


        public string Text { get; set; }


        public IEnumerable<RelatedArticleViewModel> RelatedArticles { get; set; }


        public ArticleSidebarLocationEnum SidebarLocation { get; set; }


        public string ArticleWidth { get; set; }


        public static ArticleWithSideBarViewModel GetViewModel(Article article, ArticleWithSideBarProperties templateProperties, IPageUrlRetriever pageUrlRetriever, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            return new ArticleWithSideBarViewModel
            {
                TeaserPath = article.Fields.Teaser == null ? null : attachmentUrlRetriever.Retrieve(article.Fields.Teaser).RelativePath,
                PublicationDate = article.PublicationDate,
                RelatedArticles = article.Fields.RelatedArticles.OfType<Article>().Select(relatedArticle => RelatedArticleViewModel.GetViewModel(relatedArticle, false, pageUrlRetriever, attachmentUrlRetriever)),
                Text = article.Fields.Text,
                Title = article.Fields.Title,
                SidebarLocation = (ArticleSidebarLocationEnum)Enum.Parse(typeof(ArticleSidebarLocationEnum), templateProperties.SidebarLocation, true),
                ArticleWidth = templateProperties.ArticleWidth,
            };
        }
    }
}