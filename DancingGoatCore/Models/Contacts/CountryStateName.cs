using System;

namespace DancingGoat.Models
{
    /// <summary>
    /// Represents a country code name with an optional state code name.
    /// </summary>
    public struct CountryStateName
    {
        private static readonly char[] mSeparators = new char[] { ';' };


        /// <summary>
        /// Gets the country code name.
        /// </summary>
        public string CountryName { get; private set; }


        /// <summary>
        /// Gets the state code name, if available; otherwise, null.
        /// </summary>
        public string StateName { get; private set; }


        /// <summary>
        /// Converts the string representation of a country and an optional state code name to its <see cref="CountryName"/> equivalent.
        /// </summary>
        /// <param name="compositeName">A string that contains a country and an optional state code name separated by a semicolon.</param>
        /// <returns>An object that is equivalent to the country and optional state code name contained in <paramref name="compositeName"/>.</returns>
        public static CountryStateName Parse(string compositeName)
        {
            if (compositeName == null)
            {
                throw new ArgumentNullException(nameof(compositeName));
            }

            var tokens = compositeName.Split(mSeparators, StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length == 0 || tokens.Length > 2)
            {
                throw new ArgumentException("The composite name must contain either country code name, or country and state code name separated by a semicolon.", compositeName);
            }

            var name = new CountryStateName
            {
                CountryName = tokens[0]
            };

            if (tokens.Length > 1)
            {
                name.StateName = tokens[1];
            }

            return name;
        }
    }
}