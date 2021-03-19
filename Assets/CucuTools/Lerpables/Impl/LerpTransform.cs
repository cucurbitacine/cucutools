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

        private float tCached;
        private int iLeftCached;
        private int iRightCached;
        
        protected override bool UpdateBehaviour()
        {
            if (SortedElements == null) return false;
            if (SortedElements.Count == 0) return false;
            
            if (SortedElements.Count == 1)
            {
                Value = Elements[0].Value;
                return false;
            }

            tCached = CucuMath.GetLerpEdges(LerpValue, out iLeftCached, out iRightCached, SortedElements);

            if (iLeftCached < 0)
            {
                Value = Elements[iRightCached].Value;
                return true;
            }
            
            if (iRightCached < 0)
            {
                Value = Elements[iLeftCached].Value;
                return true;
            }
            
            Value = CucuTransform.Lerp(points[iLeftCached].Value, points[iRightCached].Value, tCached);

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

                Gizmos.color = CucuColor.Rainbow.Evaluate(point.T);
                
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