using System.Collections.Generic;
using System.Linq;
using CucuTools.Raycasts.Effects.Impl;
using UnityEngine;

namespace CucuTools.Interactables
{
    public class InteractableObserver : MonoBehaviour
    {
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                if (!isEnabled)
                {
                    foreach (var interactable in Interactables)
                    {
                        interactable?.Normal();
                    }
                
                    Interactables.Clear();
                }
            }
        }

        public RaycastEffectObserver Observer
        {
            get => observer;
            protected set => observer = value;
        }

        public List<IInteractableEntity> Interactables =>
            interactables ?? (interactables = new List<IInteractableEntity>());
        
        [SerializeField] private bool isEnabled = true;
        [SerializeField] private RaycastEffectObserver observer;
        
        private List<IInteractableEntity> interactables;
        
        [SerializeField] private string[] interactableInfos;
        
        public void Press()
        {
            foreach (var interactable in interactables)
            {
                interactable?.Press();               
            }
        }
        
        private void UpdateInteracable()
        {
            if (Observer.HasHit)
            {
                var actualInteractables = Observer.ObservedObject.transform.GetComponents<IInteractableEntity>();

                foreach (var lostInteractable in Interactables.Where(current => !actualInteractables.Contains(current)))
                {
                    lostInteractable?.Normal();
                }
                
                foreach (var newInteractable in actualInteractables.Where(actual => !Interactables.Contains(actual)))
                {
                    newInteractable?.Hover();
                }

                
                Interactables.Clear();
                Interactables.AddRange(actualInteractables);
            }
            else
            {
                foreach (var interactable in Interactables)
                {
                    interactable?.Normal();
                }
                
                Interactables.Clear();
            }
            
#if UNITY_EDITOR
            interactableInfos = Interactables
                .Select(i => (i is InteractableBehavior beh) ? beh.name : i.GetType().Name)
                .ToArray();
#endif
        }

        private void Validate()
        {
            if (Observer == null) Observer = GetComponent<RaycastEffectObserver>();
            if (Observer == null) Observer = transform.parent?.GetComponent<RaycastEffectObserver>();
        }
        
        private void Awake()
        {
            Validate();
        }

        private void Update()
        {
            if (Observer.IsEnabled && IsEnabled) UpdateInteracable();
        }

        private void OnValidate()
        {
            Validate();
        }
    }
}