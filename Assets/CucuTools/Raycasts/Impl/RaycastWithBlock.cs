using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc />
    public class RaycastWithBlock : RaycastBase
    {
        /// <summary>
        /// Слои объектов-помех
        /// </summary>
        public LayerMask LayerMaskBlock
        {
            get => layerMaskBlock;
            set => layerMaskBlock = value;
        }

        [Header("Block settings")]
        [SerializeField] private LayerMask layerMaskBlock;

        /// <inheritdoc />
        public override bool Raycast(out RaycastHit hit)
        {
            var hasHit = RaycastInternal(out hit, Distance, LayerMask | LayerMaskBlock);

            if (!hasHit) return false;

            if (((1 << hit.transform.gameObject.layer) & LayerMaskBlock) > 0) return false;

            return true;
        }
    }
}