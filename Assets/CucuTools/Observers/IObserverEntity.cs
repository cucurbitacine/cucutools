namespace CucuTools
{
    public interface IObserverEntity
    {
        bool IsSubscribed(IListenerEntity listener);
        
        void Subscribe(IListenerEntity listener);
        void Unsubscribe(IListenerEntity listener);
        void UnsubscribeAll();
    
        void Silence(IListenerEntity listener);
        void Unsilence(IListenerEntity listener);
        
        void UpdateObserver();
    }
    
    public interface IListenerEntity
    {
        void OnObserverUpdated();
    }
}