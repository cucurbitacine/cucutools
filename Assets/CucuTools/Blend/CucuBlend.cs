using System;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.Blend
{
    public class CucuBlend : CucuBehaviour
    {
        public float Blend
        {
            get => GetBlend();
            set => SetBlend(value);
        }

        public BlendTolerance Tolerance => tolerance ?? (tolerance = new BlendTolerance());
        public BlendEvents Events => events ?? (events = new BlendEvents());

        [Range(0f, 1f)]
        [SerializeField] private float blend;
        [SerializeField] private BlendTolerance tolerance;
        [SerializeField] private BlendEvents events;

        public virtual void OnBlendChange()
        {
            Events.OnChanged.Invoke(Blend);
        }

        protected virtual float GetBlend()
        {
            return blend;
        }

        protected virtual void SetBlend(float value)
        {
            value = Mathf.Clamp01(value);

            if (AllowedBlendChange(value))
            {
                blend = value;
                
                OnBlendChange();
            }
        }

        protected virtual bool AllowedBlendChange(float value)
        {
            return !Tolerance.Use || Mathf.Abs(Blend - value) >= Tolerance.Tolerance;
        }

        protected virtual void Awake()
        {
            OnBlendChange();
        }
        
        protected virtual void OnValidate()
        {
            OnBlendChange();
        }
    }

    [Serializable]
    public class BlendTolerance
    {
        public bool Use
        {
            get => use;
            set => use = value;
        }

        public float Tolerance
        {
            get => tolerance;
            set => tolerance = Mathf.Clamp01(value);
        }

        [SerializeField] private bool use;
        [Range(0f, 1f)]
        [SerializeField] private float tolerance;

        public BlendTolerance()
        {
            use = true;
            tolerance = 0.001f;
        }
    }

    [Serializable]
    public class BlendEvents
    {
        public UnityEvent<float> OnChanged => onChanged ?? (onChanged = new UnityEvent<float>());
        
        [SerializeField] private UnityEvent<float> onChanged;
    }
}