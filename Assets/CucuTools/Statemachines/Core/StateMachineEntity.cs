namespace CucuTools
{
    public abstract class StateMachineEntity : StateEntity
    {
        public abstract StateEntity Current { get; }
    }
}