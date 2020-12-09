using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc cref="IRaycastEntity" />
    public class RaycastBase : MonoBehaviour, IRaycastEntity
    {
        private const float MIN_DISTANCE = 0.01f;
        private const float MAX_DISTANCE = 100f;

        private Ray _cachedRay;
        private RaycastHit _cachedHitGizmos;
     
        #region SerializeField

        [Header("Raycast settings")]
        [Range(MIN_DISTANCE, MAX_DISTANCE)]
        [SerializeField] private float distance = 10f;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private bool isEnabled = true;

        [Header("Ray params")]
        [SerializeField] protected Transform origin;
        [SerializeField] protected Transform forward;

        #endregion
        
        public virtual bool IsValid()
        {
            return origin != null && forward != null;
        }

        protected virtual bool RaycastInternal(out RaycastHit hit, float distance, LayerMask layerMask)
        {
            _cachedRay.origin = origin.position;
            _cachedRay.direction = forward.forward;
            
            return Physics.Raycast(_cachedRay, out hit, distance, layerMask);
        }
        
        protected virtual void Validate()
        {
            if (origin == null) origin = transform;
            if (forward == null) forward = transform;
        }
        
        #region IRaycastEntity

        /// <inheritdoc />
        public bool IsEnabled
        {
            get => isEnabled;
            set => isEnabled = value;
        }

        /// <inheritdoc />
        public float Distance
        {
            get => distance;
            set => distance = Mathf.Clamp(value, MIN_DISTANCE, MAX_DISTANCE);
        }

        /// <inheritdoc />
        public LayerMask LayerMask
        {
            get => layerMask;
            set => layerMask = value;
        }

        /// <inheritdoc />
        public virtual bool Raycast(out RaycastHit hit)
        {
            hit = default;
            
            if (!IsEnabled) return false;
            
            return RaycastInternal(out hit, Distance, LayerMask);
        }

        #endregion
        
        #region MonoBehaviour

        protected virtual void Reset()
        {
            Distance = 10f;
            LayerMask = 1;
            IsEnabled = true;
            origin = null;
            forward = null;

            Validate();
        }
        
        protected virtual void OnValidate()
        {
            Validate();
        }

        protected virtual void OnDrawGizmosSelected()
        {
            if (!IsValid()) return;

            var pos = origin.position;

            if (Application.isPlaying)
            {
                var hasHit = Raycast(out _cachedHitGizmos);

                var color = hasHit ? Color.green : Color.red;
                Gizmos.color = color;
                Gizmos.DrawLine(pos, hasHit ? _cachedHitGizmos.point : (pos + forward.forward));

                color.a = 0.2f;
                Gizmos.color = color;
                Gizmos.DrawSphere(pos, 0.1f);
            }
            else
            {
                var color = Color.blue;
                Gizmos.color = color;
                Gizmos.DrawLine(pos, pos + forward.forward);

                color.a = 0.2f;
                Gizmos.color = color;
                Gizmos.DrawSphere(pos, 0.1f);
            }
        }

        #endregion
    }
}