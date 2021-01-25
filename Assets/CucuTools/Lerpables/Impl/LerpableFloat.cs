using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc />
    [AddComponentMenu(LerpMenuRoot + nameof(LerpableFloat))]
    public class LerpableFloat : LerpableList<float>
    {
        /// <inheritdoc />
        public override List<LerpPoint<float>> Elements
        {
            get => points ?? (points = new List<LerpPoint<float>>());
            protected set
            {
                points = value;
                UpdateEntity();
            }
        }

        [Header("Points")]
        [SerializeField] private List<LerpPoint<float>> points;

        /// <inheritdoc />
        protected override bool UpdateEntityInternal()
        {
            if (Elements == null) return false;
            if (Elements.Count == 0) return false;

            var ordered = Elements.OrderBy(e => e.T).ToArray();
            
            var t = CucuMath.GetLerpEdges(LerpValue, out var iLeft, out var iRight, ordered);

            if (iLeft < 0)
            {
                Result = ordered[iRight].Value;
                return true;
            }
            
            if (iRight < 0)
            {
                Result = ordered[iLeft].Value;
                return true;
            }

            Result = Mathf.Lerp(ordered[iLeft].Value, ordered[iRight].Value, t);
            
            return true;
        }
    }
}