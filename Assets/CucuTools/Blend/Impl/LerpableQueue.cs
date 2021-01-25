using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    public class LerpableQueue : LerpableEntity
    {
        [Header("Lerp points")]
        [SerializeField] private List<LerpPoint<LerpableEntity>> _points;

        public void Add(LerpPoint<LerpableEntity> point)
        {
            if (_points == null) _points = new List<LerpPoint<LerpableEntity>>();
            if (point.IsValid() && point.Value != null && point.Value != this) _points.Add(point);
        }
        
        public void Add(float t, LerpableEntity value)
        {
            Add(new LerpPoint<LerpableEntity>(t, value));
        }

        public bool Remove(LerpableEntity value)
        {
            if (_points == null) return false;

            if (value == null) return false;

            return _points.RemoveAll(p => p.Value == value) > 0;
        }

        public bool Remove(float t)
        {
            if (_points == null) return false;

            return _points.RemoveAll(p => p.T == t) > 0;
        }

        public bool Remove(LerpPoint<LerpableEntity> point)
        {
            if (_points == null) return false;

            return _points.Remove(point);
        }

        public int RemoveAll(Predicate<LerpPoint<LerpableEntity>> match = null)
        {
            if (_points == null) return 0;

            if (match != null) return _points.RemoveAll(match);
            
            var count = _points.Count;
            _points.Clear();
            return count;
        }

        public void RemoveAt(params int[] index)
        {
            if (_points == null) return;

            var indexes = index
                .Where(t => 0 <= t && t < _points.Count)
                .Distinct()
                .OrderByDescending(t => t)
                .ToArray();

            for (int i = 0; i < indexes.Length; i++)
            {
                _points.RemoveAt(indexes[i]);
            }
        }

        public void Sort()
        {
            if (_points == null) return;

            _points = _points.OrderBy(p => p).ToList();
        }
        
        protected override bool UpdateEntityInternal()
        {
            if (_points == null) return false;
            if (_points.Count == 0) return false;

            return LinearLerp(_points.OrderBy(p => p.T).ToArray());
        }

        private bool LinearLerp(params LerpPoint<LerpableEntity>[] points)
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

            if (_points != null)
            {
                _points = _points.Where(p => p.Value != this).ToList();
            }
        }
    }
}