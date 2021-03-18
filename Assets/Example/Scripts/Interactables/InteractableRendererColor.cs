using System;
using CucuTools.Colors;
using CucuTools.Interactables;
using UnityEngine;

namespace Example.Scripts.Interactables
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

        protected override void Awake()
        {
            base.Awake();
            
            Validate();
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            
            Validate();
        }

        public void SetColorRed()
        {
            SetColorRoot(Color.red);
        }

        public void SetColorGreen()
        {
            SetColorRoot(Color.green);
        }
        
        public void SetColorBlue()
        {
            SetColorRoot(Color.blue);
        }
        
        public void SetColorRoot(Color color)
        {
            this.color.normal = color;
            this.color.hovered = (color.ToVector3() * (205f / 255f)).ToColor();
            this.color.clicked = (color.ToVector3() * (155f / 255f)).ToColor();
            this.color.disabled = (color.ToVector3() * (105f / 255f)).ToColor();

            UpdateColor();
        }

        public void UpdateColor()
        {
            if (IsEnabled)
            {
                if (state.isNormal) SetColor(color.normal);
                if (state.isHovered) SetColor(color.hovered);
                if (state.isClicked) SetColor(color.clicked);
            }
            else SetColor(color.disabled);
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
