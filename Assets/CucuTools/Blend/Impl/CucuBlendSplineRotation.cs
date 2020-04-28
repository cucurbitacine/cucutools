using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    public class CucuBlendSplineRotation : CucuBlendSplineEntity<Transform>
    {
        public override string Key => "Rotation blend-spline";

        public Quaternion Rotation => _rotation;

        [Header("Result")] [SerializeField] private Quaternion _rotation;

        [Header("Use hashed pins")] [SerializeField]
        private bool _useHash = true;

        [Header("Transform pins")] [SerializeField]
        private List<CucuBlendPinTransform> _pins;

        private List<IBlendPin<Transform>> _hashPins;

        /// <inheritdoc />
        protected override void UpdateEntity()
        {
            var Pins = GetPins();

            if (Pins == null || !Pins.Any()) return;

            var blend = GetLocalBlend(out var lefts, out var rights);

            if (lefts == null || rights == null) return;

            var leftQ = lefts.Select(l => l.Pin.rotation).FirstOrDefault();
            var rightQ = rights.Select(r => r.Pin.rotation).FirstOrDefault();

            if (UseCurve) blend = Curve.Evaluate(blend);

            _rotation = Quaternion.Lerp(leftQ, rightQ, blend);
        }

        /// <inheritdoc />
        public override List<IBlendPin<Transform>> GetPins()
        {
            return _useHash
                ? _hashPins ?? (_hashPins = new List<IBlendPin<Transform>>(_pins))
                : (_hashPins = new List<IBlendPin<Transform>>(_pins));
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (_pins == null || !_pins.Any()) return;

            foreach (var pin in _pins)
                pin.Key = pin.Pin.rotation.ToString();
        }

        [Serializable]
        private class CucuBlendPinTransform : CucuBlendPinEntity<Transform>
        {
        }
    }
}