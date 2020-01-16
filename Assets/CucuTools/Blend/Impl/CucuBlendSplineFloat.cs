using System;
using System.Collections.Generic;
using System.Linq;
using CucuTools.Blend.Interfaces;
using CucuTools.Blend.Spline;
using UnityEngine;

namespace CucuTools.Blend.Impl
{
    public class CucuBlendSplineFloat : CucuBlendSplineEntity<float>
    {
        public override string Key => "Float blend-spline";

        public float Value => _value;

        [Header("Result")]
        [SerializeField]
        private float _value;
        
        [Header("Use hashed pins")]
        [SerializeField]
        private bool _useHash = true;
        
        [Header("Float pins")]
        [SerializeField]
        private List<CucuBlendPinFloat> _pins;

        private List<IBlendPin<float>> _hashPins;
        
        protected override void UpdateEntity()
        {
            var blend = GetLocalBlend(out var lefts, out var rights);

            if (lefts == null || rights == null) return;

            var left = lefts.Sum(l => l.Pin) / lefts.Count();
            var right = rights.Sum(r => r.Pin) / rights.Count();

            _value = left * (1 - blend) + right * blend;
        }

        public override List<IBlendPin<float>> GetPins()
        {
            return _useHash
                ? _hashPins ?? (_hashPins = new List<IBlendPin<float>>(_pins))
                : (_hashPins = new List<IBlendPin<float>>(_pins));
        }

        [Serializable]
        private class CucuBlendPinFloat : CucuBlendPinEntity<float>
        {
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (_pins == null || !_pins.Any()) return;

            foreach (var pin in _pins)
                pin.Key = $"[{pin.Value}] : {pin.Pin}";
        }
    }
}