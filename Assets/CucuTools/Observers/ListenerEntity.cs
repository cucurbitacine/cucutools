using UnityEngine;
using UnityEngine.Events;

namespace CucuTools
{
    public class ListenerEntity : MonoBehaviour, IListenerEntity
    {
        public UnityEvent ObserverUpdated => observerUpdated ?? (observerUpdated = new UnityEvent());
        
        public virtual ObserverEntity ObserverEntity
        {
            get => observerEntity;
            set
            {
                observerEntity?.Unsubscribe(this);
                observerEntity = value;
                observerEntity?.Subscribe(this);
            }
        }
        
        [Header("Observer")]
        [SerializeField] private ObserverEntity observerEntity;

        [Header("Event")]
        [SerializeField] private UnityEvent observerUpdated;
        
        public void OnObserverUpdated()
        {
            try
            {
                OnObserverUpdatedInternal();
            }
            catch
            {
                // ignored
            }

            ObserverUpdated.Invoke();
        }

        protected virtual void OnObserverUpdatedInternal()
        {
            
        }
        
        protected virtual void OnAwake()
        {
        }
        
        private void Awake()
        {
            ObserverEntity = ObserverEntity;

            OnAwake();
        }
    }
}