using System;
using CucuTools.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.Lerpables.Animations
{
    public abstract class AnimationBehaviour : LerpBehavior
    {
        public const string AnimLerpMenuRoot = LerpMenuRoot + "Animation/";
        
        protected const string GroupBaseName = "Animation";

        #region Properties
        
        public bool Paused
        {
            get => Info.Paused;
            set => Info.Paused = value;
        }
        
        public bool Playing
        {
            get => Info.Playing;
            private set => Info.Playing = value;
        }

        public float CurrentTime => Mathf.Clamp(TotalTime * LerpValue, 0f, TotalTime);

        public virtual float TotalTime
        {
            get => AnimationSpeed > 0f ? AnimationTime / AnimationSpeed : 0f;
            set { AnimationTime = value * (AnimationSpeed > 0f ? AnimationSpeed : (AnimationSpeed = 1f)); }
        }

        public bool AutoStart
        {
            get => Settings.autoStart;
            set => Settings.autoStart = value;
        }
        
        public bool Looped
        {
            get => Settings.looped;
            set => Settings.looped = value;
        }
        
        public virtual float AnimationTime
        {
            get => Settings.animationTime;
            set => Settings.animationTime = Mathf.Max(0f, value);
        }

        public virtual float AnimationSpeed
        {
            get => Settings.animationSpeed;
            set => Settings.animationSpeed = Mathf.Clamp(value,
                AnimationSettings.MIN_ANIMATION_SPEED,
                AnimationSettings.MAX_ANIMATION_SPEED);
        }

        public UnityEvent OnAnimationStart => AnimEvents.OnAnimationStart;
        public UnityEvent OnAnimationStop => AnimEvents.OnAnimationStop;
        
        public AnimationInfo Info => animationInfo ?? (animationInfo = new AnimationInfo());
        public AnimationSettings Settings => animationSettings ?? (animationSettings = new AnimationSettings());
        public AnimationEvents AnimEvents => animationEvents ?? (animationEvents = new AnimationEvents());
        
        #endregion

        #region SerializeField

        [Header("Animation")]

        [SerializeField] private AnimationInfo animationInfo;
        [SerializeField] private AnimationSettings animationSettings;
        [SerializeField] private AnimationEvents animationEvents;

        #endregion

        #region Public API

        [CucuButton("Start", @group: GroupBaseName, order: 0)]
        public void StartAnimation()
        {
            if (!Application.isPlaying) return;

            if (Playing) return;

            Lerp(0f);

            Playing = StartAnimationInternal();

            if (Playing) OnAnimationStart.Invoke();
        }

        [CucuButton("Stop", @group: GroupBaseName, order: 1)]
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

        protected override void OnAwake()
        {
            Validate();
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
            LerpTolerance.Use = false;
            LerpCurve.Use = false;
            
            AnimationTime = AnimationTime;
            AnimationSpeed = AnimationSpeed;
            Info.totalTime = TotalTime;
            Info.currentTime = CurrentTime;
        }

        #region MonoBehaviour

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
                Info.currentTime = CurrentTime;
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
        public class AnimationInfo
        {
            public bool Paused
            {
                get => paused;
                set => paused = value;
            }
        
            public bool Playing
            {
                get => playing;
                set => playing = value;
            }

            [SerializeField] private bool paused = false;
            [SerializeField] private bool playing = false;
            [Min(0f)]
            public float currentTime = 0f;
            [Min(0f)]
            public float totalTime = 1f;
        }
        
        [Serializable]
        public class AnimationEvents
        {
            public UnityEvent OnAnimationStart => onAnimationStart;
            public UnityEvent OnAnimationStop => onAnimationStop;
            
            [SerializeField] private UnityEvent onAnimationStart;
            [SerializeField] private UnityEvent onAnimationStop;

            public AnimationEvents()
            {
                onAnimationStart = new UnityEvent();
                onAnimationStop = new UnityEvent();
            }
        }

        [Serializable]
        public class AnimationSettings
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

            public AnimationSettings() : this(false)
            {
            }
        }
    }
}