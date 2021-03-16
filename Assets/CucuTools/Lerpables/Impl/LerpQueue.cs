using System;
using System.Collections.Generic;
using System.Linq;
using CucuTools.Math;
using UnityEngine;

namespace CucuTools.Lerpables.Impl
{
    /// <inheritdoc />
    [AddComponentMenu(LerpMenuRoot + nameof(LerpQueue))]
    public class LerpQueue : LerpableList<LerpBehavior>
    {
        public override List<LerpPoint<LerpBehavior>> Elements
        {
            get => elements ?? (elements = new List<LerpPoint<LerpBehavior>>());
            protected set => elements = value;
        }
        
        [Header("Lerp points")]
        [SerializeField] private List<LerpPoint<LerpBehavior>> elements;

        private LerpPoint<LerpBehavior> leftCached;
        private LerpPoint<LerpBehavior> rightCached;
        

        public bool Remove(float t)
        {
            return Elements.RemoveAll(p => p.T == t) > 0;
        }

        public int RemoveAll(Predicate<LerpPoint<LerpBehavior>> match = null)
        {
            if (match != null) return Elements.RemoveAll(match);
            
            var count = Elements.Count;
            Elements.Clear();
            return count;
        }

        public void RemoveAt(params int[] index)
        {
            var indexes = index
                .Where(t => 0 <= t && t < Elements.Count)
                .Distinct()
                .OrderByDescending(t => t)
                .ToArray();

            for (int i = 0; i < indexes.Length; i++)
            {
                Elements.RemoveAt(indexes[i]);
            }
        }

        /// <inheritdoc />
        protected override bool UpdateBehaviour()
        {
            if (SortedElements == null) return false;
            if (SortedElements.Count == 0) return false;

            return LinearLerp(SortedElements);
        }

        private bool LinearLerp(List<LerpPoint<LerpBehavior>> points)
        {
            if (points.Count == 1)
            {
                points[0].Value?.Lerp(LerpValue);
                return true;
            }

            for (var i = 1; i < points.Count; i++)
            {
                leftCached.T = points[i - 1].T;
                leftCached.Value = points[i - 1].Value;

                rightCached.T = points[i].T;
                rightCached.Value = points[i].Value;

                var t = CucuMath.GetLerpValue(LerpValue, leftCached.T, rightCached.T);
                rightCached.Value?.Lerp(t);

                if (i == 1)
                {
                    t = CucuMath.GetLerpValue(LerpValue, 0f, leftCached.T);
                    leftCached.Value?.Lerp(t);
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