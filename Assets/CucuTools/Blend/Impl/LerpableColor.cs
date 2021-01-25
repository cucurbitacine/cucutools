using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc />
    public class LerpableColor : LerpableEntity<Color>
    {
        public Gradient Gradient
        {
            get => _gradient ?? (_gradient = GetDefaultGradient());
            set
            {
                _gradient = value;
                UpdateEntity();
            }
        }

        [Header("Gradient")]
        [SerializeField] private Gradient _gradient;

        protected override bool UpdateEntityInternal()
        {
            Result = Gradient.Evaluate(LerpValue);
            return true;
        }

        private static Gradient GetDefaultGradient()
        {
            var gradient = new Gradient();
            gradient.colorKeys = new[] {new GradientColorKey(Color.white, 0f),};
            gradient.alphaKeys = new[] {new GradientAlphaKey(1f, 0f),};
            gradient.mode = GradientMode.Blend;

            return gradient;
        }
        
        protected override void OnValidate()
        {
            base.OnValidate();

            if (_gradient == null) _gradient = GetDefaultGradient();
        }
    }
}