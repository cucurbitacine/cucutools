using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools.Observers
{
    public class ListenerEntity : IListenerEntity
    {
        private readonly Action action;

        public ListenerEntity(Action action)
        {
            this.action = action;
        }

        protected ListenerEntity()
        {
            action = null;
        }
        
        public void OnObserverUpdated()
        {
            action?.Invoke();
        }
    }
    
    [Serializable]
    public class ListenerEntity<TValue> : IListenerEntity<TValue>
    {
        public TValue Value
        {
            get => value;
            set => this.value = value;
        }

        [SerializeField] private TValue value;

        private IObserverEntity<TValue> _observer;

        public ListenerEntity(IObserverEntity<TValue> observer)
        {
            _observer = observer;
        }

        protected ListenerEntity()
        {
            _observer = null;
        }
        
        public void OnObserverUpdated()
        {
            if (_observer != null)
                Value = _observer.Value;
        }
    }

    public static class ListenerExtenstions
    {
        public static void Subscribe(this IListenerEntity listener, IObserverEntity observer)
        {
            observer.Subscribe(listener);
        }
        
        public static void Subscribe(this IEnumerable<IListenerEntity> listeners, IObserverEntity observer)
        {
            observer.Subscribe(listeners.ToArray());
        }
        
        public static void Unsubscribe(this IListenerEntity listener, IObserverEntity observer)
        {
            observer.Unsubscribe(listener);
        }
        
        public static void Unsubscribe(this IEnumerable<IListenerEntity> listeners, IObserverEntity observer)
        {
            observer.Unsubscribe(listeners.ToArray());
        }
        
        public static void Silence(this IListenerEntity listener, IObserverEntity observer)
        {
            observer.Silence(listener);
        }
        
        public static void Silence(this IEnumerable<IListenerEntity> listeners, IObserverEntity observer)
        {
            observer.Silence(listeners.ToArray());
        }
        
        public static void Unsilence(this IListenerEntity listener, IObserverEntity observer)
        {
            observer.Unsilence(listener);
        }
        
        public static void Unsilence(this IEnumerable<IListenerEntity> listeners, IObserverEntity observer)
        {
            observer.Unsilence(listeners.ToArray());
        }
    }
}