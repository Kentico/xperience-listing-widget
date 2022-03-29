using System;
using System.Collections.Generic;

using CMS.DocumentEngine;

using Kentico.Content.Web.Mvc;

namespace Kentico.Xperience.ListingWidget
{
    /// <summary>
    /// Provides methods to get transformation model and custom parametrization for page retriever.
    /// </summary>
    public abstract class BaseTransformationService : ITransformationService
    {
        /// <inheritdoc/>
        public abstract string PageType { get; }


        /// <inheritdoc/>
        public abstract IEnumerable<Transformation> Transformations { get; }


        /// <inheritdoc/>
        public abstract ITransformationViewModel GetModel(IEnumerable<TreeNode> pages);


        /// <inheritdoc/>
        public virtual Action<IPageCacheDependencyBuilder<TreeNode>, IEnumerable<TreeNode>> GetCustomCacheDependency(string transformationView)
        {
            return null;
        }


        /// <inheritdoc/>
        public virtual string GetCustomCacheDependencyKey(string transformationView)
        {
            return null;
        }


        /// <inheritdoc/>
        public virtual Action<DocumentQuery> GetCustomQueryParametrization(string transformationView)
        {
            return null;
        }
    }
}
