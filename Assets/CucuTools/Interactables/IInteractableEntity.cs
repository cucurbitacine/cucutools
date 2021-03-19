namespace CucuTools.Interactables
{
    /// <summary>
    /// Interactable entity.
    /// Which have four states, Normal/Hovered/Pressed/Disabled
    /// </summary>
    public interface IInteractableEntity
    {
        /// <summary>
        /// Enabled state
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Set normal state
        /// </summary>
        void Normal();
        
        /// <summary>
        /// Set hovered state
        /// </summary>
        void Hover();
        
        /// <summary>
        /// Set clicked state
        /// </summary>
        void Press();
    }
}