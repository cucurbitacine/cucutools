using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    public class CucuBlendSplineTime : CucuBlendSplineEntity<TimeSpan>
    {
        public override string Key => "Time blend-spline";
    
        public TimeSpan Time => _time;
    
        [Header("Result")]
        [SerializeField] private string _times;
    
        [Header("Use hashed pins")]
        [SerializeField]
        private bool _useHash = true;
    
        [Header("Time pins")]
        [SerializeField] private List<CucuBlendPinTime> _pins;
    
        private TimeSpan _time;

        private List<IBlendPin<TimeSpan>> _hashPins;
    
        protected override void UpdateEntity()
        {
            var pins = GetPins();

            var blend = GetLocalBlend(out var lefts, out var rights);

            var left = TimeUnit.GetUnit(lefts.First().Pin);
            var right = TimeUnit.GetUnit(rights.First().Pin);

            _time = TimeUnit.Lerp(left, right, blend).GetSpan();
            _times = _time.ToString();
        }

        public override List<IBlendPin<TimeSpan>> GetPins()
        {
            if (_pins == null) _pins = new List<CucuBlendPinTime>();
            
            return _useHash
                ? _hashPins ??
                  (_hashPins =
                      new List<IBlendPin<TimeSpan>>(_pins.Select(p => new CucuBlendPinTimeSpan(p.Value, p.Pin.GetSpan()))))
                : (_hashPins =
                    new List<IBlendPin<TimeSpan>>(_pins.Select(p => new CucuBlendPinTimeSpan(p.Value, p.Pin.GetSpan()))));
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            for (var i = 0; i < _pins.Count; i++)
            {
                var pin = _pins[i];
                pin.Key = $"[{i}] " + pin.Pin.GetSpan().ToString();
            }
        }
    
        [Serializable]
        private class CucuBlendPinTimeSpan : CucuBlendPinEntity<TimeSpan>
        {
            public CucuBlendPinTimeSpan(float argValue, TimeSpan getSpan) : base(argValue, getSpan)
            {
            }
        }
    
        [Serializable]
        private class CucuBlendPinTime : CucuBlendPinEntity<TimeUnit>
        {
        }

        [Serializable]
        private struct TimeUnit
        {
            [Range(0, 23)] public int hour;
            [Range(0, 59)] public int minute;
            [Range(0, 59)] public int second;

            public static TimeUnit Lerp(TimeUnit a, TimeUnit b, float t)
            {
                var sA = a.hour * 3600 + a.minute * 60 + a.second;
                var sB = b.hour * 3600 + b.minute * 60 + b.second;
                var diff = sB - sA;
                var newTime = (int)(diff * t) + sA;
                var m = newTime / 60;
                var s = newTime - m * 60;
                var h = m / 60;
                m -= h * 60;
                return new TimeUnit {hour = h, minute = m, second = s};
            }

            public TimeSpan GetSpan()
            {
                return new TimeSpan(hour, minute, second);
            }

            public static TimeUnit GetUnit(TimeSpan timeSpan)
            {
                return new TimeUnit {hour = timeSpan.Hours, minute = timeSpan.Minutes, second = timeSpan.Seconds};
            }
        }
    }
}
