using UnityEngine;

namespace CucuTools.Interactables
{
    /// <inheritdoc cref="CucuTools.Interactables.IInteractableEntity" />
    public abstract class InteractableEntity : MonoBehaviour, IInteractableEntity
    {
        /// <inheritdoc />
        public abstract bool IsEnabled { get; set; }

        /// <inheritdoc />
        public abstract void Normal();

        /// <inheritdoc />
        public abstract void Hover();

        /// <inheritdoc />
        public abstract void Press();
    }
}