using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc />
    [AddComponentMenu(LerpMenuRoot + nameof(LerpableCurveFloat))]
    public class LerpableCurveFloat : LerpableBehavior<float>
    {
        public AnimationCurve Curve
        {
            get => curve ?? (curve = GetDefaultCurve());
            set
            {
                curve = value;
                UpdateEntity();
            }
        }

        [Header("Curve")]
        [SerializeField] private AnimationCurve curve;

        /// <inheritdoc />
        protected override bool UpdateEntityInternal()
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