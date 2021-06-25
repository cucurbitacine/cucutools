using CucuTools.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.Blend
{
    public class CucuBlendMask : CucuBlendEntity
    {
        public CucuBlendEntity Target
        {
            get => target;
            set => target = value;
        }
        
        public float Value
        {
            get => value;
            private set => this.value = value;
        }

        public AnimationCurve Mask
        {
            get => mask ?? (mask = AnimationCurve.Linear(0, 0, 1, 1));
            set
            {
                mask = value;
                
                UpdateEntity();
            }
        }

        public UnityEvent<float> OnValueChanged => onValueChanged ?? (onValueChanged = new UnityEvent<float>());
        
        [Header("Mask")]
        [SerializeField] private CucuBlendEntity target;
        [CucuReadOnly]
        [SerializeField] private float value;
        [SerializeField] private AnimationCurve mask = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private UnityEvent<float> onValueChanged;
        
        protected override void UpdateEntityInternal()
        {
            Value = Mask.Evaluate(Blend);
            
            OnValueChanged.Invoke(Value);

            if (Target != null) Target.Blend = Value;
        }
    }
}