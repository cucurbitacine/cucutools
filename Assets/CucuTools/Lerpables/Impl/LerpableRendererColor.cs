using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc />
    [AddComponentMenu(LerpMenuRoot + nameof(LerpableRendererColor))]
    public class LerpableRendererColor : LerpableColor
    {
        public Renderer Renderer
        {
            get => renderer;
            set => renderer = value;
        }
        
        [Header("Renderer")]
        [SerializeField] private Renderer renderer;
        
        /// <inheritdoc />
        protected override bool UpdateEntityInternal()
        {
            if (!base.UpdateEntityInternal()) return false;

            if (renderer == null) return false;
            
            renderer.material.color = Value;

            return true;
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (Renderer == null) Renderer = GetComponent<Renderer>();
        }
    }
}