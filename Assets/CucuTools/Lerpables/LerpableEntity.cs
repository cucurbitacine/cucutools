using System;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools
{
    /// <inheritdoc cref="CucuTools.ILerpable" />
    public abstract class LerpableEntity : MonoBehaviour, ILerpable
    {
        public bool UseTolerance
        {
            get => lerpTolerance.use;
            set => lerpTolerance.use = value;
        }

        public float ToleranceValue
        {
            get => lerpTolerance.value;
            set => lerpTolerance.value = value;
        }
        
        /// <inheritdoc />
        public float LerpValue
        {
            get => lerpValue;
            private set => lerpValue = value;
        }

        /// <summary>
        /// Lerp value was changed
        /// </summary>
        public UnityEvent OnUpdated => lerpEvents.onUpdated ?? (lerpEvents.onUpdated = new UnityEvent());
        
        [Header("Lerpable")]
        [Range(0f, 1f)]
        [SerializeField] private float lerpValue = 0f;
        [SerializeField] protected LerpTolerance lerpTolerance;

        [SerializeField] private LerpCurve lerpCurve;
        [SerializeField] private LerpEvents lerpEvents;

        /// <inheritdoc />
        public void Lerp(float lerpValue)
        {
            lerpValue = Mathf.Clamp01(lerpCurve.useCurve ? lerpCurve.curve.Evaluate(lerpValue) : lerpValue);

            if (UseTolerance && Mathf.Abs(LerpValue - lerpValue) < ToleranceValue)
            {
                return;
            }

            LerpValue = lerpValue;

            UpdateEntity();
        }
        
        public bool UpdateEntity()
        {
            if (!UpdateEntityInternal()) return false;
            
            OnUpdated.Invoke();
            return true;
        }
        
        /// <summary>
        /// Update Entity after set lerp value
        /// </summary>
        protected abstract bool UpdateEntityInternal();

        protected virtual void Reset()
        {
            lerpTolerance.Reset();
            lerpCurve.Reset();
        }

        protected virtual void OnValidate()
        {
            UpdateEntityInternal();
        }
        
        [Serializable]
        public struct LerpTolerance
        {
            public bool use;
            [Range(0f, 1f)]
            public float value;

            public LerpTolerance(bool use)
            {
                this.use = use;
                value = 0.001f;
            }

            public void Reset()
            {
                use = true;
                value = 0.001f;
            }
        }
    
        [Serializable]
        public struct LerpCurve
        {
            public bool useCurve;
            public AnimationCurve curve;

            public LerpCurve(bool useCurve)
            {
                this.useCurve = useCurve;
                curve = GetDefault();
            }

            public void Reset()
            {
                useCurve = false;
                curve = GetDefault();
            }
            
            public static AnimationCurve GetDefault()
            {
                return AnimationCurve.Linear(0f, 0f, 1f, 1f);
            } 
        }
        
        [Serializable]
        public struct LerpEvents
        {
            public UnityEvent onUpdated;
        }

        public const string LerpMenuRoot = Cucu.MenuRoot + "Lerp/";
    }

    /// <summary>
    /// Lerpable entity with current result
    /// </summary>
    /// <typeparam name="TResult">Type of result</typeparam>
    public abstract class LerpableEntity<TResult> : LerpableEntity, IResultable<TResult>
    {
        /// <summary>
        /// Result of lerp
        /// </summary>
        public virtual TResult Result
        {
            get => _result;
            protected set => _result = value;
        }

        [Header("Result")]
        [SerializeField] private TResult _result;
    }
}