using System;
using UnityEngine;

namespace CucuTools
{
    [Serializable]
    public class CucuRange<T> : ObserverEntity, IComparable, IObserverEntity<T>, ISetValue<T>
        where T : IComparable
    {
        public T Value
        {
            get => value;
            set
            {
                if (this.value.CompareTo(value)==0)
                {
                    return;
                }
                
                this.value = value;
                
                UpdateValue();
                
                UpdateObserver();
            }
        }

        public T Max => limitFirst.CompareTo(limitSecond) >= 0 ? limitFirst : limitSecond;
        public T Min => limitFirst.CompareTo(limitSecond) < 0 ? limitFirst : limitSecond;

        [Header("Value")]
        [SerializeField] private T value;
        [Space]
        [SerializeField] private T limitFirst;
        [SerializeField] private T limitSecond;

        private T _cache;
        
        public CucuRange<T> SetEdges(T first, T second)
        {
            limitFirst = first;
            limitSecond = second;
            return this;
        }

        public CucuRange<T> SetMax(T max)
        {
            var newMax = max;

            if (max.CompareTo(Min) < 0)
            {
                Debug.LogWarning($"New max {max} must be more then min {Min}");

                newMax = limitFirst.CompareTo(limitSecond) >= 0 ? limitSecond : limitFirst;
            }

            if (limitFirst.CompareTo(limitSecond) >= 0)
                limitFirst = newMax;
            else
                limitSecond = newMax;

            UpdateValue();

            return this;
        }

        public CucuRange<T> SetMin(T min)
        {
            var newMin = min;

            if (min.CompareTo(Max) > 0)
            {
                Debug.LogWarning($"New min {min} must be more then max {Max}");

                newMin = limitFirst.CompareTo(limitSecond) < 0 ? limitSecond : limitFirst;
            }

            if (limitFirst.CompareTo(limitSecond) < 0)
                limitFirst = newMin;
            else
                limitSecond = newMin;

            UpdateValue();

            return this;
        }

        public bool IsValid(T value, bool includeMin = true, bool includeMax = true)
        {
            var isGreaterMin = includeMin ? value.CompareTo(Min) >= 0 : value.CompareTo(Min) > 0;
            var isLessMax = includeMax ? value.CompareTo(Max) <= 0 : value.CompareTo(Max) < 0;
            return isGreaterMin && isLessMax;
        }

        public void UpdateValue()
        {
            if (value.CompareTo(Min) < 0) value = Min;
            if (value.CompareTo(Max) > 0) value = Max;
        }

        public static implicit operator T(CucuRange<T> range)
        {
            return range.Value;
        }

        public int CompareTo(object obj)
        {
            if (obj is CucuRange<T> range)
            {
                return Value.CompareTo(range.value);
            }

            throw new InvalidCastException(
                $"Cannot convert {obj.GetType().FullName} to CucuRange<{typeof(T).FullName}>");
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
            SetEdges(min, max);
            Value = value;
        }

        public static float operator +(CucuRangeFloat left, CucuRangeFloat right)
        {
            return left.Value + right.Value;
        }

        public static float operator -(CucuRangeFloat left, CucuRangeFloat right)
        {
            return left.Value - right.Value;
        }

        public static float operator *(CucuRangeFloat left, CucuRangeFloat right)
        {
            return left.Value * right.Value;
        }

        public static float operator /(CucuRangeFloat left, CucuRangeFloat right)
        {
            return left.Value / right.Value;
        }

        public static float operator +(CucuRangeFloat range, float value)
        {
            return new CucuRangeFloat(range.Min, range.Max, range.Value + value);
        }

        public static float operator +(float value, CucuRangeFloat range)
        {
            return range + value;
        }

        public static float operator -(CucuRangeFloat range, float value)
        {
            return range + -value;
        }

        public static float operator -(float value, CucuRangeFloat range)
        {
            return value + -1 * range;
        }
        
        public static float operator *(CucuRangeFloat range, float value)
        {
            return range.Value * value;
        }

        public static float operator *(float value, CucuRangeFloat range)
        {
            return range * value;
        }

        public static float operator /(CucuRangeFloat range, float value)
        {
            return range * (1 / value);
        }

        public static float operator /(float value, CucuRangeFloat range)
        {
            return value / range.Value;
        }
        
        public static bool operator ==(CucuRangeFloat left, CucuRangeFloat right)
        {
            return Mathf.Abs(left.Value - right.Value) <= float.Epsilon;
        }

        public static bool operator !=(CucuRangeFloat left, CucuRangeFloat right)
        {
            return !(left == right);
        }
        
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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
            SetEdges(min, max);
            Value = value;
        }

        public static int operator +(CucuRangeInt left, CucuRangeInt right)
        {
            return left.Value + right.Value;
        }

        public static int operator -(CucuRangeInt left, CucuRangeInt right)
        {
            return left.Value - right.Value;
        }

        public static int operator *(CucuRangeInt left, CucuRangeInt right)
        {
            return left.Value * right.Value;
        }

        public static int operator /(CucuRangeInt left, CucuRangeInt right)
        {
            return left.Value / right.Value;
        }

        public static int operator +(CucuRangeInt range, int value)
        {
            return range.Value + value;
        }

        public static int operator +(int value, CucuRangeInt range)
        {
            return range + value;
        }

        public static int operator -(CucuRangeInt range, int value)
        {
            return range + -value;
        }

        public static int operator -(int value, CucuRangeInt range)
        {
            return value + -1 * range;
        }

        public static int operator *(CucuRangeInt range, int value)
        {
            return range.Value * value;
        }

        public static int operator *(int value, CucuRangeInt range)
        {
            return range * value;
        }

        public static int operator /(CucuRangeInt range, int value)
        {
            return range * (1 / value);
        }

        public static int operator /(int value, CucuRangeInt range)
        {
            return value / range.Value;
        }
        
        public static bool operator ==(CucuRangeInt left, CucuRangeInt right)
        {
            return left.Value == right.Value;
        }

        public static bool operator !=(CucuRangeInt left, CucuRangeInt right)
        {
            return !(left == right);
        }
        
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}