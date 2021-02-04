namespace CucuTools
{
    public interface IHaveValue<TValue> : IGetValue<TValue>, ISetValue<TValue>
    {
    }

    public interface IGetValue<out TValue>
    {
        TValue Value { get; }
    }

    public interface ISetValue<in TValue>
    {
        TValue Value { set; }
    }
}