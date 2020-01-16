using System;
using System.Collections.Generic;
using System.Linq;
using CucuTools.Blend.Interfaces;
using CucuTools.Blend.Spline;
using UnityEngine;

namespace CucuTools.Blend.Impl
{
    /// <inheritdoc />
    public class CucuBlendSplineColor : CucuBlendSplineEntity<Color>
    {
        public override string Key => "Color blend-spline";
        
        public Color Color => _color;
        
        [Header("Result")]
        [SerializeField] private Color _color;
        
        [Header("Use hashed pins")]
        [SerializeField]
        private bool _useHash = true;
        
        [Header("Color pins")]
        [SerializeField] private List<CucuBlendPinColor> _pins;

        private List<IBlendPin<Color>> _hashPins;
        
        /// <inheritdoc />
        protected override void UpdateEntity()
        {
            var Pins = GetPins();

            if (Pins == null || !Pins.Any()) return;

            var blend = GetLocalBlend(out var lefts, out var rights);

            if (lefts == null || rights == null) return;

            var leftC = lefts.Select(l => l.Pin).Aggregate((res, cur) => res + cur) / lefts.Count();
            var rightC = rights.Select(r => r.Pin).Aggregate((res, cur) => res + cur) / rights.Count();

            _color = Color.Lerp(leftC, rightC, blend);
        }

        /// <inheritdoc />
        public override List<IBlendPin<Color>> GetPins()
        {
            return _useHash
                ? _hashPins ?? (_hashPins = new List<IBlendPin<Color>>(_pins))
                : (_hashPins = new List<IBlendPin<Color>>(_pins));
        }
        
        protected override void OnValidate()
        {
            base.OnValidate();
            
            foreach (var pin in _pins)
            {
                pin.Key = pin.Pin.ToString();
            }
        }
        
        [Serializable]
        private class CucuBlendPinColor : CucuBlendPinEntity<Color>
        {
        }
    }
}