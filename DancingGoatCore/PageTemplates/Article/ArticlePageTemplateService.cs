using CMS.DocumentEngine.Types.DancingGoatCore;

using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;

[assembly: RegisterPageTemplate("DancingGoat.Article", "Article detail", customViewName: "~/PageTemplates/Article/_Article.cshtml", Description = "Displays an article detail with related articles underneath.", IconClass = "icon-l-text")]

namespace DancingGoat.PageTemplates
{
    public class ArticlePageTemplateService
    {
        private readonly IPageDataContextRetriever pageDataContextRetriver;
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly IPageAttachmentUrlRetriever attachmentUrlRetriever;


        public ArticlePageTemplateService(IPageDataContextRetriever pageDataContextRetriver, IPageUrlRetriever pageUrlRetriever, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            this.pageDataContextRetriver = pageDataContextRetriver;
            this.pageUrlRetriever = pageUrlRetriever;
            this.attachmentUrlRetriever = attachmentUrlRetriever;
        }


        public ArticleViewModel GetTemplateModel()
        {
            var article = pageDataContextRetriver.Retrieve<Article>().Page;
            return ArticleViewModel.GetViewModel(article, pageUrlRetriever, attachmentUrlRetriever);
        }
    }
}