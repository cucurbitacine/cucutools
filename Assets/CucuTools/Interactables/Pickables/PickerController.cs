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

        public void SwitchPick()
        {
            if (actualPickable != null)
            {
                if (TryThrow(actualPickable))
                {
                    actualPickable = null;
                }
            }
            else
            {
                if (TryPick(observerPickable))
                {
                    actualPickable = observerPickable;
                }
            }
        }
        
        public bool TryPick(PickableBehaviour pickable)
        {
            return pickable?.Pick(this) ?? false;
        }

        public bool TryThrow(PickableBehaviour pickable)
        {
            return pickable?.Throw(this) ?? false;
        }

        public void Pick(PickableBehaviour pickable)
        {
            TryPick(pickable);
        }

        public void Throw(PickableBehaviour pickable)
        {
            TryThrow(pickable);
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