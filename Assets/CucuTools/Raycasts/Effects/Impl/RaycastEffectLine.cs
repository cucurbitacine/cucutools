using UnityEngine;

namespace CucuTools.Raycasts.Effects.Impl
{
    public class RaycastEffectLine : RaycastEffectBase
    {
        [Header("Line settings")]
        [SerializeField] protected LineRenderer line;
        [SerializeField] protected Transform lineOrigin;

        private RaycastHit _cachedHit;

        /// <inheritdoc />
        public override void UpdateEffect()
        {
            UpdateLine(Raycaster.Raycast(out _cachedHit), _cachedHit);
        }

        /// <summary>
        /// Обновить линию
        /// </summary>
        /// <param name="hasHit">Был ли удар</param>
        /// <param name="hit">Информация о ударе</param>
        protected virtual void UpdateLine(bool hasHit, RaycastHit hit)
        {
            if (hasHit)
            {
                line.enabled = true;

                line.SetPosition(0, lineOrigin.position);
                line.SetPosition(1, hit.point);
            }
            else line.enabled = false;
        }

        protected override void Validate()
        {
            base.Validate();
            
            if (line == null) line = GetComponent<LineRenderer>();
            if (line != null) line.useWorldSpace = true;
            else Debug.LogError($"{gameObject.name} was not found {nameof(LineRenderer)}");
        }
    }
}