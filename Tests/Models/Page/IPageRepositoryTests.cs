using System;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.DancingGoatCore;
using CMS.Tests;

using Kentico.Content.Web.Mvc;

using NSubstitute;
using NUnit.Framework;

using DancingGoat.Models;

namespace DancingGoatCore.Models
{
    [TestFixture(typeof(Article))]
    [TestFixture(typeof(TreeNode))]
    [TestFixture(typeof(Coffee))]
    [TestFixture(typeof(Cafe))]
    [TestFixture(typeof(Brewer))]
    [TestFixture(typeof(Manufacturer))]
    public class IPageRepositoryTests<T> : UnitTests where T : TreeNode, new()
    {
        private IPageRetriever retriever;
        private IPageRepository repository;


        [SetUp]
        public void SetUp()
        {
            retriever = Substitute.For<IPageRetriever>();
            repository = Substitute.ForPartsOf<PageRepository>(retriever);
        }


        [Test]
        public void GetAllPages_Generic_CallsRetrieverWithCorrectType() 
        {
            repository.GetAllPages<T>();

            retriever.Received(1).Retrieve(Arg.Any<Action<DocumentQuery<T>>>(), Arg.Any<Action<IPageCacheBuilder<T>>>());
        }
    }
}
