namespace CucuTools
{
    /// <summary>
    /// Blendable entity
    /// </summary>
    public interface IBlendable
    {
        /// <summary>
        /// Blend value, between 0.0 and 1.0
        /// </summary>
        float Blend { get; }

        /// <summary>
        /// Set blend value
        /// </summary>
        void Lerp(float blend);
    }
}