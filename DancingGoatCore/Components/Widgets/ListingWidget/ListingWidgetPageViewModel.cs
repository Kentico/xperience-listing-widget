using System;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// View model for one item of Listing widget.
    /// </summary>
    public class ListingWidgetPageViewModel
    {
        /// <summary>
        /// Document name to show.
        /// </summary>
        public string DocumentName { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="ListingWidgetPageViewModel"/> class. 
        /// </summary>
        /// <param name="documentName">Document name to show.</param>
        public ListingWidgetPageViewModel(string documentName) 
        { 
            if (documentName == null)
            {
                throw new ArgumentNullException(nameof(documentName), $"Argument {nameof(documentName)} cannot be null.");
            }
            this.DocumentName = documentName; 
        }
    }
}
