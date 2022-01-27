using System;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Class representing transformation.
    /// </summary>
    public class Transformation
    {
        /// <summary>
        /// Name of the transformation.
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Path of the transformation view.
        /// </summary>
        public string View { get; set; }


        /// <summary>
        /// Transformation tooltip to be shown in transformation selector.
        /// </summary>
        public string ToolTip { get; set; }


        /// <summary>
        /// Type of service providing view models for transformation.
        /// </summary>
        public Type ServiceType { get; set; }
    }
}
