using System;
using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine.Types.DancingGoatCore;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Class providing supported transformations.
    /// </summary>
    public class SupportedTransformations
    {
        /// <summary>
        /// Articles transformation.
        /// </summary>
        public readonly Transformation Articles =
            new Transformation
            {
                Name = "Articles",
                View = "Transformations/Articles/_Articles.cshtml",
                ToolTip = "Transformation displays articles in 4 column grid.",
                ServiceType = typeof(ArticlesTransformationService),
            };


        /// <summary>
        /// Articles transformation with heading.
        /// </summary>
        public readonly Transformation ArticlesWithHeading =
            new Transformation
            {
                Name = "Articles with heading",
                View = "Transformations/Articles/_ArticlesWithHeading.cshtml",
                ToolTip = "Transformation displays articles in 4 column grid with first large heading article.",
                ServiceType = typeof(ArticlesTransformationService),
            };


        /// <summary>
        /// Cafes transformation.
        /// </summary>
        public readonly Transformation Cafes =
            new Transformation
            {
                Name = "Our cafes",
                View = "Transformations/Cafes/_OurCafes.cshtml",
                ToolTip = "Transformation displays our cafes in 2 column grid.",
                ServiceType = typeof(CafesTransformationService),
            };


        /// <summary>
        /// Coffees transformation.
        /// </summary>
        public readonly Transformation Coffees =
            new Transformation
            {
                Name = "Coffees",
                View = "Transformations/Coffees/_Coffees.cshtml",
                ToolTip = "Transformation displays coffees in 4 column grid.",
                ServiceType = typeof(CoffeesTransformationService),
            };


        /// <summary>
        /// Supported transformations. Under each page type are supported transformations for this page type.
        /// </summary>
        public readonly IDictionary<string, IEnumerable<Transformation>> Transformations;


        /// <summary>
        /// Creates an instance of <see cref="SupportedTransformations"/> class.
        /// </summary>
        public SupportedTransformations()
        {
            Transformations = new Dictionary<string, IEnumerable<Transformation>>
            {
                { Article.CLASS_NAME, new List<Transformation>{ Articles, ArticlesWithHeading } },
                { Cafe.CLASS_NAME, new List<Transformation>{ Cafes } },
                { Coffee.CLASS_NAME, new List<Transformation>{ Coffees } },
            };
        }


        /// <summary>
        /// Checks if transformation is supported.
        /// </summary>
        /// <param name="transformationPath">Path of the transformation view.</param>
        public bool IsTransformationSupported(string transformationPath)
        {
            return Transformations.Any(pair => pair.Value.Any(transformation => transformation.View == transformationPath));
        }


        /// <summary>
        /// Checks if transformation is supported for selected page type.
        /// </summary>
        /// <param name="transformationPath">Path of the transformation view.</param>
        /// <param name="pageType">Page type.</param>
        public bool IsTransformationSupportedForPageType(string transformationPath, string pageType)
        {
            if (string.IsNullOrEmpty(pageType) || string.IsNullOrEmpty(transformationPath))
            {
                return false;
            }
            return Transformations[pageType].Any(transformation => transformation.View == transformationPath);
        }


        /// <summary>
        /// Gets type of <see cref="ITransformationService"/> for selected transformation.
        /// </summary>
        /// <param name="transformationPath">Path of the transformation view.</param>
        public Type GetServiceType(string transformationPath)
        {
            return Transformations.Values
                .Select(list => list
                    .Where(transformation => transformation.View == transformationPath)
                    .FirstOrDefault())
                .Where(transformation => transformation != null)
                .FirstOrDefault()
                .ServiceType;
        }
    }
}
