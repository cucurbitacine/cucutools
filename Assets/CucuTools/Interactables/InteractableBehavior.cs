using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.Interactables
{
    public class InteractableBehavior : MonoBehaviour, IInteractableEntity
    {
        public virtual bool IsEnabled
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
                }
                else
                {
                    DisableInternal();

                    Events.OnDisable.Invoke();
                }
            }
        }

        public bool DefaultOnAwake
        {
            get => defaultOnAwake;
            set => defaultOnAwake = value;
        }

        public InteractableEvents Events => events ?? (events = new InteractableEvents());
        
        public virtual float ClickDuration
        {
            get => clickDuration;
            set => clickDuration = value;
        }
        
        [Header("Info")]
        [SerializeField] private bool isEnabled = true;
        [SerializeField] private bool defaultOnAwake = true;
        [SerializeField] protected InteratableState state;

        [Header("Events")]
        [SerializeField] private InteractableEvents events;
        
        [Header("Settings")]
        [SerializeField] private float clickDuration = 0.2f;
        
        public void Normal()
        {
            //state.isNormal = true;
            state.isHovered = false;
            
            if (!IsEnabled) return;

            if (state.isClicked) return;

            NormalInternal();
            
            Events.OnNormal.Invoke();
        }
        
        public void Hover()
        {
            //state.isNormal = false;
            state.isHovered = true;
            
            if (!IsEnabled) return;
            
            if (state.isClicked) return;

            HoverInternal();
            
            Events.OnHover.Invoke();
        }

        public void Click()
        {
            if (!IsEnabled) return;
            
            if (state.isClicked) return;

            StartCoroutine(_ClickCoroutine(clickDuration));
            
            ClickInternal();
            
            Events.OnClick.Invoke();
        }

        protected virtual void NormalInternal()
        {
        }

        protected virtual void HoverInternal()
        {
        }

        protected virtual void ClickInternal()
        {
        }
        
        protected virtual void DisableInternal()
        {
        }

        protected virtual IEnumerator _ClickCoroutine(float delay)
        {
            state.isClicked = true;
                
            yield return new WaitForSeconds(delay);
            
            state.isClicked = false;

            if (state.isHovered) Hover();
            else if (state.isNormal) Normal();
        }
        
        protected virtual void OnAwake()
        {
        }
        
        private void Awake()
        {
            if (defaultOnAwake)
            {
                if (IsEnabled)
                {
                    NormalInternal();

                    Events.OnNormal.Invoke();
                }
                else
                {
                    DisableInternal();

                    Events.OnDisable.Invoke();
                }
            }

            OnAwake();
        }

        [Serializable]
        public struct InteratableState
        {
            public bool isNormal => !isHovered && !isClicked;
            public bool isHovered;
            public bool isClicked;
        }
        
        [Serializable]
        public class InteractableEvents
        {
            public UnityEvent OnNormal => onNormal ?? (onNormal = new UnityEvent());
            public UnityEvent OnHover => onHover ?? (onHover = new UnityEvent());
            public UnityEvent OnClick => onClick ?? (onClick = new UnityEvent());
            public UnityEvent OnDisable => onDisable ?? (onDisable = new UnityEvent());

            [SerializeField] private UnityEvent onNormal;
            [SerializeField] private UnityEvent onHover;
            [SerializeField] private UnityEvent onClick;
            [SerializeField] private UnityEvent onDisable;
        }
    }
}