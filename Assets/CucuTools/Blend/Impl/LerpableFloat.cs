using UnityEngine;

namespace CucuTools
{
    public class LerpableFloat : LerpableEntity<float>
    {
        public AnimationCurve Curve
        {
            get => _curve ?? (_curve = GetDefaultCurve());
            set
            {
                _curve = value;
                UpdateEntity();
            }
        }

        [Header("Curve")]
        [SerializeField] private AnimationCurve _curve;

        protected override bool UpdateEntityInternal()
        {
            Result = Curve.Evaluate(LerpValue);
            return true;
        }

        private static AnimationCurve GetDefaultCurve()
        {
            return AnimationCurve.Linear(0f, 0f, 1f, 1f);
        }
        
        protected override void OnValidate()
        {
            base.OnValidate();

            if (_curve == null) _curve = GetDefaultCurve();
        }
    }
}