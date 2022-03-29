namespace Kentico.Xperience.ListingWidget.InlineEditors
{
    /// <summary>
    /// Represents page selection state of <see cref="SelectedPage"/>.
    /// </summary>
    public enum PageSelectionState
    {
        /// <summary>
        /// Page is not accessible. The selected page has been deleted or its alias path has been changed.
        /// </summary>
        Inaccessible,


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
