using System;
using CucuTools.Colors;
using UnityEngine;

namespace CucuTools.Blend
{
    public class CucuBlendRotate : CucuBlendTransformBase
    {
        public bool ChangeLocal
        {
            get => changeLocal;
            set
            {
                changeLocal = value;
                
                UpdateEntity();
            }
        }
        
        public RotateInfoParam RotateInfo
        {
            get => rotateInfo ?? (rotateInfo = new RotateInfoParam());
            set
            {
                rotateInfo = value;
                
                UpdateEntity();
            }
        }

        [Header("Rotate")]
        [SerializeField] private bool changeLocal = true;
        [SerializeField] private RotateInfoParam rotateInfo;
        
        protected override void UpdateEntityInternal()
        {
            if (ChangeLocal)
            {
                Target.localRotation = RotateInfo.Evaluate(Blend);
            }
            else
            {
                Target.rotation = RotateInfo.Evaluate(Blend);
            }
        }
    }
    
    [Serializable]
    public class RotateInfoParam : InfoParamBase<Quaternion>
    {
        public Vector3 Start
        {
            get => start;
            set => start = value;
        }

        public Vector3 End
        {
            get => end;
            set => end = value;
        }
        
        public AnimationCurve Modificator
        {
            get => modificator;
            set => modificator = value;
        }
        
        [SerializeField] private Vector3 start;
        [SerializeField] private Vector3 end;
        [SerializeField] private AnimationCurve modificator;
        
        public RotateInfoParam()
        {
            start = Vector3.zero;
            end = Vector3.zero;
            modificator = AnimationCurve.Constant(0, 1, 1);
        }

        public override Quaternion Evaluate(float t)
        {
            return Quaternion.Euler(Modificator.Evaluate(t) * Vector3.Lerp(Start, End, t));
        }
    }
}