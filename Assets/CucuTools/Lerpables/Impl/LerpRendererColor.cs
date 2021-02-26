using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc />
    [AddComponentMenu(LerpMenuRoot + nameof(LerpRendererColor))]
    public class LerpRendererColor : LerpColor
    {
        public Renderer Renderer
        {
            get => renderer;
            set => renderer = value;
        }
        
        [Header("Renderer")]
        [SerializeField] private Renderer renderer;
        
        /// <inheritdoc />
        protected override bool UpdateBehaviour()
        {
            if (!base.UpdateBehaviour()) return false;

            if (renderer == null) return false;

            if (Application.isPlaying)
            {
                renderer.material.color = Value;
            }

            return true;
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (Renderer == null) Renderer = GetComponent<Renderer>();
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            
            if (Application.isPlaying) return;

            if (Renderer == null) return;

            var sharedMesh = Renderer.GetComponent<MeshFilter>()?.sharedMesh;
            
            if (sharedMesh == null) return;

            var tr = Renderer.transform;

            Gizmos.color = Value;
            Gizmos.DrawWireMesh(sharedMesh, tr.position, tr.rotation, tr.lossyScale);
        }
    }
}