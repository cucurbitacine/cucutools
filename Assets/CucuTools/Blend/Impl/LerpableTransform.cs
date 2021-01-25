using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    public class LerpableTransform : LerpableBlend<Transform, Transform>
    {
        public override Transform Result
        {
            get => base.Result;
            protected set
            {
                base.Result = value;
                UpdateEntity();
            }
        }

        public override List<LerpPoint<Transform>> Elements
        {
            get => _points ?? (_points = new List<LerpPoint<Transform>>());
            protected set => _points = value;
        }

        [Header("Anchors")]
        [SerializeField] private List<LerpPoint<Transform>> _points;

        private CucuTransform proxyTransform
        {
            set
            {
                if (Result != null) Result.Set(value);
            }
        }
        
        protected override bool UpdateEntityInternal()
        {
            if (Elements == null) return false;
            if (Elements.Count == 0) return false;
            
            if (Elements.Count == 1)
            {
                proxyTransform = Elements[0].Value;
                return false;
            }

            var points = Elements.OrderBy(e => e.T).ToArray();

            CucuMath.GetEdges(LerpValue, out var iLeft, out var iRight, points.Select(p => p.T));

            if (iLeft < 0 && iRight >= 0)
            {
                proxyTransform = points[iRight].Value;
                return true;
            }
            
            if (iRight < 0 && iLeft >= 0)
            {
                proxyTransform = points[iLeft].Value;
                return true;
            }

            var t = CucuMath.GetLerpValue(LerpValue, points[iLeft].T, points[iRight].T);
            proxyTransform = CucuTransform.Lerp(points[iLeft].Value, points[iRight].Value, t);

            return true;
        }
    }
}