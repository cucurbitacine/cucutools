using System;
using System.Linq;
using UnityEngine;

namespace CucuTools.Interactables.Pickables
{
    public class PickerController : MonoBehaviour
    {
        public InteractableObserver Observer => observer;

        [Header("Info")]
        [SerializeField] private PickableBehaviour observerPickable;
        [SerializeField] private PickableBehaviour actualPickable;
        
        [Header("References")]
        [SerializeField] private InteractableObserver observer;

        public void Switch()
        {
            if (actualPickable != null)
            {
                if (Throw(actualPickable))
                {
                    actualPickable = null;
                }
            }
            else
            {
                if (Pick(observerPickable))
                {
                    actualPickable = observerPickable;
                }
            }
        }
        
        public bool Pick(PickableBehaviour pickable)
        {
            return pickable?.Pick(this) ?? false;
        }

        public bool Throw(PickableBehaviour pickable)
        {
            return pickable?.Throw(this) ?? false;
        }

        private void Update()
        {
            if (Observer == null) return;

            if (actualPickable != null)
            {
                if (actualPickable.Ownership.Owner != this)
                {
                    actualPickable = null;
                }
            }
            
            Observer.IsEnabled = actualPickable == null;
            
            if (Observer.IsEnabled)
                observerPickable = Observer.Interactables?.OfType<PickableBehaviour>().FirstOrDefault();
        }
    }
}