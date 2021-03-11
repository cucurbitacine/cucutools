using System;
using System.Collections.Generic;
using System.Linq;
using CucuTools.Colors;
using CucuTools.Common;
using CucuTools.Math;
using UnityEngine;

namespace CucuTools.Lerpables.Impl
{
    [AddComponentMenu(LerpMenuRoot + nameof(LerpTransform))]
    public class LerpTransform : LerpList<CucuTransform, Transform>
    {
        public override List<LerpPoint<Transform>> Elements
        {
            get => points ?? (points = new List<LerpPoint<Transform>>());
            protected set => points = value;
        }

        [SerializeField] private Transform target;
        [SerializeField] private SyncParam syncParam;
        
        [Header("Transform points")]
        [SerializeField] private List<LerpPoint<Transform>> points;

        protected override bool UpdateBehaviour()
        {
            if (Elements == null) return false;
            if (Elements.Count == 0) return false;
            
            if (Elements.Count == 1)
            {
                Value = Elements[0].Value;
                return false;
            }

            var ordered = Elements.OrderBy(e => e.T).ToArray();

            var t = CucuMath.GetLerpEdges(LerpValue, out var iLeft, out var iRight, ordered);

            if (iLeft < 0)
            {
                Value = Elements[iRight].Value;
                return true;
            }
            
            if (iRight < 0)
            {
                Value = Elements[iLeft].Value;
                return true;
            }
            
            Value = CucuTransform.Lerp(points[iLeft].Value, points[iRight].Value, t);

            if (target != null)
            {
                if (syncParam.SyncAll)
                {
                    target.Set(Value);
                }
                else
                {
                    if (syncParam.syncPosition) target.SetPosition(Value);
                    if (syncParam.syncRotation) target.SetRotation(Value);
                    if (syncParam.syncScale) target.SetScale(Value);
                }
            }
            
            return true;
        }

        protected virtual void Reset()
        {
            syncParam.syncPosition = true;
            syncParam.syncRotation = true;
            syncParam.syncScale = true;
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (target != null && (points?.Any(p => p.Value == target) ?? false))
            {
                target = null;
            }
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (target == null) return;

            var sharedMesh = target.GetComponent<MeshFilter>()?.sharedMesh;

            if (sharedMesh == null) return;

            var ordered = Elements.OrderBy(e => e.T).ToArray();

            for (var i = 0; i < ordered.Length; i++)
            {
                var point = ordered[i];
                
                if (point.Value == null) continue;

                Gizmos.color = CucuColor.Rainbow.Get(point.T);
                
                if (i < ordered.Length - 1)
                {
                    var next = ordered[i + 1];
                    if (next.Value != null)
                    {
                        Gizmos.DrawLine(point.Value.position, next.Value.position);
                    }
                }
                
                Gizmos.DrawWireMesh(sharedMesh, point.Value.position, point.Value.rotation, point.Value.lossyScale);
            }
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