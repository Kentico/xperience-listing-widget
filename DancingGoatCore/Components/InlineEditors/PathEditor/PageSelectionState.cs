namespace DancingGoat.InlineEditors
{
    /// <summary>
    /// Represents page selection state of <see cref="SelectedPage"/>.
    /// </summary>
    public enum PageSelectionState
    {
        /// <summary>
        /// Page is not acessible, user has insufficient permissions or the page has been deleted.
        /// </summary>
        Inacessible,


        /// <summary>
        /// Page is not selected.
        /// </summary>
        NotSelected,


        /// <summary>
        /// Page is selected and accessible.
        /// </summary>
        Valid
    }
}
