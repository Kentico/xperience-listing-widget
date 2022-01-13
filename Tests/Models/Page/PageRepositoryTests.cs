using System;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;
using CMS.Tests;

using Kentico.Content.Web.Mvc;

using NSubstitute;
using NUnit.Framework;

namespace DancingGoat.Models
{
    [TestFixture(typeof(Article))]
    [TestFixture(typeof(TreeNode))]
    [TestFixture(typeof(Coffee))]
    [TestFixture(typeof(Cafe))]
    [TestFixture(typeof(Brewer))]
    [TestFixture(typeof(Manufacturer))]
    public class PageRepositoryTests<T> : UnitTests where T : TreeNode, new()
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
        public void GetPage_ValidPath_CallsRetrieverWithCorrectType()
        {
            repository.GetPage<T>("path");

            retriever.Received(1).Retrieve(Arg.Any<Action<DocumentQuery<T>>>(), Arg.Any<Action<IPageCacheBuilder<T>>>());
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
