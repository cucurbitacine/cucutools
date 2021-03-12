using System.Collections;
using UnityEngine;

namespace Example.Scripts
{
    public class InteracableColor : InteracableBehavior
    {
        public Color normal;
        public Color hovered;
        public Color clicked;
    
        public bool needNormal;
        public bool needHovered;
        public bool wasClicled;

        public Renderer renderer;
    
        public override void Hover()
        {
            if (wasClicled)
            {
                needNormal = false;
                needHovered = true;
                return;
            }

            needHovered = false;
            
            SetColor(hovered);
        
            base.Hover();
        }

        public override void Normal()
        {
            if (wasClicled)
            {
                needHovered = false;
                needNormal = true;
                return;
            }
        
            needNormal = false;
            
            SetColor(normal);
        
            base.Normal();
        }

        public override void Click()
        {
            if (wasClicled) return;

            StartCoroutine(_Click());

            base.Click();
        
            IEnumerator _Click()
            {
                wasClicled = true;
                needHovered = true;
                
                SetColor(clicked);
            
                yield return new WaitForSeconds(0.2f);
            
                wasClicled = false;

                if (needHovered) Hover();
                else if (needNormal) Normal();
            }
        }

        private void SetColor(Color color)
        {
            renderer.material.color = color;
        }
    
        private void Validate()
        {
            if (renderer == null) renderer = GetComponent<Renderer>();
        }

        private void Awake()
        {
            Validate();
            
            Normal();
        }

        private void OnValidate()
        {
            Validate();
        }
    }
}
