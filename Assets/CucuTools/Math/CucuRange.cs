using System;
using UnityEngine;

namespace CucuTools.Math
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
        public CucuRangeFloat()
        {
            SetMinValue(0f);
            SetMaxValue(1f);
        }

        public CucuRangeFloat(float min, float max)
        {
            SetMinValue(min);
            SetMaxValue(max);
        }
    }


    [Serializable]
    public class CucuRangeInt : CucuRange<int>
    {
        public CucuRangeInt()
        {
            SetMinValue(0);
            SetMaxValue(1);
        }

        public CucuRangeInt(int min, int max)
        {
            SetMinValue(min);
            SetMaxValue(max);
        }
    }
}