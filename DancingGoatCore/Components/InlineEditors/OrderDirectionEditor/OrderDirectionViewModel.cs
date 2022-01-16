using System.Collections.Generic;

using CMS.DataEngine;

namespace DancingGoat.InlineEditors
{
    /// <summary>
    /// View model for order direction editor.
    /// </summary>
    public class OrderDirectionViewModel : InlineEditorViewModel
    {
        /// <summary>
        /// Order direction.
        /// </summary>
        public OrderDirection Direction { get; set; }


        /// <summary>
        /// Collection of supported order directions.
        /// </summary>
        public IEnumerable<OrderDirection> OrderDirectionOptions = new List<OrderDirection> { OrderDirection.Ascending, OrderDirection.Descending };
    }
}
