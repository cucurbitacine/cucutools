namespace CucuTools
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

    public interface IObserverEntity<out TValue> : IObserverEntity, IGetValue<TValue>
    {
    }
    
    public interface IListenerEntity<TValue> : IListenerEntity, IHaveValue<TValue>
    {
    }
}