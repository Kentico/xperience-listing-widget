using System.Collections.Generic;
using System.Linq;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Retriever of supported page types for listing widget.
    /// </summary>
    public class SupportedPageTypesRetriever
    {
        private IEnumerable<ITransformationService> transformationsServices;


        /// <summary>
        /// Creates an instance of <see cref="SupportedTransformationsRetriever"/> class.
        /// </summary>
        /// <param name="transformationsServices">Transformation services for supported transformations.</param>
        public SupportedPageTypesRetriever(IEnumerable<ITransformationService> transformationsServices)
        {
            this.transformationsServices = transformationsServices;
        }


        /// <summary>
        /// Gets supported page types.
        /// </summary>
        public IEnumerable<string> GetSupportedPageTypes()
        {
            return transformationsServices.Select(service => service.PageType);
        }
    }
}
