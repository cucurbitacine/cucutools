using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.Interactables
{
    /// <inheritdoc />
    public class InteractableBehavior : InteractableEntity
    {
        /// <summary>
        /// State info
        /// </summary>
        public InteratableState InteractState => state;
        
        /// <summary>
        /// Events of interactable
        /// </summary>
        public InteractableEvents InteractEvents => events ?? (events = new InteractableEvents());
        
        /// <summary>
        /// Flag set default state on awake
        /// </summary>
        public bool DefaultOnAwake
        {
            get => defaultOnAwake;
            set => defaultOnAwake = value;
        }

        /// <summary>
        /// Duration time after last press down
        /// </summary>
        public virtual float PressDuration
        {
            get => pressDuration;
            set => pressDuration = value;
        }
        
        protected float pressTimeLeft;
        protected Coroutine pressCoroutine;

        #region SerializeField

        [Header("Info")]
        [SerializeField] private bool isEnabled = true;
        [SerializeField] private InteratableState state;

        [SerializeField] private InteractableEvents events;
        
        [Header("Settings")]
        [SerializeField] private bool defaultOnAwake = true;
        [Range(0.005f, 1f)]
        [SerializeField] private float pressDuration = 0.01f;

        #endregion

        #region Virtual API

        protected virtual void NormalInternal()
        {
        }

        protected virtual void HoverInternal()
        {
        }

        protected virtual void PressDownInternal()
        {
        }
        
        protected virtual void PressUpInternal()
        {
        }

        protected virtual void EnabledInternal()
        {
        }
        
        protected virtual void DisableInternal()
        {
        }

        protected virtual IEnumerator _PressCoroutine()
        {
            PressDownInternal();
            
            InteractEvents.OnPressDown.Invoke();
            
            while (pressTimeLeft > 0f)
            {
                pressTimeLeft -= Time.deltaTime;
                yield return null;
            }

            pressTimeLeft = 0f;
            state.isPressed = false;

            PressUpInternal();
            
            InteractEvents.OnPressUp.Invoke();
            
            if (state.isHovered) Hover();
            else if (state.isNormal) Normal();
        }

        protected virtual void Awake()
        {
            if (defaultOnAwake)
            {
                if (IsEnabled)
                {
                    NormalInternal();

                    EnabledInternal();
                    
                    InteractEvents.OnNormal.Invoke();
                }
                else
                {
                    DisableInternal();

                    InteractEvents.OnDisable.Invoke();
                }
            }
        }

        protected virtual void OnValidate()
        {
        }
        
        #endregion
        
        #region IInteractableEntity

        /// <inheritdoc />
        public override bool IsEnabled
        {
            get => isEnabled;
            set
            {
                if (isEnabled == value) return;

                isEnabled = value;

                if (isEnabled)
                {
                    if (state.isHovered) Hover();
                    else if (state.isNormal) Normal();

                    EnabledInternal();
                    
                    InteractEvents.OnEnabled.Invoke();
                }
                else
                {
                    DisableInternal();

                    InteractEvents.OnDisable.Invoke();
                }
            }
        }
        
        /// <inheritdoc />
        public override void Normal()
        {
            state.isHovered = false;
            
            if (!IsEnabled) return;

            if (state.isPressed) return;

            NormalInternal();
            
            InteractEvents.OnNormal.Invoke();
        }
        
        /// <inheritdoc />
        public override void Hover()
        {
            state.isHovered = true;
            
            if (!IsEnabled) return;
            
            if (state.isPressed) return;

            HoverInternal();
            
            InteractEvents.OnHover.Invoke();
        }

        /// <inheritdoc />
        public override void Press()
        {
            if (!IsEnabled) return;

            state.isPressed = true;
            
            if (pressTimeLeft <= 0f)
            {
                pressTimeLeft = pressDuration;
                
                if (pressCoroutine != null) StopCoroutine(pressCoroutine);
                pressCoroutine = StartCoroutine(_PressCoroutine());
            }
            
            pressTimeLeft = pressDuration;
        }

        #endregion

        #region Settings

        [Serializable]
        public struct InteratableState
        {
            public bool isNormal => !isHovered && !isPressed;
            public bool isHovered;
            public bool isPressed;
        }
        
        [Serializable]
        public class InteractableEvents
        {
            public UnityEvent OnNormal => onNormal ?? (onNormal = new UnityEvent());
            public UnityEvent OnHover => onHover ?? (onHover = new UnityEvent());
            public UnityEvent OnPressDown => onPressDown ?? (onPressDown = new UnityEvent());
            public UnityEvent OnPressUp => onPressUp ?? (onPressUp = new UnityEvent());
            public UnityEvent OnEnabled => onEnabled ?? (onEnabled = new UnityEvent());
            public UnityEvent OnDisable => onDisable ?? (onDisable = new UnityEvent());

            [SerializeField] private UnityEvent onNormal;
            [SerializeField] private UnityEvent onHover;
            [SerializeField] private UnityEvent onPressDown;
            [SerializeField] private UnityEvent onPressUp;
            [SerializeField] private UnityEvent onEnabled;
            [SerializeField] private UnityEvent onDisable;
        }

        #endregion
    }
}