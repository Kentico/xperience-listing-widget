﻿namespace DancingGoat.Widgets
{
    /// <summary>
    /// Provides transformations for tests.
    /// </summary>
    internal static class TransformationsMock
    {
        /// <summary>
        /// Articles transformation.
        /// </summary>
        public static readonly Transformation Articles =
            new Transformation
            {
                Name = "Articles",
                View = "Transformations/Articles/_Articles.cshtml",
                Description = "Transformation displays articles in 4 column grid.",
            };


        /// <summary>
        /// Articles transformation with heading.
        /// </summary>
        public static readonly Transformation ArticlesWithHeading =
            new Transformation
            {
                Name = "Articles with heading",
                View = "Transformations/Articles/_ArticlesWithHeading.cshtml",
                Description = "Transformation displays articles in 4 column grid with first large heading article.",
            };


        /// <summary>
        /// Cafes transformation with heading.
        /// </summary>
        public static readonly Transformation Cafes =
            new Transformation
            {
                Name = "Our cafes",
                View = "Transformations/Cafes/_OurCafes.cshtml",
                Description = "Transformation displays our cafes in 2 column grid.",
            };


        /// <summary>
        /// Coffees transformation with heading.
        /// </summary>
        public static readonly Transformation Coffees =
            new Transformation
            {
                Name = "Coffees",
                View = "Transformations/Coffees/_Coffees.cshtml",
                Description = "Transformation displays coffees in 4 column grid.",
            };
    }
}