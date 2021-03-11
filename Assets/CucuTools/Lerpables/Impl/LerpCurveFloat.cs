using UnityEngine;

namespace CucuTools.Lerpables.Impl
{
    /// <inheritdoc />
    [AddComponentMenu(LerpMenuRoot + nameof(LerpCurveFloat))]
    public class LerpCurveFloat : LerpBehavior<float>
    {
        public AnimationCurve Curve
        {
            get => curve ?? (curve = GetDefaultCurve());
            set
            {
                curve = value;
                OnObserverUpdated();
            }
        }

        [Header("Curve")]
        [SerializeField] private AnimationCurve curve;

        /// <inheritdoc />
        protected override bool UpdateBehaviour()
        {
            Value = Curve.Evaluate(LerpValue);
            return true;
        }

        private static AnimationCurve GetDefaultCurve()
        {
            return AnimationCurve.Linear(0f, 0f, 1f, 1f);
        }
        
        protected override void OnValidate()
        {
            base.OnValidate();

            if (curve == null) curve = GetDefaultCurve();
        }
    }
}