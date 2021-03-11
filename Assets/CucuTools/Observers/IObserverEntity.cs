namespace CucuTools.Observers
{
    public interface IObserverEntity
    {
        bool IsSubscribed(IListenerEntity listener);

        void Subscribe(params IListenerEntity[] listeners);
        void Unsubscribe(params IListenerEntity[] listeners);
        void UnsubscribeAll();

        void Silence(params IListenerEntity[] listeners);
        void Unsilence(params IListenerEntity[] listeners);
    }

    public interface IListenerEntity
    {
        void OnObserverUpdated();
    }

    public interface IObserverEntity<out TValue> : IObserverEntity
    {
        TValue Value { get; }
    }
    
    public interface IListenerEntity<TValue> : IListenerEntity
    {
    }
}