using System.Collections.Generic;
using System.Linq;
using CucuTools.Raycasts.Effects.Impl;
using UnityEngine;

namespace CucuTools.Interactables
{
    public class InteractableObserver : MonoBehaviour
    {
        public RaycastEffectObserver Observer
        {
            get => observer;
            protected set => observer = value;
        }

        private List<IInteractableEntity> Interactables =>
            interactables ?? (interactables = new List<IInteractableEntity>());
        
        [SerializeField] private RaycastEffectObserver observer;
        
        private List<IInteractableEntity> interactables;

        [SerializeField] private string[] interactableInfos;
        
        public void Click()
        {
            foreach (var interactable in interactables)
            {
                interactable?.Click();                
            }
        }
        
        private void UpdateInteracable()
        {
            if (Observer.HasHit)
            {
                var actualInteractables = Observer.ObservedObject.transform.GetComponents<IInteractableEntity>();

                foreach (var newInteractable in actualInteractables.Where(actual => !Interactables.Contains(actual)))
                {
                    newInteractable?.Hover();
                }

                foreach (var lostInteractable in Interactables.Where(current => !actualInteractables.Contains(current)))
                {
                    lostInteractable?.Normal();
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
        }
        
        private void Awake()
        {
            Validate();
        }

        private void Update()
        {
            if (Observer.IsEnabled) UpdateInteracable();
        }

        private void OnValidate()
        {
            Validate();
        }
    }
}