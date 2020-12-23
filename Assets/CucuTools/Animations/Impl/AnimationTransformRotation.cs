using UnityEngine;

namespace CucuTools
{
    public class AnimationTransformRotation : CucuAnimationBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target;

        [Header("Curve")]
        [SerializeField] private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        
        [Header("Limits")]
        [SerializeField] private Vector3 start;
        [SerializeField] private Vector3 finish;

        private Vector3 _localRotation;
        
        protected override void LerpInternal(float t)
        {
            target.localRotation = Quaternion.Euler(_localRotation + Vector3.Lerp(start, finish, curve.Evaluate(t)));
        }

        protected override void OnAwake()
        {
            _localRotation = target.localRotation.eulerAngles;
        }
    }
}
