using System;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools
{
    public abstract class AnimationEntity : LerpableEntity
    {
        public const float MIN_ANIMATION_SPEED = 0f;
        public const float MAX_ANIMATION_SPEED = 10f;

        protected const string GroupBaseName = "Animation";

        #region Properties

        public bool Playing
        {
            get => playing;
            private set => playing = value;
        }

        public UnityEvent OnAnimationStart => _events.OnAnimationStart ?? (_events.OnAnimationStart = new UnityEvent());
        public UnityEvent OnAnimationStop => _events.OnAnimationStop ?? (_events.OnAnimationStop = new UnityEvent());

        public float CurrentTime => Mathf.Clamp(TotalTime * LerpValue, 0f, TotalTime);

        public virtual float TotalTime
        {
            get => AnimationSpeed > 0f ? AnimationTime / AnimationSpeed : 0f;
            set { AnimationTime = value * (AnimationSpeed > 0f ? AnimationSpeed : (AnimationSpeed = 1f)); }
        }

        public virtual float AnimationTime
        {
            get => animationTime;
            set => animationTime = Mathf.Max(0f, value);
        }

        public virtual float AnimationSpeed
        {
            get => animationSpeed;
            set => animationSpeed = Mathf.Clamp(value, MIN_ANIMATION_SPEED, MAX_ANIMATION_SPEED);
        }

        #endregion

        #region SerializeField

        [Header("Info")]
        [SerializeField] private bool playing = false;
        [Min(0f)]
        [SerializeField] private float currentTime = 0f;
        [Min(0f)]
        [SerializeField] private float totalTime = 1f;

        [Header("Settings")]
        [SerializeField] private bool autoStart;
        [Min(0f)]
        [SerializeField] private float animationTime = 1f;
        [Range(MIN_ANIMATION_SPEED, MAX_ANIMATION_SPEED)]
        [SerializeField]
        private float animationSpeed = 1f;

        [Header("Events")]
        [SerializeField] private Events _events;
        
        #endregion

        #region Public API

        [CucuButton("Start", group: GroupBaseName, order: 0)]
        public void StartAnimation()
        {
            if (!Application.isPlaying) return;

            if (Playing) return;

            Lerp(0f);

            Playing = StartAnimationInternal();

            if (Playing) OnAnimationStart.Invoke();
        }

        [CucuButton("Stop", group: GroupBaseName, order: 1)]
        public void StopAnimation()
        {
            if (!Application.isPlaying) return;

            if (!Playing) return;
            
            Lerp(1f);
            
            Playing = false;

            StopAnimationInternal();

            OnAnimationStop.Invoke();
        }

        #endregion

        #region Virtual API
        
        protected virtual bool StartAnimationInternal()
        {
            return true;
        }

        protected virtual void StopAnimationInternal()
        {
        }

        protected virtual void OnAwake()
        {
        }

        protected virtual void OnStart()
        {
        }
        
        protected virtual void OnUpdate()
        {
        }
        
        #endregion

        private void AnimationFrame(float deltaTime)
        {
            if (LerpValue >= 1f)
            {
                StopAnimation();
            }
            else
            {
                Lerp(LerpValue + deltaTime / TotalTime); // TODO :: may change less than tolerance!
            }
        }
        
        private void Validate()
        {
            UseTolerance = false;
            
            AnimationTime = AnimationTime;
            AnimationSpeed = AnimationSpeed;
            totalTime = TotalTime;
            currentTime = CurrentTime;
        }

        #region MonoBehaviour

        private void Awake()
        {
            Validate();

            OnAwake();
        }

        private void Start()
        {
            OnStart();

            if (autoStart) StartAnimation();
        }

        private void Update()
        {
            if (Playing)
            {
                AnimationFrame(Time.deltaTime);
                currentTime = CurrentTime;
            }

            OnUpdate();
        }
        
        protected override void OnValidate()
        {
            base.OnValidate();

            Validate();
        }

        #endregion

        [Serializable]
        private struct Events
        {
            public UnityEvent OnAnimationStart;
            public UnityEvent OnAnimationStop;
        }
    }
}