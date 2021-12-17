namespace DancingGoat.InlineEditors
{
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


        public DropdownOptionViewModel(string value, string text)
        {
            Value = value;
            Text = text;
        }
    }
}
