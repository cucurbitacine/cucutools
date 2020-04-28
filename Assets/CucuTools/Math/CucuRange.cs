using System;
using UnityEngine;

namespace CucuTools
{
    public class CucuRange<T>
        where T : IComparable
    {
        public T Value
        {
            get
            {
                Validate();
                return _value;
            }

            set
            {
                _value = value;
                Validate();
            }
        }

        public T MinValue => _minValue;
        public T MaxValue => _maxValue;

        [Header("Value")] [SerializeField] private T _value;

        [Space] [SerializeField] private T _minValue;

        [SerializeField] private T _maxValue;

        public void SetMinValue(T value)
        {
            _maxValue = value.CompareTo(_maxValue) < 0 ? value : _maxValue;
            Validate();
        }

        public void SetMaxValue(T value)
        {
            _maxValue = value.CompareTo(_minValue) > 0 ? value : _minValue;
            Validate();
        }

        public bool InRange(T value, bool includeMin = true, bool includeMax = true)
        {
            var isGreaterMin = includeMin ? value.CompareTo(_minValue) >= 0 : value.CompareTo(_minValue) > 0;
            var isLessMax = includeMax ? value.CompareTo(_maxValue) <= 0 : value.CompareTo(_maxValue) < 0;
            return isGreaterMin && isLessMax;
        }

        public void Validate()
        {
            if (_minValue.CompareTo(_maxValue) > 0) _maxValue = _minValue;

            if (_value.CompareTo(_minValue) < 0) _value = _minValue;
            if (_value.CompareTo(_maxValue) > 0) _value = _maxValue;
        }
    }

    [Serializable]
    public class CucuRangeFloat : CucuRange<float>
    {
        public CucuRangeFloat() : this(0f, 1f, 0)
        {
        }

        public CucuRangeFloat(float min, float max) : this(min, max, min)
        {
        }

        public CucuRangeFloat(float min, float max, float value)
        {
            SetMinValue(min);
            SetMaxValue(max);
            Value = value;
        }
    }


    [Serializable]
    public class CucuRangeInt : CucuRange<int>
    {
        public CucuRangeInt() : this(0, 1, 0)
        {
        }

        public CucuRangeInt(int min, int max) : this(min, max, min)
        {
        }

        public CucuRangeInt(int min, int max, int value)
        {
            SetMinValue(min);
            SetMaxValue(max);
            Value = value;
        }
    }
}