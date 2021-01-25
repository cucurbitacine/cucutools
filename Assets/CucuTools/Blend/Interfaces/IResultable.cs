namespace CucuTools
{
    public interface IResultable<out TResult>
    {
        public TResult Result { get; }
    }
}