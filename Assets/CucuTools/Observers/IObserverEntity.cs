namespace CucuTools
{
    public interface IObserverEntity
    {
        bool IsSubscribed(IListenerEntity listener);

        void Subscribe(params IListenerEntity[] listeners);
        void Unsubscribe(params IListenerEntity[] listeners);
        void Subscribe(IListenerEntity listener);
        void Unsubscribe(IListenerEntity listener);
        void UnsubscribeAll();

        void Silence(params IListenerEntity[] listeners);
        void Unsilence(params IListenerEntity[] listeners);
        void Silence(IListenerEntity listener);
        void Unsilence(IListenerEntity listener);

        void UpdateObserver();
    }

    public interface IListenerEntity
    {
        void OnObserverUpdated();
    }

    public interface IObserverEntity<out TValue> : IObserverEntity, IGetValue<TValue>
    {
    }
    
    public interface IListenerEntity<TValue> : IListenerEntity, IHaveValue<TValue>
    {
    }
}