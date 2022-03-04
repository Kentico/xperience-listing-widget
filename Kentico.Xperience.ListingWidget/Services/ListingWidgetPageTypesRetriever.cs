using System.Collections.Generic;
using System.Linq;

using Kentico.Xperience.ListingWidget.Transformations;

namespace Kentico.Xperience.ListingWidget.Services
{
    /// <summary>
    /// Provides a method for retrieving supported page types for listing widget.
    /// </summary>
    public class ListingWidgetPageTypesRetriever
    {
        private readonly IEnumerable<ITransformationService> transformationsServices;


        /// <summary>
        /// Creates an instance of <see cref="ListingWidgetPageTypesRetriever"/> class.
        /// </summary>
        /// <param name="transformationsServices">Transformation services for supported transformations.</param>
        public ListingWidgetPageTypesRetriever(IEnumerable<ITransformationService> transformationsServices)
        {
            this.transformationsServices = transformationsServices;
        }


        /// <summary>
        /// Retrieves supported page types.
        /// </summary>
        public IEnumerable<string> Retrieve()
        {
            return transformationsServices.Select(service => service.PageType);
        }
    }
}
