using System;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools
{
    /// <inheritdoc cref="CucuTools.ILerpable" />
    public abstract class LerpBehavior : ObserverBehaviour, ILerpable, IListenerEntity
    {
        public const string LerpMenuRoot = Cucu.MenuRoot + "Lerp/";
        
        public float LerpValue => Lerpable.LerpValue;
        public Lerpable.LerpTolerance LerpTolerance => Lerpable.Tolerance;
        public Lerpable.LerpCurve LerpCurve => Lerpable.Curve;
        public Lerpable.LerpEvents LerpEvents => Lerpable.Events;
        
        protected Lerpable Lerpable => lerpable ?? (lerpable = new Lerpable());
        
        [Header("Lerpable")]
        [SerializeField] private Lerpable lerpable;
        
        public void Lerp(float t)
        {
            Lerpable.Lerp(t);

#if UNITY_EDITOR
            UpdateBehaviour();
#endif
        }
        
        public void OnObserverUpdated()
        {
            if (UpdateBehaviour()) UpdateObserver();
        }

        protected abstract bool UpdateBehaviour();

        protected virtual void OnAwake()
        {
        }
        
        private void Awake()
        {
            Lerpable.Subscribe(this);
            
            OnAwake();
        }
        
        protected virtual void OnValidate()
        {
            UpdateBehaviour();
        }

        protected virtual void OnDrawGizmos()
        {
        }
    }

    /// <summary>
    /// Lerpable entity with current result
    /// </summary>
    /// <typeparam name="TValue">Type of result</typeparam>
    public abstract class LerpBehavior<TValue> : LerpBehavior, IObserverEntity<TValue>
    {
        public virtual TValue Value
        {
            get => Result.Value;
            protected set
            {
                Result.Value = value;

                Result.OnValueChanged.Invoke(Result.Value);
            }
        }

        public LerpResult Result => lerpResult ?? (lerpResult = new LerpResult());
        
        [SerializeField] private LerpResult lerpResult;

        [Serializable]
        public class LerpResult
        {
            public TValue Value
            {
                get => value;
                set => this.value = value;
            }
            
            public UnityEvent<TValue> OnValueChanged
            {
                get => onValueChanged;
                set => this.onValueChanged = value;
            }
            
            [SerializeField] private TValue value;
            [SerializeField] private UnityEvent<TValue> onValueChanged;

            public LerpResult()
            {
                value = default;
                onValueChanged = new UnityEvent<TValue>();
            }
        }
    }
}