using CucuTools.Attributes;
using UnityEngine;

namespace CucuTools.Blend
{
    public class CucuBlendFloatCurve : CucuBlend
    {
        public float Value
        {
            get => value;
            private set => this.value = value;
        }

        public AnimationCurve AnimationCurve
        {
            get => animationCurve ?? (animationCurve = AnimationCurve.Linear(0, 0, 1, 1));
            set
            {
                animationCurve = value;
                
                OnBlendChange();
            }
        }
        
        [Header("Float")]
        [CucuReadOnly]
        [SerializeField] private float value;
        [SerializeField] private AnimationCurve animationCurve;

        public override void OnBlendChange()
        {
            base.OnBlendChange();

            Value = AnimationCurve.Evaluate(Blend);
        }
    }
}