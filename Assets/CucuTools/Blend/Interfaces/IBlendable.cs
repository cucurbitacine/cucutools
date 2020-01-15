namespace CucuTools.Blend.Interfaces
{
    /// <summary>
    /// Blendable entity
    /// </summary>
    public interface IBlendable
    {
        /// <summary>
        /// Key of blend entity
        /// </summary>
        string Key { get; }
        
        /// <summary>
        /// Blend value, between 0.0 and 1.0
        /// </summary>
        float Blend { get; }

        /// <summary>
        /// Set blend value
        /// </summary>
        void SetBlend(float blend);
    }
}