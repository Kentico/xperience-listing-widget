using Kentico.Xperience.ListingWidget.Transformations;

namespace Kentico.Xperience.ListingWidget.Services
{
    /// <summary>
    /// Interface for strategy for selecting <see cref="ITransformationService"/>.
    /// </summary>
    public interface ITransformationStrategy
    {
        /// <summary>
        /// Gets <see cref="ITransformationService"/> for specified transformation.
        /// </summary>
        /// <param name="transformationView">Transformation view path.</param>
        ITransformationService GetService(string transformationView);
    }
}