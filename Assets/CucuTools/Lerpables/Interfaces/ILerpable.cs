namespace CucuTools
{
    /// <summary>
    /// Lerpable entity
    /// </summary>
    public interface ILerpable
    {
        /// <summary>
        /// Lerp value, between 0.0 and 1.0
        /// </summary>
        float LerpValue { get; }

        /// <summary>
        /// Do linear interpolation by value
        /// </summary>
        /// <param name="lerpValue">Linear interpolation value. Must be between 0f and 1f</param>
        void Lerp(float lerpValue);
    }
}