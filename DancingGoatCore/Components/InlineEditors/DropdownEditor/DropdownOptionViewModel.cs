namespace DancingGoat.InlineEditors
{
    /// <summary>
    /// View model for a drop-down option.
    /// </summary>
    public class DropdownOptionViewModel
    {
        /// <summary>
        /// Value of a drop-down option. 
        /// </summary>
        public string Value { get; set; }


        /// <summary>
        /// Text of a drop-down option. 
        /// </summary>
        public string Text { get; set; }


        /// <summary>
        /// Creates an instance of <see cref="DropdownOptionViewModel"/> class.
        /// </summary>
        /// <param name="value">Value of a drop-down option.</param>
        /// <param name="text">Text of a drop-down option.</param> 
        public DropdownOptionViewModel(string value, string text)
        {
            Value = value;
            Text = text;
        }
    }
}
