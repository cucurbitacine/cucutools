using UnityEngine;
using UnityEngine.Events;

namespace CucuTools
{
    public abstract class CucuAnimationEntity : MonoBehaviour, IBlendable
    {
        protected const string GroupBaseName = "Animation";

        #region Properties

        public bool Playing
        {
            get => playing;
            private set => playing = value;
        }

        public float Blend => AnimationTime > 0f ? Mathf.Clamp01(CurrentTime / AnimationTime) : 1f;

        public UnityEvent OnAnimationStart => onAnimationStart ?? (onAnimationStart = new UnityEvent());
        public UnityEvent OnAnimationStop => onAnimationStop ?? (onAnimationStop = new UnityEvent());
        
        public float CurrentTime
        {
            get => currentTime;
            protected set => currentTime = Mathf.Max(0f, value);
        }
        
        public virtual float AnimationTime
        {
            get => AnimationTimeScale > 0f ? animationTime / AnimationTimeScale : float.MaxValue;
            protected set => animationTime = Mathf.Max(0f, value) * AnimationTimeScale;
        }
        
        public virtual float AnimationTimeScale
        {
            get => animationTimeScale;
            set => animationTimeScale = Mathf.Clamp(value, 0f, 10f);
        }

        #endregion

        #region SerializeField

        [Header("Info")]
        [SerializeField] private bool playing;
        [SerializeField, Min(0f)] private float currentTime;
        [SerializeField, Range(0f, 1f)] protected float progressDisplay;
        
        [Header("Settings")]
        [SerializeField, Min(0f)] private float animationTime;
        [SerializeField, Range(0f, 10f)] private float animationTimeScale = 1f;
        
        [Header("Events")]
        [SerializeField] private UnityEvent onAnimationStart;
        [SerializeField] private UnityEvent onAnimationStop;

        #endregion

        #region Public API

        [CucuButton("Start", group: GroupBaseName, order: 0)]
        public void StartAnimation()
        {
            if (!Application.isPlaying) return;

            if (Playing) StopAnimation();
            
            CurrentTime = 0f;

            Playing = StartAnimationInternal();

            if (Playing) OnAnimationStart.Invoke();
        }

        [CucuButton("Stop", group: GroupBaseName, order: 1)]
        public void StopAnimation()
        {
            if (!Application.isPlaying) return;
            
            Playing = false;

            CurrentTime = AnimationTime;

            StopAnimationInternal();

            OnAnimationStop.Invoke();
        }

        public void Lerp(float t)
        {
            LerpInternal(t);
        }

        public void Default()
        {
            if (Application.isPlaying)
            {
                Lerp(0f);

                DefaultInternal();
            }
        }
        
        #endregion

        #region Virtual API

        protected virtual void LerpInternal(float t)
        {
            
        }

        
        protected virtual bool StartAnimationInternal()
        {
            return true;
        }

        protected virtual void StopAnimationInternal()
        {
            
        }

        protected virtual void DefaultInternal()
        {
            
        }
        
        protected virtual void OnAwake()
        {
            
        }
        
        protected virtual void Validate()
        {
            AnimationTimeScale = AnimationTimeScale;
            AnimationTime = AnimationTime;
        }
        
        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            Validate();
            
            OnAwake();
            
            Default();
        }

        private void OnValidate()
        {
            Validate();
        }

        #endregion
    }
}