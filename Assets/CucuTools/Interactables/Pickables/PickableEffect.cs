using UnityEngine;

namespace CucuTools.Interactables.Pickables
{
    [RequireComponent(typeof(PickableBehaviour))]
    public abstract class PickableEffect : MonoBehaviour
    {
        public bool IsEnabled
        {
            get => isEnabled;
            set => isEnabled = value;
        }

        public PickableBehaviour Pickable => pickable;
        
        [SerializeField] private bool isEnabled = true;
        [SerializeField] private PickableBehaviour pickable;

        public void Pick()
        {
            if (IsEnabled) PickInternal();
        }

        public void Throw()
        {
            if (IsEnabled) ThrowInternal();
        }

        protected abstract void PickInternal();
        
        protected abstract void ThrowInternal();
        
        private void Validate()
        {
            pickable = GetComponent<PickableBehaviour>();
        }
        
        protected virtual void Awake()
        {
            Validate();
        }

        protected virtual void OnValidate()
        {
            Validate();
        }
    }
}