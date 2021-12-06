using System;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// View model for one item of Listing widget.
    /// </summary>
    public class ListingWidgetBasePageViewModel
    {
        /// <summary>
        /// Document name to show.
        /// </summary>
        public string DocumentName { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="ListingWidgetBasePageViewModel"/> class. 
        /// </summary>
        /// <param name="DocumentName">Document name to show.</param>
        public ListingWidgetBasePageViewModel(string DocumentName) 
        { 
            if (DocumentName == null)
            {
                throw new ArgumentNullException(nameof(DocumentName), $"Argument {nameof(DocumentName)} cannot be null.");
            }
            this.DocumentName = DocumentName; 
        }
    }
}
