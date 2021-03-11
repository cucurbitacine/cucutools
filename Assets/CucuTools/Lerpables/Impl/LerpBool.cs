using System.Collections.Generic;
using System.Linq;
using CucuTools.Math;
using UnityEngine;

namespace CucuTools.Lerpables.Impl
{
    /// <inheritdoc />
    [AddComponentMenu(LerpMenuRoot + nameof(LerpBool))]
    public class LerpBool : LerpableList<bool>
    {
        /// <inheritdoc />
        public override List<LerpPoint<bool>> Elements
        {
            get => points ?? (points = new List<LerpPoint<bool>>());
            protected set
            {
                points = value;
                OnObserverUpdated();
            }
        }

        [Header("Bools")]
        [SerializeField] private List<LerpPoint<bool>> points;

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

            Value = ordered[iLeft].Value;

            return true;
        }
    }
}