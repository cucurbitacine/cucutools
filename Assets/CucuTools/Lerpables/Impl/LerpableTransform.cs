using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    [AddComponentMenu(LerpMenuRoot + nameof(LerpableTransform))]
    public class LerpableTransform : LerpableList<Transform, Transform>
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
            get => points ?? (points = new List<LerpPoint<Transform>>());
            protected set => points = value;
        }

        [SerializeField] private SyncParam syncParam;
        
        [Header("Transform points")]
        [SerializeField] private List<LerpPoint<Transform>> points;

        private CucuTransform proxyTransform
        {
            set
            {
                if (Result == null) return;

                if (syncParam.SyncAll)
                {
                    Result.Set(value);
                    return;
                }

                if (syncParam.syncPosition) Result.SetPosition(value);
                if (syncParam.syncRotation) Result.SetRotation(value);
                if (syncParam.syncScale) Result.SetScale(value);
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

            var ordered = Elements.OrderBy(e => e.T).ToArray();

            var t = CucuMath.GetLerpEdges(LerpValue, out var iLeft, out var iRight, ordered);

            if (iLeft < 0)
            {
                proxyTransform = Elements[iRight].Value;
                return true;
            }
            
            if (iRight < 0)
            {
                proxyTransform = Elements[iLeft].Value;
                return true;
            }
            
            proxyTransform = CucuTransform.Lerp(points[iLeft].Value, points[iRight].Value, t);
            
            return true;
        }

        protected override void Reset()
        {
            base.Reset();

            syncParam.syncPosition = true;
            syncParam.syncRotation = true;
            syncParam.syncScale = true;
        }

        [Serializable]
        private struct SyncParam
        {
            public bool SyncAll => syncPosition && syncRotation && syncScale;
            
            public bool syncPosition;
            public bool syncRotation;
            public bool syncScale;
        }
    }
}