using UnityEngine;

namespace CucuTools.Blend.Impl
{
    /// <inheritdoc />
    public class CucuBlendColorGradient : CucuBlendEntity
    {
        public override string Key => "Color gradient blend";
        
        public Color Color => _color;

        [Header("Result")]
        [SerializeField] private Color _color;
        
        [Header("Gradient")]
        [SerializeField] private Gradient _gradient;

        protected override void UpdateEntity()
        {
            _color = _gradient.Evaluate(Blend);
        }

        public void SetGradient(Gradient gradient)
        {
            _gradient = gradient;
            UpdateEntity();
        }
    }
}