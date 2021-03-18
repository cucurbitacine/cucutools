using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.Interactables.Pickables
{
    public class PickableEffectTrigger : PickableEffect
    {
        public UnityEvent TriggerEvents => triggerEvents ?? (triggerEvents = new UnityEvent());

        [Header("Events")]
        public InvokeType invokeType;
        [SerializeField] private UnityEvent triggerEvents;
        
        public void Invoke()
        {
            TriggerEvents.Invoke();
        }

        public void InvokeAs(InvokeType type)
        {
            if (CanInvoke(type)) Invoke();
        }
        
        public bool CanInvoke(InvokeType type)
        {
            if (!IsEnabled) return false; 
            
            switch (type)
            {
                case InvokeType.OnPick: return invokeType == InvokeType.OnPick;
                case InvokeType.OnStayPicked:  return invokeType == InvokeType.OnStayPicked && Pickable.Picked;
                case InvokeType.OnThrow:  return invokeType == InvokeType.OnThrow;
            }

            return false;
        }

        protected override void PickInternal()
        {
            InvokeAs(InvokeType.OnPick);
        }

        protected override void ThrowInternal()
        {
            InvokeAs(InvokeType.OnThrow);
        }

        private void Update()
        {
            InvokeAs(InvokeType.OnStayPicked);
        }

        public enum InvokeType
        {
            OnPick,
            OnStayPicked,
            OnThrow,
        }
    }
}