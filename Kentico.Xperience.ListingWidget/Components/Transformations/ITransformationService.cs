using System;
using System.Collections.Generic;

using CMS.DocumentEngine;

using Kentico.Content.Web.Mvc;

namespace Kentico.Xperience.ListingWidget.Transformations
{
    /// <summary>
    /// Interface for service providing methods to get transformation model and custom parametrization for page retriever.
    /// </summary>
    public interface ITransformationService
    {
        /// <summary>
        /// Supported page type for transformations.
        /// </summary>
        string PageType { get; }


        /// <summary>
        /// Supported transformations.
        /// </summary>
        IEnumerable<Transformation> Transformations { get; }


        /// <summary>
        /// Returns hydrated <see cref="ITransformationViewModel"/> with pages for corresponding views.
        /// </summary>
        /// <param name="pages">Pages to be used in view model.</param>
        ITransformationViewModel GetModel(IEnumerable<TreeNode> pages);


        /// <summary>
        /// Returns Action for applying custom cache dependency for page retriever to retrieve pages for corresponding view.
        /// </summary>
        /// <param name="transformationView">Transformation view path.</param>
        Action<IPageCacheDependencyBuilder<TreeNode>, IEnumerable<TreeNode>> GetCustomCacheDependency(string transformationView);


        /// <summary>
        /// Returns custom cache dependency key for page retriever to retrieve pages for corresponding view.
        /// </summary>
        /// <param name="transformationView">Transformation view path.</param>
        string GetCustomCacheDependencyKey(string transformationView);


        /// <summary>
        /// Returns Action for applying custom query parameters for page retriever to retrieve pages for corresponding view.
        /// </summary>
        /// <param name="transformationView">Transformation view path.</param>
        Action<DocumentQuery> GetCustomQueryParametrization(string transformationView);
    }
}
