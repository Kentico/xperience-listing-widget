using System;
using System.Collections.Generic;

using CMS.DocumentEngine;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Interface for service providing methods to get transformation model and custom parametrization for page retriever.
    /// </summary>
    public interface ITransformationService
    {
        /// <summary>
        /// Returns hydrated <see cref="ITransformationViewModel"/> for pages.
        /// </summary>
        /// <param name="pages">Pages to be listed in view model.</param>
        public ITransformationViewModel GetModel(IEnumerable<TreeNode> pages);


        /// <summary>
        /// Returns Action for applying custom cache dependency for page retriever.
        /// </summary>
        /// <param name="transformationView">Transformation view path.</param>
        public Action<IPageCacheDependencyBuilder<TreeNode>, IEnumerable<TreeNode>> GetCustomCacheDependency(string transformationView);


        /// <summary>
        /// Returns custom cache dependency key for page retriever.
        /// </summary>
        /// <param name="transformationView">Transformation view path.</param>
        public string GetCustomCacheDependencyKey(string transformationView);


        /// <summary>
        /// Returns Action for applying custom query parameters for page retriever.
        /// </summary>
        /// <param name="transformationView">Transformation view path.</param>
        public Action<DocumentQuery> GetCustomQueryParametrization(string transformationView);
    }
}
