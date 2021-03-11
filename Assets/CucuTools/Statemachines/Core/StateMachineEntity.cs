namespace CucuTools.Statemachines.Core
{
    public abstract class StateMachineEntity : StateEntity
    {
        public abstract StateEntity Current { get; }
    }
}