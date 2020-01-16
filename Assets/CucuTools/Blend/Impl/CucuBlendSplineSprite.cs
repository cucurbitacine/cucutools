using System;
using System.Collections.Generic;
using System.Linq;
using CucuTools.Blend.Interfaces;
using CucuTools.Blend.Spline;
using UnityEngine;

namespace CucuTools.Blend.Impl
{
    public class CucuBlendSplineSprite : CucuBlendSplineEntity<Sprite>
    {
        public override string Key => "Sprite blend";

        public Sprite Value => _value.Pin;

        [SerializeField] private List<CucuBlendPinSprite> _pins;

        private List<IBlendPin<Sprite>> _hashPins;
        private bool _useHash = false;

        private CucuBlendPinSprite _value;

        protected override void UpdateEntity()
        {
            var blend = GetLocalBlend(out var lefts, out var rights);

            var right = rights?.FirstOrDefault() as CucuBlendPinSprite;

            if (right == null) return;

            if (_value == null || !_value.Guid.Equals(right.Guid))
            {
                _value = right;
            }
        }

        public override List<IBlendPin<Sprite>> GetPins()
        {
            return _useHash
                ? _hashPins ?? (_hashPins = new List<IBlendPin<Sprite>>(_pins))
                : (_hashPins = new List<IBlendPin<Sprite>>(_pins));
        }

        [Serializable]
        private class CucuBlendPinSprite : CucuBlendPinEntity<Sprite>
        {
            public Guid Guid => (_guid ?? (_guid = Guid.NewGuid())).Value;
            private Guid? _guid;
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            foreach (var pin in _pins)
                pin.Key = $"[{pin.Value}] - {pin.Pin.name}";
        }
    }
}