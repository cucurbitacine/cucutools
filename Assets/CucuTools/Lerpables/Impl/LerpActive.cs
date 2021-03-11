using UnityEngine;

namespace CucuTools.Lerpables.Impl
{
    /// <inheritdoc />
    [AddComponentMenu(LerpMenuRoot + nameof(LerpActive))]
    public class LerpActive : LerpBool
    {
        [Header("Target game object")]
        [SerializeField] private GameObject target;

        /// <inheritdoc />
        protected override bool UpdateBehaviour()
        {
            if (!base.UpdateBehaviour()) return false;

            if (target == null) return false;

            try
            {
                target.SetActive(Value);
            }
            catch
            {
                return false;
            }

            return true;
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (target == null) return;

            if (Value) return;

            var tr = target.transform;

            var sharedMesh = tr.GetComponent<MeshFilter>()?.sharedMesh;
            
            if (sharedMesh == null) return;
            
            Gizmos.color = Color.gray;
            Gizmos.DrawWireMesh(sharedMesh, tr.position, tr.rotation, tr.lossyScale);
        }
    }
}