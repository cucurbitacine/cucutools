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

        private int iLeftCached;
        private int iRightCached;
        
        /// <inheritdoc />
        protected override bool UpdateBehaviour()
        {
            if (SortedElements == null) return false;
            if (SortedElements.Count == 0) return false;

            CucuMath.GetLerpEdges(LerpValue, out iLeftCached, out iRightCached, SortedElements);

            if (iLeftCached < 0)
            {
                Value = SortedElements[iRightCached].Value;
                return true;
            }

            if (iRightCached < 0)
            {
                Value = SortedElements[iLeftCached].Value;
                return true;
            }

            Value = SortedElements[iLeftCached].Value;

            return true;
        }
    }
}