using System;
using UnityEngine;

namespace CucuTools.Interactables
{
    public class InteractableRendererColor : InteractableBehavior
    {
        public Renderer rendererTarget;
        
        public InteractableColor color;
        
        protected override void NormalInternal()
        {
            base.NormalInternal();
            
            SetColor(color.normal);
        }

        protected override void HoverInternal()
        {
            base.HoverInternal();
            
            SetColor(color.hovered);
        }

        protected override void ClickInternal()
        {
            base.ClickInternal();
            
            SetColor(color.clicked);
        }

        protected override void DisableInternal()
        {
            base.DisableInternal();
            
            SetColor(color.disabled);
        }

        private void SetColor(Color c)
        {
            rendererTarget.material.color = c;
        }
    
        private void Validate()
        {
            if (rendererTarget == null) rendererTarget = GetComponent<Renderer>();
        }

        protected override void OnAwake()
        {
            Validate();
        }

        private void OnValidate()
        {
            Validate();
        }
        
        [Serializable]
        public class InteractableColor
        {
            public Color normal;
            public Color hovered;
            public Color clicked;
            public Color disabled;

            public InteractableColor()
            {
                normal = Color.white;
                hovered = new Color(205f / 255f, 205f / 255f, 205f / 255f, 1f);
                clicked = new Color(155f / 255f, 155f / 255f, 155f / 255f, 1f);
                disabled = new Color(105f / 255f, 105f / 255f, 105f / 255f, 0.5f);
            }
        }
    }
}
