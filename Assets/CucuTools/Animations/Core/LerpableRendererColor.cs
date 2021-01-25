using UnityEngine;

namespace CucuTools
{
    public class LerpableRendererColor : LerpableColor
    {
        [Header("Renderer")]
        [SerializeField] private Renderer _renderer;
        
        protected override bool UpdateEntityInternal()
        {
            if (!base.UpdateEntityInternal()) return false;
            
            _renderer.material.color = Result;

            return true;
        }
    }
}