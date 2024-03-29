﻿using System.Collections.Generic;
using System.Linq;

namespace Kentico.Xperience.ListingWidget.Widgets
{
    /// <summary>
    /// Provides strategy for selecting <see cref="ITransformationService"/>.
    /// </summary>
    internal class TransformationStrategy : ITransformationStrategy
    {
        private readonly IEnumerable<ITransformationService> transformationsServices;


        /// <summary>
        /// Creates an instance of <see cref="TransformationStrategy"/> class.
        /// </summary>
        /// <param name="transformationsServices">Transformation services for supported transformations.</param>
        public TransformationStrategy(IEnumerable<ITransformationService> transformationsServices)
        {
            this.transformationsServices = transformationsServices;
        }


        /// <inheritdoc/>
        public ITransformationService GetService(string transformationView)
        {
            if (string.IsNullOrEmpty(transformationView) || !IsTransformationSupported(transformationView))
            {
                return null;
            }
            return transformationsServices.Where(service => service.Transformations.Any(transformation => transformation.View == transformationView)).FirstOrDefault();
        }


        private bool IsTransformationSupported(string transformationPath)
        {
            return transformationsServices.Any(service => service.Transformations.Any(transformation => transformation.View == transformationPath));
        }
    }
}
