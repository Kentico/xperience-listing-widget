using System.Collections.Generic;
using System.Linq;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Provides strategy for selecting <see cref="ITransformationService"/>.
    /// </summary>
    public class TransformationStrategy : ITransformationStrategy
    {
        private IEnumerable<ITransformationService> transformationsServices;
        private readonly SupportedTransformations supportedTransformations;


        /// <summary>
        /// Creates an instance of <see cref="TransformationStrategy"/> class.
        /// </summary>
        /// <param name="transformationsServices">Transformation services for supported transformations.</param>
        /// <param name="supportedTransformations">Supported transformations.</param>
        public TransformationStrategy(IEnumerable<ITransformationService> transformationsServices, SupportedTransformations supportedTransformations)
        {
            this.transformationsServices = transformationsServices;
            this.supportedTransformations = supportedTransformations;
        }


        /// <inheritdoc/>
        public ITransformationService GetService(string transformationView)
        {
            if (string.IsNullOrEmpty(transformationView) || !supportedTransformations.IsTransformationSupported(transformationView))
            {
                return null;
            }
            var transformationType = supportedTransformations.GetServiceType(transformationView);

            return transformationsServices.Where(service => transformationType.IsAssignableFrom(service.GetType())).FirstOrDefault();
        }
    }
}
