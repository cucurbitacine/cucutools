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
            get => tolerance.use;
            set => tolerance.use = value;
        }

        public float ToleranceValue
        {
            get => tolerance.value;
            set => tolerance.value = value;
        }
        
        /// <inheritdoc />
        public float LerpValue
        {
            get => _lerpValue;
            private set => _lerpValue = value;
        }

        /// <summary>
        /// Lerp value was changed
        /// </summary>
        public UnityEvent OnUpdated => _event.onUpdated ?? (_event.onUpdated = new UnityEvent());
        
        [Header("Lerpable")]
        [Range(0f, 1f)]
        [SerializeField] private float _lerpValue = 0f;
        
        [Header("Settings")]
        [SerializeField] protected ToleranceParam tolerance;
        [SerializeField] private EventParam _event;

        /// <inheritdoc />
        public void Lerp(float lerpValue)
        {
            lerpValue = Mathf.Clamp01(lerpValue);

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
            tolerance.use = true;
            tolerance.value = 0.001f;
        }

        protected virtual void OnValidate()
        {
            UpdateEntityInternal();
        }
        
        [Serializable]
        public struct ToleranceParam
        {
            public bool use;
            [Range(0f, 1f)]
            public float value;
        }
    
        [Serializable]
        public struct EventParam
        {
            public UnityEvent onUpdated;
        }
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

        [Header("Target result")]
        [SerializeField] private TResult _result;
    }
}