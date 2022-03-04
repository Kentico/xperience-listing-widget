using System;

using CMS.DocumentEngine;
using CMS.Tests;

using Kentico.Content.Web.Mvc;

using NSubstitute;

using NUnit.Framework;

namespace Kentico.Xperience.ListingWidget.Tests
{
    [TestFixture]
    public class PageRepositoryTests : UnitTests
    {
        private IPageRetriever retriever;
        private PageRepository repository;


        [SetUp]
        public void SetUp()
        {
            retriever = Substitute.For<IPageRetriever>();
            repository = new PageRepository(retriever);
        }


        [Test]
        public void GetPageName_ValidPath_CallsRetrieverWithCorrectType()
        {
            repository.GetPageName("path");

            retriever.Received(1).Retrieve(Arg.Any<Action<DocumentQuery<TreeNode>>>(), Arg.Any<Action<IPageCacheBuilder<TreeNode>>>());
        }


        [Test]
        public void GetPages_CallsRetrieverWithCorrectClassName()
        {
            var className = "Test.ClassName";
            repository.GetPages(className);

            retriever.Received(1).Retrieve(className, Arg.Any<Action<DocumentQuery>>(), Arg.Any<Action<IPageCacheBuilder<TreeNode>>>());
        }
    }
}
