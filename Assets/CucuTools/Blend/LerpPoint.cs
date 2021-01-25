using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CucuTools
{
    [Serializable]
    public struct LerpPoint<TValue> : IComparable<LerpPoint<TValue>>
    {
        public float T
        {
            get => t;
            set => t = Mathf.Clamp01(value);
        }
        
        public TValue Value
        {
            get => this.value;
            set => this.value = value;
        }

        [Range(0f, 1f)]
        [SerializeField] private float t;
        [SerializeField] private TValue value;

        public LerpPoint(float t, TValue value)
        {
            this.t = t;
            this.value = value;
        }

        public bool IsValid()
        {
            return 0 <= T && T <= 1f;
        }

        public static bool operator == (LerpPoint<TValue> a, LerpPoint<TValue> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(LerpPoint<TValue> a, LerpPoint<TValue> b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return (obj is LerpPoint<TValue> other) && Equals(other);
        }

        public bool Equals(LerpPoint<TValue> other)
        {
            return t.Equals(other.t) && EqualityComparer<TValue>.Default.Equals(value, other.value);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = T.GetHashCode();
                hashCode = (hashCode * 397) ^ EqualityComparer<TValue>.Default.GetHashCode(Value);
                return hashCode;
            }
        }

        public int CompareTo(LerpPoint<TValue> obj)
        {
            return T.CompareTo(obj.t);
        }
    }
}