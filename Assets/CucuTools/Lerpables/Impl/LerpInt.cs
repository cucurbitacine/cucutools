using System.Collections.Generic;
using System.Linq;
using CucuTools.Math;
using UnityEngine;

namespace CucuTools.Lerpables.Impl
{
    /// <inheritdoc />
    [AddComponentMenu(LerpMenuRoot + nameof(LerpFloat))]
    public class LerpInt : LerpableList<int>
    {
        /// <inheritdoc />
        public override List<LerpPoint<int>> Elements
        {
            get => points ?? (points = new List<LerpPoint<int>>());
            protected set
            {
                points = value;
                OnObserverUpdated();
            }
        }

        [Header("Points")]
        [SerializeField] private List<LerpPoint<int>> points;

        /// <inheritdoc />
        protected override bool UpdateBehaviour()
        {
            if (Elements == null) return false;
            if (Elements.Count == 0) return false;

            var ordered = Elements.OrderBy(e => e.T).ToArray();
            
            var t = CucuMath.GetLerpEdges(LerpValue, out var iLeft, out var iRight, ordered);

            if (iLeft < 0)
            {
                Value = ordered[iRight].Value;
                return true;
            }
            
            if (iRight < 0)
            {
                Value = ordered[iLeft].Value;
                return true;
            }

            Value = (int) Mathf.Lerp(ordered[iLeft].Value, ordered[iRight].Value, t);
            
            return true;
        }
    }
}