using System.Linq;
using UnityEngine;

namespace CucuTools.Interactables
{
    /// <inheritdoc />
    [DisallowMultipleComponent]
    public sealed class InteractableAgent : InteractableEntity
    {
        /// <inheritdoc />
        public override bool IsEnabled
        {
            get => Target?.IsEnabled ?? false;
            set
            {
                if (Target != null) Target.IsEnabled = value;
            }
        }

        /// <summary>
        /// Target of interactable agent
        /// </summary>
        public InteractableEntity Target
        {
            get => target;
            set => target = value;
        }

        [SerializeField] private InteractableEntity target;

        /// <inheritdoc />
        public override void Normal()
        {
            Target?.Normal();
        }

        /// <inheritdoc />
        public override void Hover()
        {
            Target?.Hover();
        }

        /// <inheritdoc />
        public override void Press()
        {
            Target?.Press();
        }

        private void Validate()
        {
            if (Target == this) Target = null;
            if (Target == null) Target = GetComponentsInChildren<InteractableEntity>().FirstOrDefault(ib => ib != this);
        }

        private void Awake()
        {
            Validate();
        }

        private void OnValidate()
        {
            Validate();
        }
    }
}