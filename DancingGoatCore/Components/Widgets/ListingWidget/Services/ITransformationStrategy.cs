namespace DancingGoat.Widgets
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