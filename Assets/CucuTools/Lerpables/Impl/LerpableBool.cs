using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc />
    [AddComponentMenu(LerpMenuRoot + nameof(LerpableBool))]
    public class LerpableBool : LerpableList<bool>
    {
        /// <inheritdoc />
        public override List<LerpPoint<bool>> Elements
        {
            get => points ?? (points = new List<LerpPoint<bool>>());
            protected set
            {
                points = value;
                UpdateEntity();
            }
        }

        [Header("Points")]
        [SerializeField] private List<LerpPoint<bool>> points;

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

            Result = ordered[iLeft].Value;

            return true;
        }
    }
}