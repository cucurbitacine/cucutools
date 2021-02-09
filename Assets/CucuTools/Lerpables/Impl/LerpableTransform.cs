using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    [AddComponentMenu(LerpMenuRoot + nameof(LerpableTransform))]
    public class LerpableTransform : LerpableList<Transform>
    {
        public override Transform Value
        {
            get => base.Value;
            protected set
            {
                base.Value = value;
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
                if (Value == null) return;

                if (syncParam.SyncAll)
                {
                    Value.Set(value);
                    return;
                }

                if (syncParam.syncPosition) Value.SetPosition(value);
                if (syncParam.syncRotation) Value.SetRotation(value);
                if (syncParam.syncScale) Value.SetScale(value);
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