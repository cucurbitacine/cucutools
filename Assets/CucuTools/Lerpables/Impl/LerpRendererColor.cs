using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc />
    [AddComponentMenu(LerpMenuRoot + nameof(LerpRendererColor))]
    public class LerpRendererColor : LerpColor
    {
        public Renderer Renderer
        {
            get => rendererTarget;
            set => rendererTarget = value;
        }
        
        [Header("Renderer")]
        [SerializeField] private Renderer rendererTarget;
        
        /// <inheritdoc />
        protected override bool UpdateBehaviour()
        {
            if (!base.UpdateBehaviour()) return false;

            if (rendererTarget == null) return false;

            if (Application.isPlaying)
            {
                rendererTarget.material.color = Value;
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