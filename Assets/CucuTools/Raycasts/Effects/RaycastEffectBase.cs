using UnityEngine;

namespace CucuTools.Raycasts.Effects
{
    /// <inheritdoc cref="IRaycastEffect" />
    public abstract class RaycastEffectBase : MonoBehaviour, IRaycastEffect
    {
        #region SerializeField

        [Header("Base settings")]
        [SerializeField] private bool isEnabled = true;
        [SerializeField] private RaycastBase raycastBase;

        #endregion

        #region IRaycastEffect

        /// <inheritdoc />
        public virtual bool IsEnabled
        {
            get => isEnabled;
            set => isEnabled = value;
        }

        /// <inheritdoc />
        public virtual IRaycastEntity Raycaster => raycastBase;

        /// <inheritdoc />
        public abstract void UpdateEffect();

        #endregion
        
        #region Virtual API

        protected virtual void Validate()
        {
            if (raycastBase == null) raycastBase = GetComponent<RaycastBase>();
            if (raycastBase == null) raycastBase = transform.parent?.GetComponent<RaycastBase>();
        }
        
        protected virtual void Update()
        {
            if (IsEnabled) UpdateEffect();
        }

        protected virtual void Reset()
        {
            IsEnabled = true;
            raycastBase = null;
            
            Validate();
        }

        protected virtual void OnValidate()
        {
            Validate();
        }

        #endregion
    }
}