using System;
using UnityEngine;

namespace CucuTools.Interactables.Pickables
{
    [DisallowMultipleComponent]
    public class PickableBehaviour : InteractableBehavior
    {
        public bool Picked
        {
            get => picked;
            private set => picked = value;
        }

        public OwnershipSettings Ownership => ownership ?? (ownership = new OwnershipSettings());
        
        public PickableEffect[] Effects => effects;

        [SerializeField] private bool picked;

        [SerializeField] private OwnershipSettings ownership;
        
        [Header("Effects")]
        [SerializeField] private PickableEffect[] effects;
        
        public bool Pick(PickerController picker)
        {
            if (Ownership.Owner == null)
            {
                Ownership.Owner = picker;

                PickInternal(picker);
                
                return true;
            }

            if (Ownership.HaveRights) return false;

            if (Ownership.Owner == picker) return false;
            
            Ownership.Owner = picker;

            PickInternal(picker);

            return true;
        }
        
        public bool Throw(PickerController picker)
        {
            if (Ownership.Owner == null) return false;

            if (Ownership.Owner != picker) return false;
            
            Ownership.Owner = null;
                
            ThrowInternal(picker);

            return true;
        }

        private void PickInternal(PickerController picker)
        {
            foreach (var effect in Effects)
            {
                effect.Pick();
            }
            
            Picked = true;
        }
        
        private void ThrowInternal(PickerController picker)
        {
            foreach (var effect in Effects)
            {
                effect.Throw();
            }
            
            Picked = false;
        }
        
        private void Validate()
        {
            effects = GetComponents<PickableEffect>();
        }
        
        protected override void Awake()
        {
            base.Awake();

            Validate();
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            Validate();
        }
        
        [Serializable]
        public class OwnershipSettings
        {
            public bool HaveRights
            {
                get => haveRights;
                set => haveRights = value;
            }

            public PickerController Owner
            {
                get => owner;
                set
                {
                    lastOwner = owner;
                    owner = value;
                }
            }

            public PickerController LastOwner => lastOwner;
            
            [Header("Ownership")]
            [SerializeField] private bool haveRights;
            [SerializeField] private PickerController owner;
            [SerializeField] private PickerController lastOwner;
            
            public OwnershipSettings(bool haveRights)
            {
                this.haveRights = haveRights;
            }
            
            public OwnershipSettings() : this(true)
            {
            }
        }
    }
}