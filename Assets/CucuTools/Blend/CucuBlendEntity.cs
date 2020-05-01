using UnityEngine;
using UnityEngine.Events;

namespace CucuTools
{
    /// <inheritdoc />
    public abstract class CucuBlendEntity : MonoBehaviour, IBlendable
    {
        public abstract string Key { get; }

        /// <summary>
        /// Blend value was changed
        /// </summary>
        public UnityEvent OnValueChanged { get; } = new UnityEvent();

        /// <inheritdoc />
        public float Blend
        {
            get => _blend;
            private set => _blend = value;
        }

        /// <summary>
        /// Blend value
        /// </summary>
        [Header("Blend value")]
        [Range(0f, 1f)]
        [SerializeField]
        private float _blend;

        /// <inheritdoc />
        public void SetBlend(float blend)
        {
            Blend = blend;
            UpdateEntity();
            OnValueChanged.Invoke();
        }

        /// <summary>
        /// Update Entity after set new blend value 
        /// </summary>
        protected abstract void UpdateEntity();

        protected virtual void OnValidate()
        {
            UpdateEntity();
        }
    }
}