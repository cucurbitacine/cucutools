using System.Collections.Generic;

namespace CucuTools
{
    /// <summary>
    /// Configuration of pins
    /// </summary>
    /// <typeparam name="TBlendPin">Blend pin type</typeparam>
    public interface IBlendPinConfig<TBlendPin>
    {
        /// <summary>
        /// Key of config
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Pins
        /// </summary>
        List<TBlendPin> Pins { get; }
    }
}