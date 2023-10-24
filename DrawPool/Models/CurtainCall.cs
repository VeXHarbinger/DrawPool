namespace DrawPool.Models
{
    /// <summary>
    /// Helper for Hiding and showing internal display controls
    /// </summary>
    public class CurtainCall
    {
        /// <summary>
        /// Gets or sets the calling view, that is sending the data.
        /// </summary>
        /// <value>The calling view.</value>
        public ViewModes CallingView { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [should show] the window or hide it.
        /// </summary>
        /// <value><c>true</c> if [should show]; the window otherwise, <c>false</c> hide it.</value>
        public bool ShouldShow { get; set; } = false;
    }
}