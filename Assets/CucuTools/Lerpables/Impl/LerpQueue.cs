using System;
using System.Collections.Generic;
using System.Linq;
using CucuTools.Math;
using UnityEngine;

namespace CucuTools.Lerpables.Impl
{
    /// <inheritdoc />
    [AddComponentMenu(LerpMenuRoot + nameof(LerpQueue))]
    public class LerpQueue : LerpBehavior
    {
        [Header("Lerp points")]
        [SerializeField] private List<LerpPoint<LerpBehavior>> elements;

        public void Add(LerpPoint<LerpBehavior> point)
        {
            if (elements == null) elements = new List<LerpPoint<LerpBehavior>>();
            if (point.IsValid() && point.Value != null && point.Value != this) elements.Add(point);
        }
        
        public void Add(float t, LerpBehavior value)
        {
            Add(new LerpPoint<LerpBehavior>(t, value));
        }

        public bool Remove(LerpBehavior value)
        {
            if (elements == null) return false;

            if (value == null) return false;

            return elements.RemoveAll(p => p.Value == value) > 0;
        }

        public bool Remove(float t)
        {
            if (elements == null) return false;

            return elements.RemoveAll(p => p.T == t) > 0;
        }

        public bool Remove(LerpPoint<LerpBehavior> point)
        {
            if (elements == null) return false;

            return elements.Remove(point);
        }

        public int RemoveAll(Predicate<LerpPoint<LerpBehavior>> match = null)
        {
            if (elements == null) return 0;

            if (match != null) return elements.RemoveAll(match);
            
            var count = elements.Count;
            elements.Clear();
            return count;
        }

        public void RemoveAt(params int[] index)
        {
            if (elements == null) return;

            var indexes = index
                .Where(t => 0 <= t && t < elements.Count)
                .Distinct()
                .OrderByDescending(t => t)
                .ToArray();

            for (int i = 0; i < indexes.Length; i++)
            {
                elements.RemoveAt(indexes[i]);
            }
        }

        public void Sort()
        {
            if (elements == null) return;

            elements = elements.OrderBy(p => p).ToList();
        }

        /// <inheritdoc />
        protected override bool UpdateBehaviour()
        {
            if (elements == null) return false;
            if (elements.Count == 0) return false;

            return LinearLerp(elements.OrderBy(p => p.T).ToArray());
        }

        private bool LinearLerp(params LerpPoint<LerpBehavior>[] points)
        {
            if (points.Length == 1)
            {
                points[0].Value?.Lerp(LerpValue);
                return true;
            }

            for (var i = 1; i < points.Length; i++)
            {
                var left = points[i - 1];
                var right = points[i];

                var t = CucuMath.GetLerpValue(LerpValue, left.T, right.T);
                right.Value?.Lerp(t);

                if (i == 1)
                {
                    t = CucuMath.GetLerpValue(LerpValue, 0f, left.T);
                    left.Value?.Lerp(t);
                }
            }

            return true;
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (elements != null)
            {
                elements = elements.Where(p => p.Value != this).ToList();
            }
        }
    }
}