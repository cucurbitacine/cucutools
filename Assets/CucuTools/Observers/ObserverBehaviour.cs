using UnityEngine;

namespace CucuTools
{
    public class ObserverBehaviour : MonoBehaviour, IObserverEntity
    {
        public ObserverEntity ObserverEntity { get; } = new ObserverEntity();

        public virtual bool IsSubscribed(IListenerEntity listener)
        {
            return ObserverEntity.IsSubscribed(listener);
        }

        public void Subscribe(params IListenerEntity[] listeners)
        {
            ObserverEntity.Subscribe(listeners);
        }

        public void Unsubscribe(params IListenerEntity[] listeners)
        {
            ObserverEntity.Unsubscribe(listeners);
        }

        public virtual void Subscribe(IListenerEntity listener)
        {
            ObserverEntity.Subscribe(listener);
        }

        public virtual void Unsubscribe(IListenerEntity listener)
        {
            ObserverEntity.Unsubscribe(listener);
        }

        public virtual void UnsubscribeAll()
        {
            ObserverEntity.UnsubscribeAll();
        }

        public void Silence(params IListenerEntity[] listeners)
        {
            ObserverEntity.Silence(listeners);
        }

        public void Unsilence(params IListenerEntity[] listeners)
        {
            ObserverEntity.Unsilence(listeners);
        }

        public virtual void Silence(IListenerEntity listener)
        {
            ObserverEntity.Silence(listener);
        }

        public virtual void Unsilence(IListenerEntity listener)
        {
            ObserverEntity.Unsilence(listener);
        }

        [ContextMenu(nameof(UpdateObserver))]
        public virtual void UpdateObserver()
        {
            ObserverEntity.Update();
        }
    }
}