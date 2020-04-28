using System.Collections.Generic;

namespace CucuTools
{
    /// <summary>
    /// Blending spline between pins
    /// </summary>
    /// <typeparam name="TPin">Type of pin</typeparam>
    public interface IBlendSpline<TObject> : IBlendable
    {
        /// <summary>
        /// Get list of pins
        /// </summary>
        /// <returns>List of Pins</returns>
        List<IBlendPin<TObject>> GetPins();

        /// <summary>
        /// Get local blend value in relation left and right pins 
        /// </summary>
        /// <param name="lefts">Output left pins</param>
        /// <param name="rights">Output rights pins</param>
        /// <returns>Local blend</returns>
        float GetLocalBlend(out IEnumerable<IBlendPin<TObject>> lefts, out IEnumerable<IBlendPin<TObject>> rights);
    }
}