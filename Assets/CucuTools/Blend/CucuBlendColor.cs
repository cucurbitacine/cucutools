using CucuTools.Attributes;
using UnityEngine;

namespace CucuTools.Blend
{
    public class CucuBlendColor : CucuBlend
    {
        public Color Color
        {
            get => color;
            private set => color = value;
        }

        public Gradient Gradient
        {
            get => gradient ?? (gradient = new Gradient());
            set
            {
                gradient = value;
                OnBlendChange();
            }
        }

        [Header("Color")]
        [CucuReadOnly]
        [SerializeField] private Color color;
        [SerializeField] private Gradient gradient;

        public override void OnBlendChange()
        {
            base.OnBlendChange();
            
            color = Gradient.Evaluate(Blend);
        }
    }
}