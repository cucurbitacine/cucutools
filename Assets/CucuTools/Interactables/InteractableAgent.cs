using System.Linq;
using UnityEngine;

namespace CucuTools.Interactables
{
    [DisallowMultipleComponent]
    public sealed class InteractableAgent : InteractableBehavior
    {
        public override bool IsEnabled
        {
            get => base.IsEnabled;
            set
            {
                base.IsEnabled = value;
                if (Target != null) Target.IsEnabled = base.IsEnabled;
            }
        }

        public override float ClickDuration
        {
            get => base.ClickDuration;
            set
            {
                base.ClickDuration = value;
                if (Target != null) Target.ClickDuration = base.ClickDuration;
            }
        }

        public InteractableBehavior Target
        {
            get => target;
            private set => target = value;
        }
        
        [SerializeField] private InteractableBehavior target;
        
        protected override void NormalInternal()
        {
            Target?.Normal();
        }

        protected override void HoverInternal()
        {
            Target?.Hover();
        }

        protected override void ClickInternal()
        {
            Target?.Click();
        }

        private void Validate()
        {
            if (Target == this) Target = null;
            if (Target == null) Target = GetComponentsInChildren<InteractableBehavior>()
                .FirstOrDefault(ib => ib != this);
            
            if (Target != null)
            {
                Target.DefaultOnAwake = false;
                Target.ClickDuration = ClickDuration;
            }
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
    }
}