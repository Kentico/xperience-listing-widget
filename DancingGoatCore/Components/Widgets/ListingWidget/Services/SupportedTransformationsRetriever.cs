using System.Collections.Generic;
using System.Linq;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Provides methods to get supported transformations.
    /// </summary>
    public class SupportedTransformationsRetriever
    {
        private IEnumerable<ITransformationService> transformationsServices;


        /// <summary>
        /// Creates an instance of <see cref="SupportedTransformationsRetriever"/> class.
        /// </summary>
        /// <param name="transformationsServices">Transformation services for supported transformations.</param>
        public SupportedTransformationsRetriever(IEnumerable<ITransformationService> transformationsServices)
        {
            this.transformationsServices = transformationsServices;
        }


        /// <summary>
        /// Checks if transformation is supported for selected page type.
        /// </summary>
        /// <param name="transformationPath">Path of the transformation view.</param>
        /// <param name="pageType">Page type.</param>
        public bool IsTransformationSupported(string transformationPath, string pageType)
        {
            if (string.IsNullOrEmpty(pageType) || string.IsNullOrEmpty(transformationPath))
            {
                return false;
            }
            return GetTransformations(pageType).Any(transformation => transformation.View == transformationPath);
        }


        /// <summary>
        /// Gets supported transformations for page type.
        /// </summary>
        /// <param name="pageType">Page type.</param>
        public IEnumerable<Transformation> GetTransformations(string pageType)
        {
            return transformationsServices.FirstOrDefault(service => service.PageType == pageType)?.Transformations ?? Enumerable.Empty<Transformation>();
        }
    }
}
