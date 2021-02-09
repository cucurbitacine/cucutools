using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools
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

        private IGetValue<TValue> _resource;

        public ListenerEntity(IGetValue<TValue> resource)
        {
            _resource = resource;
        }

        protected ListenerEntity()
        {
            _resource = null;
        }
        
        public void OnObserverUpdated()
        {
            if (_resource != null)
                Value = _resource.Value;
        }
    }

    public static class ListenerExt
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