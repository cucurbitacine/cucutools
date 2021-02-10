using System;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools
{
    /// <inheritdoc cref="CucuTools.ILerpable" />
    [Serializable]
    public sealed class Lerpable : ObserverEntity, ILerpable
    {
        public float LerpValue
        {
            get => lerpValue;
            private set => lerpValue = value;
        }

        public LerpTolerance Tolerance => lerpTolerance;
        public LerpCurve Curve => lerpCurve;
        public LerpEvents Events => lerpEvents;
        
        [Range(0f, 1f)]
        [SerializeField] private float lerpValue;
        [SerializeField] private LerpTolerance lerpTolerance;
        [SerializeField] private LerpCurve lerpCurve;
        [SerializeField] private LerpEvents lerpEvents;

        public Lerpable()
        {
            lerpValue = 0f;
            lerpTolerance = new LerpTolerance();
            lerpCurve = new LerpCurve();
            lerpEvents = new LerpEvents();
        }
        
        public void Lerp(float t)
        {
            t = Mathf.Clamp01(Curve.Use ? Curve.AnimationCurve.Evaluate(t) : t);

            if (Tolerance.Use && Mathf.Abs(LerpValue - t) < Tolerance.Value)
            {
                return;
            }

            LerpValue = t;

            Events.OnUpdated.Invoke();
            
            Update();
        }
        
        [Serializable]
        public class LerpTolerance
        {
            public bool Use
            {
                get => use;
                set => use = value;
            }

            public float Value
            {
                get => value;
                set => this.value = Mathf.Clamp01(value);
            }
            
            [SerializeField] private bool use;
            [Range(0f, 1f)]
            [SerializeField] private float value;

            public LerpTolerance(bool use)
            {
                this.use = use;
                value = 0.001f;
            }

            public LerpTolerance() : this(true)
            {
            }
            
            public void Reset()
            {
                use = true;
                value = 0.001f;
            }
        }
    
        [Serializable]
        public class LerpCurve
        {
            public bool Use
            {
                get => use;
                set => use = value;
            }
            
            public AnimationCurve AnimationCurve
            {
                get => animationCurve;
            }
            
            [SerializeField] private bool use;
            [SerializeField] private AnimationCurve animationCurve;

            public LerpCurve(bool use)
            {
                this.use = use;
                animationCurve = DefaultFill(new AnimationCurve());
            }

            public LerpCurve() : this(false)
            {
            }
            
            public void Reset()
            {
                use = false;
                DefaultFill(animationCurve);
            }
            
            private static AnimationCurve DefaultFill(AnimationCurve curve)
            {
                if (curve == null) return null;

                for (int i = curve.length - 1; i >= 0; i--)
                    curve.RemoveKey(i);

                curve.AddKey(0f, 0f);
                curve.AddKey(1f, 1f);

                return curve;
            } 
        }
        
        [Serializable]
        public class LerpEvents
        {
            public UnityEvent OnUpdated => onUpdated;
            
            [SerializeField] private UnityEvent onUpdated;

            public LerpEvents()
            {
                onUpdated = new UnityEvent();
            }
        }
    }
}