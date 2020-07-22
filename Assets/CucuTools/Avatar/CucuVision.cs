using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools
{
    public class CucuVision : MonoBehaviour
    {
        public static CucuVision Instance { get; private set; }
        
        public bool SeeAnything => target.transform != null;
        
        public bool Active
        {
            get => active;
            set => active = value;
        }

        public float MaxDistance
        {
            get => maxDistance;
            set => maxDistance = value;
        }
        
        public Transform Root
        {
            get => root;
            set => root = value;
        }
        
        public LayerMask LayerMaskVision
        {
            get => layerMaskVision;
            set => layerMaskVision = value;
        }
        
        public UnityEvent<Transform, Transform> OnTargetChanged { get; } = new UnityDoubleTransformEvent();

        public VisionInfo Target => target;
        
        [Space]
        [SerializeField] private bool active = true;

        [Header("Settings")]
        [SerializeField] private bool isSingleton = true;
        [SerializeField] [Range(0.001f, 1000f)] private float maxDistance = 100f;
        [SerializeField] private LayerMask layerMaskVision;

        [Header("Reference")]
        [SerializeField] private Transform root;

        [Header("Info")]
        [SerializeField] private VisionInfo target = default;
        
        #region MonoBehaviour

        private void Awake()
        {
            if (isSingleton)
            {
                if (Instance == null) Instance = this;
            }
            
            if (Root == null) Root = transform;
        }

        private void Update()
        {
            UpdateVision();
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            if (!Active)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(Root.position, Root.position + Root.forward);
                return;
            }
            
            var target = Target;
            var see = target.transform != null;
            Gizmos.color = see ? Color.green : Color.red;
            Gizmos.DrawLine(Root.position, see ? target.point : (Root.position + Root.forward * maxDistance));
            if (see)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(target.point, target.point + target.normal);
            }
        }

        #endregion

        private void UpdateVision()
        {
            if (!Active)
            {
                if (target != default) target = default;
                return;
            }

            var ray = new Ray(Root.position, Root.forward);

            if (Physics.Raycast(ray, out var hitInfo, maxDistance, LayerMaskVision))
            {
                target.distance = hitInfo.distance;
                target.point = hitInfo.point;
                target.normal = hitInfo.normal;

                if (hitInfo.transform != target.transform)
                {
                    var prevTarget = target.transform;
                    target.transform = hitInfo.transform;
                    OnTargetChanged.Invoke(prevTarget, target.transform);
                }
            }
            else
            {
                if (target.transform != null)
                {
                    var prevTarget = target.transform;
                    target = default;
                    OnTargetChanged.Invoke(prevTarget, target.transform);
                }
            }
        }
        
        public bool TryGetTarget(out VisionInfo targetInfo)
        {
            targetInfo = SeeAnything ? this.target : default;

            return SeeAnything;
        }
        
        private class UnityDoubleTransformEvent : UnityEvent<Transform, Transform>
        {
        }
    }

    [Serializable]
    public struct VisionInfo
    {
        public float distance;
        public Vector3 point;
        public Vector3 normal;
        public Transform transform;

        public bool IsThisIt<T>(T component) where T : Component
        {
            return transform != null && transform.GetComponents<T>().Any(a => a == component);
        }
        
        public static bool operator ==(VisionInfo a, VisionInfo b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(VisionInfo a, VisionInfo b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is VisionInfo visionInfo))
            {
                return false;
                
            }

            if (visionInfo.transform == null)
            {
                return transform == null;
            }

            if (transform == null)
            {
                return false;
            }

            return visionInfo.transform == transform;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}