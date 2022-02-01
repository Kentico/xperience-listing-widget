using System.Collections.Generic;
using System.Linq;

namespace DancingGoat.Widgets
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
        /// Retrieves supported transformations for the page type.
        /// </summary>
        /// <param name="pageType">Page type.</param>
        public IEnumerable<Transformation> Retrieve(string pageType)
        {
            return transformationsServices.FirstOrDefault(service => service.PageType == pageType)?.Transformations ?? Enumerable.Empty<Transformation>();
        }
    }
}
