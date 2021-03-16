using System.Collections.Generic;
using CucuTools.Math;
using UnityEngine;

namespace CucuTools.Lerpables.Impl
{
    /// <inheritdoc />
    [AddComponentMenu(LerpMenuRoot + nameof(LerpFloat))]
    public class LerpFloat : LerpableList<float>
    {
        /// <inheritdoc />
        public override List<LerpPoint<float>> Elements
        {
            get => points ?? (points = new List<LerpPoint<float>>());
            protected set
            {
                points = value;
                OnObserverUpdated();
            }
        }

        [Header("Points")]
        [SerializeField] private List<LerpPoint<float>> points;

        private float tCached;
        private int iLeftCached;
        private int iRightCached;
        
        /// <inheritdoc />
        protected override bool UpdateBehaviour()
        {
            if (SortedElements == null) return false;
            if (SortedElements.Count == 0) return false;
            
            tCached = CucuMath.GetLerpEdges(LerpValue, out iLeftCached, out iRightCached, SortedElements);

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

            Value = Mathf.Lerp(SortedElements[iLeftCached].Value, SortedElements[iRightCached].Value, tCached);
            
            return true;
        }
    }
}