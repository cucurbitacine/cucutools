using System;
using UnityEngine;

namespace CucuTools
{
    public class AnimationTransformScale : CucuAnimationBehaviour
    {
        [Serializable]
        private class ScaleParams
        {
            public bool x = true;
            public bool y = true;
            public bool z = true;
        }

        [Header("Scale settings")]
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private ScaleParams scaleParam;
        [SerializeField] private Vector3 scaleInitial = Vector3.zero;
        [SerializeField] private Vector3 scaleFinal = Vector3.one;
        
        [Header("References")]
        [SerializeField] private Transform target;

        private Vector3 _localScale;

        protected override void LerpInternal(float t)
        {
            UpdateScale(t);
        }

        protected override void Validate()
        {
            base.Validate();

            if (target == null) target = transform;

            if (curve == null) curve = AnimationCurve.Constant(0f, 1f, 1f);
        }

        private void UpdateScale(float t)
        {
            if (target == null) return;
            target.localScale = GetScale(t);
        }
        
        private Vector3 GetScale(float t)
        {
            var scale = Vector3.one;

            if (scaleParam.x)
            {
                scale.x = curve.Evaluate(t) *
                          Mathf.Lerp(scaleInitial.x, Vector3.Scale(_localScale, scaleFinal).x, t);
            }

            if (scaleParam.y)
            {
                scale.y = curve.Evaluate(t) *
                          Mathf.Lerp(scaleInitial.y, Vector3.Scale(_localScale, scaleFinal).y, t);
            }

            if (scaleParam.z)
            {
                scale.z = curve.Evaluate(t) *
                          Mathf.Lerp(scaleInitial.z, Vector3.Scale(_localScale, scaleFinal).z, t);
            }

            return scale;
        }
        
        protected override void OnAwake()
        {
            _localScale = target.localScale;
        }
    }
}