using System;
using UnityEngine;

namespace CucuTools
{
    public class RaycastEffectObserver : RaycastEffectBase
    {
        public bool HasHit => hasHit;
        public ObservedObject ObservedObject => observedObject;
        
        public override bool IsEnabled
        {
            get => base.IsEnabled;
            set
            {
                base.IsEnabled = value;
                if (IsEnabled) return;
                ResetObject();
            }
            
        }
        
        [Header("Observer")]
        [SerializeField] private bool hasHit;
        [SerializeField] private ObservedObject observedObject;

        private bool _gizmosAnimating;
        private float _gizmosTimer;
        private float _gizmosDuration = 0.5f;
        
        private RaycastHit _hitCached;
        
        public override void UpdateEffect()
        {
            Observe();
        }

        [CucuButton(group: "Observer")]
        private void Observe()
        {
            hasHit = Raycaster.Raycast(out _hitCached);

            if (hasHit)
            {
                observedObject.transform = _hitCached.transform;
                observedObject.point = _hitCached.point;
                observedObject.normal = _hitCached.normal;
            }
            else observedObject = default;

            _gizmosAnimating = true;
        }

        private void ResetObject()
        {
            hasHit = false;
            observedObject = default;
        }

        private void GizmosAnimate()
        {
            if (!_gizmosAnimating) return;
            
            if (ObservedObject.transform == null)
            {
                _gizmosAnimating = false;
                _gizmosTimer = 0f;
                return;
            }

            var t = _gizmosTimer / _gizmosDuration;

            var color = Color.cyan;
            color.a = 0.5f;
            Gizmos.color = color;
            Gizmos.DrawSphere(Vector3.Lerp(Raycaster.Origin.position, ObservedObject.point, t), 0.1f);

            _gizmosTimer += Time.deltaTime;

            if (!(_gizmosTimer > _gizmosDuration)) return;
            
            _gizmosAnimating = false;
            _gizmosTimer = 0f;
        }
        
        protected virtual void Awake()
        {
            ResetObject();
        }

        protected override void Reset()
        {
            base.Reset();
            ResetObject();
        }

        protected virtual void OnDrawGizmos()
        {
            GizmosAnimate();
        }
    }

    [Serializable]
    public struct ObservedObject
    {
        public Transform transform;
        public Vector3 point;
        public Vector3 normal;

        public bool Exist() => transform != null;
    }
}