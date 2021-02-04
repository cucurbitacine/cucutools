using System;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools
{
    public abstract class AnimationBehaviour : LerpableBehavior
    {
        public const string AnimLerpMenuRoot = LerpMenuRoot + "Animation/";
        
        protected const string GroupBaseName = "Animation";

        #region Properties

        public bool Paused
        {
            get => paused;
            set => paused = value;
        }
        
        public bool Playing
        {
            get => playing;
            private set => playing = value;
        }

        public float CurrentTime => Mathf.Clamp(TotalTime * LerpValue, 0f, TotalTime);

        public virtual float TotalTime
        {
            get => AnimationSpeed > 0f ? AnimationTime / AnimationSpeed : 0f;
            set { AnimationTime = value * (AnimationSpeed > 0f ? AnimationSpeed : (AnimationSpeed = 1f)); }
        }

        public bool AutoStart
        {
            get => animationSettings.autoStart;
            set => animationSettings.autoStart = value;
        }
        
        public bool Looped
        {
            get => animationSettings.looped;
            set => animationSettings.looped = value;
        }
        
        public virtual float AnimationTime
        {
            get => animationSettings.animationTime;
            set => animationSettings.animationTime = Mathf.Max(0f, value);
        }

        public virtual float AnimationSpeed
        {
            get => animationSettings.animationSpeed;
            set => animationSettings.animationSpeed = Mathf.Clamp(value,
                AnimationSettings.MIN_ANIMATION_SPEED,
                AnimationSettings.MAX_ANIMATION_SPEED);
        }

        public UnityEvent OnAnimationStart => animationEvents.onAnimationStart ?? (animationEvents.onAnimationStart = new UnityEvent());
        public UnityEvent OnAnimationStop => animationEvents.onAnimationStop ?? (animationEvents.onAnimationStop = new UnityEvent());
        
        #endregion

        #region SerializeField

        [Header("Animation")]
        [SerializeField] private bool paused = false;
        [SerializeField] private bool playing = false;
        [Min(0f)]
        [SerializeField] private float currentTime = 0f;
        [Min(0f)]
        [SerializeField] private float totalTime = 1f;
        [SerializeField] private AnimationSettings animationSettings;
        [SerializeField] private AnimationEvents animationEvents;

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
                
                if (Looped) StartAnimation();
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

            if (animationSettings.autoStart) StartAnimation();
        }

        private void Update()
        {
            if (Playing)
            {
                if (!Paused) AnimationFrame(Time.deltaTime);
                currentTime = CurrentTime;
            }

            OnUpdate();
        }

        protected override void Reset()
        {
            base.Reset();

            animationEvents.Reset();
            animationSettings.Reset();
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            Validate();
        }

        #endregion

        [Serializable]
        private struct AnimationEvents
        {
            public UnityEvent onAnimationStart;
            public UnityEvent onAnimationStop;

            public void Reset()
            {
                onAnimationStart = new UnityEvent();
                onAnimationStop = new UnityEvent();
            }
        }

        [Serializable]
        private struct AnimationSettings
        {
            public const float MIN_ANIMATION_SPEED = 0f;
            public const float MAX_ANIMATION_SPEED = 10f;
            
            public bool autoStart;
            public bool looped;
            [Min(0f)]
            public float animationTime;
            [Range(MIN_ANIMATION_SPEED, MAX_ANIMATION_SPEED)]
            public float animationSpeed;
            
            public AnimationSettings(bool autoStart)
            {
                this.autoStart = autoStart;
                looped = false;
                animationTime = 1f;
                animationSpeed = 1f;
            }

            public void Reset()
            {
                autoStart = false;
                looped = false;
                animationTime = 1f;
                animationSpeed = 1f;
            }
        }
    }
}