﻿using System.Collections.Generic;
using System.Linq;

namespace Kentico.Xperience.ListingWidget
{
    /// <summary>
    /// Provides methods for retrieving supported transformations for listing widget.
    /// </summary>
    public class ListingWidgetTransformationsRetriever
    {
        private readonly IEnumerable<ITransformationService> transformationsServices;


        /// <summary>
        /// Creates an instance of <see cref="ListingWidgetTransformationsRetriever"/> class.
        /// </summary>
        /// <param name="transformationsServices">Transformation services of supported transformations.</param>
        public ListingWidgetTransformationsRetriever(IEnumerable<ITransformationService> transformationsServices)
        {
            this.transformationsServices = transformationsServices;
        }


        /// <summary>
        /// Checks if transformation is supported for the page type.
        /// </summary>
        /// <param name="transformationPath">Path of the transformation view.</param>
        /// <param name="pageType">Page type.</param>
        public bool IsSupported(string transformationPath, string pageType)
        {
            if (string.IsNullOrEmpty(pageType) || string.IsNullOrEmpty(transformationPath))
            {
                return false;
            }
            return Retrieve(pageType).Any(transformation => transformation.View == transformationPath);
        }


        /// <summary>
        /// Checks if the transformation path corresponds to any <see cref="Transformation"> provided by any implementations of <see cref="ITransformationService"/>. 
        /// </summary>
        /// <param name="transformationPath">Path of the transformation view.</param>
        public bool IsRegistered(string transformationPath)
        {
            return transformationsServices.Select(service => service.Transformations)
                .Any(transformations => transformations.Any(transformation => transformation.View == transformationPath));
        }


        /// <summary>
        /// Retrieves supported transformations for the page type.
        /// </summary>
        /// <param name="pageType">Page type.</param>
        public IEnumerable<Transformation> Retrieve(string pageType)
        {
            return transformationsServices.FirstOrDefault(service => service.PageType == pageType)?.Transformations ?? Enumerable.Empty<Transformation>();
        }
    }
}
