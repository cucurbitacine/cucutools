namespace CucuTools.Blend.Interfaces
{
    /// <summary>
    /// Pin object with value for blend spline
    /// </summary>
    /// <typeparam name="TObject">Type of pin object</typeparam>
    public interface IBlendPin<out TObject>
    {
        /// <summary>
        /// Key of pin
        /// </summary>
        string Key { get; }
        
        /// <summary>
        /// Value
        /// </summary>
        float Value { get; }
        
        /// <summary>
        /// Pin object
        /// </summary>
        TObject Pin { get; }
    }
}