using UnityEngine;
using UnityEngine.UI;

namespace CucuTools
{
    public class AnimationSpriteColor : CucuAnimationBehaviour
    {
        [Header("Color")]
        [SerializeField] private Gradient gradient;
        
        [Header("References")]
        [SerializeField] private Image target;

        protected override void LerpInternal(float t)
        {
            SetColor(GetColor(t));
        }

        protected override void Validate()
        {
            base.Validate();
            
            if (target == null) target = GetComponent<Image>();
        }
        
        private Color GetColor(float t)
        {
            if (gradient == null) return Color.magenta;
            return gradient.Evaluate(t);
        }
        
        private void SetColor(Color color)
        {
            if (target == null) return;
            
            target.color = color;
        }
    }
}