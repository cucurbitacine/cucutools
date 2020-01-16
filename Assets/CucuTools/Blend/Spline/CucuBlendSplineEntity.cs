using System.Collections.Generic;
using System.Linq;
using CucuTools.Blend.Interfaces;
using UnityEngine;

namespace CucuTools.Blend.Spline
{
    /// <inheritdoc />
    public abstract class CucuBlendSplineEntity<TObject> : CucuBlendEntity, IBlendSpline<TObject>
    {
        /// <summary>
        /// Curve (use for your pleasure)
        /// </summary>
        public AnimationCurve Curve => _curve;

        /// <summary>
        /// Using curve or not
        /// </summary>
        public bool UseCurve
        {
            get => _useCurve;
            set => _useCurve = value;
        }

        [Header("Blend curves")]
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private bool _useCurve;

        /// <inheritdoc />
        public abstract List<IBlendPin<TObject>> GetPins();

        /// <inheritdoc />
        public float GetLocalBlend(
            out IEnumerable<IBlendPin<TObject>> lefts,
            out IEnumerable<IBlendPin<TObject>> rights)
        {
            lefts = null;
            rights = null;

            // Get all out pins
            var pins = GetPins();

            // if null or empty return default value
            if (pins == null || !pins.Any()) return 0.0f;

            // Group and sort a pins by value
            var sortedPins = pins
                .GroupBy(a => a.Value)
                .OrderBy(a => a.Key);

            // Search of left and right grouped pins which surround our value
            var _lefts = sortedPins.LastOrDefault(a => a.Key <= Blend);
            var _rights = sortedPins.FirstOrDefault(a => Blend <= a.Key);

            lefts = _lefts;
            rights = _rights;
            
            // Get left and right blend value
            var leftBlend = _lefts?.Key ?? 0f;
            var rightBlend = _rights?.Key ?? 1f;

            // return local blend value or if the values are close return default value 
            return Mathf.Abs(leftBlend - rightBlend) > float.Epsilon
                ? (Blend - leftBlend) / (rightBlend - leftBlend)
                : 0.0f;
        }

        public void SetAnimationCurve(AnimationCurve curve)
        {
            _curve = curve;
        }
    }
}