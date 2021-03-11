using CucuTools.Statemachines.Core;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.Statemachines
{
    public class StateTrigger : MonoBehaviour
    {
        public StateEntity Owner => GetOwner();

        public InvokeMode Mode
        {
            get => mode;
            set => mode = value;
        }

        [SerializeField] private InvokeMode mode;
        [SerializeField] private UnityEvent onInvoke;

        private StateEntity _ownerCache;
        
        public void Invoke(InvokeMode mode)
        {
            if (Owner == null) return;
            if (Mode == mode) onInvoke?.Invoke();
        }

        private StateEntity GetOwner()
        {
            return _ownerCache != null ? _ownerCache : (_ownerCache = GetOwner(transform));
        }
        
        private static StateEntity GetOwner(Transform root)
        {
            if (root == null) return null;

            var state = root.GetComponent<StateEntity>();

            if (state != null) return state;

            return GetOwner(root.parent);
        }

        public enum InvokeMode
        {
            OnStart,
            OnStop,
        }
    }
}