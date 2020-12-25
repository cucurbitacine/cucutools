using UnityEngine;

namespace CucuTools
{
    public class AnimationTransformPosition : CucuAnimationBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target;

        [Header("Curve")]
        [SerializeField] private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        
        [Header("Limits")]
        [SerializeField] private Vector3 start;
        [SerializeField] private Vector3 finish;

        private Vector3 _localPosition;

        protected override void LerpInternal(float t)
        {
            target.localPosition = _localPosition + Vector3.Lerp(start, finish, curve.Evaluate(t));
        }

        protected override void OnAwake()
        {
            _localPosition = target.localPosition;
        }

        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying) return;
            
            var p0 = target.position + target.TransformDirection(start) * start.magnitude;
            var p1 = target.position + target.TransformDirection(finish) * finish.magnitude;

            Gizmos.DrawLine(p0, p1);
        }
    }
}
