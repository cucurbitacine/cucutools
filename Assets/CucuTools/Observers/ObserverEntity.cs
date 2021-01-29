using System.Collections.Concurrent;
using UnityEngine;

namespace CucuTools
{
    public class ObserverEntity : MonoBehaviour, IObserverEntity
    {
        private readonly ConcurrentDictionary<IListenerEntity, bool> _listeners =
            new ConcurrentDictionary<IListenerEntity, bool>();

        public bool IsSubscribed(IListenerEntity listener)
        {
            return _listeners.ContainsKey(listener);
        }

        public virtual void Subscribe(IListenerEntity listener)
        {
            if (!IsSubscribed(listener))
                _listeners.TryAdd(listener, true);
        }

        public virtual void Unsubscribe(IListenerEntity listener)
        {
            if (IsSubscribed(listener))
                _listeners.TryRemove(listener, out _);
        }

        public virtual void UnsubscribeAll()
        {
            _listeners.Clear();
        }

        public virtual void Silence(IListenerEntity listener)
        {
            if (IsSubscribed(listener))
                _listeners[listener] = false;
        }

        public virtual void Unsilence(IListenerEntity listener)
        {
            if (IsSubscribed(listener))
                _listeners[listener] = true;
        }

        [ContextMenu(nameof(UpdateObserver))]
        public virtual void UpdateObserver()
        {
            foreach (var pair in _listeners)
            {
                try
                {
                    if (pair.Value) pair.Key?.OnObserverUpdated();
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}