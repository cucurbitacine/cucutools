using UnityEngine;

namespace CucuTools
{
    public class AnimationRendererColor : CucuAnimationBehaviour
    {
        [Header("Color")]
        [SerializeField] private Gradient gradient;
        
        [Header("References")]
        [SerializeField] private Renderer target;

        protected override void LerpInternal(float t)
        {
            SetColor(GetColor(t));
        }

        protected override void Validate()
        {
            base.Validate();
            
            if (target == null) target = GetComponent<Renderer>();
        }
        
        private Color GetColor(float t)
        {
            if (gradient == null) return Color.magenta;
            return gradient.Evaluate(t);
        }
        
        private void SetColor(Color color)
        {
            if (target == null) return;
            
            target.material.color = color;
        }
    }
}