using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc />
    [AddComponentMenu(LerpMenuRoot + nameof(LerpColor))]
    public class LerpColor : LerpBehavior<Color>
    {
        public Gradient Gradient
        {
            get => gradient ?? (gradient = GetDefaultGradient());
            set
            {
                gradient = value;
                OnObserverUpdated();
            }
        }

        [Header("Gradient")]
        [SerializeField] private Gradient gradient;

        /// <inheritdoc />
        protected override bool UpdateBehaviour()
        {
            Value = Gradient.Evaluate(LerpValue);
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

            if (gradient == null) gradient = GetDefaultGradient();
        }
    }
}