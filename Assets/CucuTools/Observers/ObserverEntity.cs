using System.Collections.Concurrent;

namespace CucuTools
{
    public class ObserverEntity : IObserverEntity
    {
        private readonly ConcurrentDictionary<IListenerEntity, bool> _listeners =
            new ConcurrentDictionary<IListenerEntity, bool>();

        public bool IsSubscribed(IListenerEntity listener)
        {
            return _listeners.ContainsKey(listener);
        }

        public void Subscribe(params IListenerEntity[] listeners)
        {
            if (listeners == null) return;
            foreach (var listener in listeners)
                Subscribe(listener);
        }

        public void Unsubscribe(params IListenerEntity[] listeners)
        {
            if (listeners == null) return;
            foreach (var listener in listeners)
                Unsubscribe(listener);
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

        public void Silence(params IListenerEntity[] listeners)
        {
            if (listeners == null) return;
            foreach (var listener in listeners)
                Silence(listener);
        }

        public void Unsilence(params IListenerEntity[] listeners)
        {
            if (listeners == null) return;
            foreach (var listener in listeners)
                Unsilence(listener);
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