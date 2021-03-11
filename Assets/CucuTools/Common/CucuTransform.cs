using System;
using UnityEngine;

namespace CucuTools.Common
{
    [Serializable]
    public struct CucuTransform
    {
        public Vector3 position
        {
            get => _position;
            set => _position = value;
        }

        public Quaternion rotation
        {
            get =>  _rotation;
            set => _rotation = value;
        }

        public Vector3 scale
        {
            get => _scale;
            set => _scale = value;
        }

        [SerializeField] private Vector3 _position;
        [SerializeField] private Quaternion _rotation;
        [SerializeField] private Vector3 _scale;
 
        public CucuTransform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            _position = position;
            _rotation = rotation;
            _scale = scale;
        }

        public CucuTransform(Transform transform) : this(transform.position, transform.rotation, transform.localScale)
        {
        }

        public static implicit operator CucuTransform(Transform tr)
        {
            return tr != null
                ? new CucuTransform {position = tr.position, rotation = tr.rotation, scale = tr.localScale}
                : new CucuTransform();
        }

        public Transform Set(Transform transform)
        {
            Set(ref transform);
            return transform;
        }
        
        public void Set(ref Transform transform)
        {
            CucuTransform.Set(ref transform, this);
        }
        
        public static CucuTransform Lerp(CucuTransform a, CucuTransform b, float t)
        {
            return new CucuTransform(
                Vector3.Lerp(a.position, b.position, t),
                Quaternion.Lerp(a.rotation, b.rotation, t),
                Vector3.Lerp(a.scale, b.scale, t));
        }

        public static CucuTransform Lerp(Transform transformA, Transform transformB, float t)
        {
            return Lerp(new CucuTransform(transformA), new CucuTransform(transformB), t);
        }

        public static CucuTransform Slerp(CucuTransform a, CucuTransform b, float t)
        {
            return new CucuTransform(
                Vector3.Slerp(a.position, b.position, t),
                Quaternion.Slerp(a.rotation, b.rotation, t),
                Vector3.Slerp(a.scale, b.scale, t));
        }

        public static CucuTransform Slerp(Transform transformA, Transform transformB, float t)
        {
            return Slerp(new CucuTransform(transformA), new CucuTransform(transformB), t);
        }

        public static Transform Set(ref Transform target, CucuTransform? source = null)
        {
            target.position = source?.position ?? Vector3.zero;
            target.rotation = source?.rotation ?? Quaternion.identity;
            target.localScale = source?.scale ?? Vector3.zero;

            return target;
        }
    }

    public static class CucuTransformExt
    {
        public static Transform Set(this Transform transform, CucuTransform cucu)
        {
            return transform.SetPosition(cucu).SetRotation(cucu).SetScale(cucu);
        }

        public static Transform SetPosition(this Transform transform, Vector3 position)
        {
            transform.position = position;
            return transform;
        }
        
        public static Transform SetPosition(this Transform transform, CucuTransform cucuTransform)
        {
            return transform.SetPosition(cucuTransform.position);
        }
        
        public static Transform SetRotation(this Transform transform, Quaternion rotation)
        {
            transform.rotation = rotation;
            return transform;
        }
        
        public static Transform SetRotation(this Transform transform, CucuTransform cucuTransform)
        {
            return transform.SetRotation(cucuTransform.rotation);
        }
        
        public static Transform SetScale(this Transform transform, Vector3 scale)
        {
            transform.localScale = scale;
            return transform;
        }
        
        public static Transform SetScale(this Transform transform, CucuTransform cucuTransform)
        {
            return transform.SetScale(cucuTransform.scale);
        }
    }
}