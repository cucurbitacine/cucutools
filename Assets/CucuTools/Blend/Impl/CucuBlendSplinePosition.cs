using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc />
    public class CucuBlendSplinePosition : CucuBlendSplineEntity<Transform>
    {
        public override string Key => "Position blend-spline";
        
        public Vector3 Position => _position;
        
        [Header("Result")]
        [SerializeField] private Vector3 _position;
        
        [Header("Use hashed pins")]
        [SerializeField]
        private bool _useHash = true;
        
        [Header("Transform pins")]
        [SerializeField] private List<CucuBlendPinTransform> _pins;
        
        private List<IBlendPin<Transform>> _hashPins;
        
        /// <inheritdoc />
        protected override void UpdateEntity()
        {
            var blend = GetLocalBlend(out var lefts, out var rights);

            if (lefts == null || rights == null) return;

            var leftV = lefts.Select(l => l.Pin.position).Aggregate((res, cur) => res + cur) / lefts.Count();
            var rightV = rights.Select(r => r.Pin.position).Aggregate((res, cur) => res + cur) / rights.Count();

            if (UseCurve) blend = Curve.Evaluate(blend);
            
            _position = Vector3.Lerp(leftV, rightV, blend);
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
            {
                if (pin == null || pin.Pin == null) continue;
                pin.Key = pin.Pin.position.ToString();
            }
        }
        
        [Serializable]
        private class CucuBlendPinTransform : CucuBlendPinEntity<Transform>
        {
        }
    }
}