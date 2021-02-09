using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc />
    [AddComponentMenu(LerpMenuRoot + nameof(LerpableActive))]
    public class LerpableActive : LerpableBool
    {
        [Header("Target game object")]
        [SerializeField] private GameObject target;

        /// <inheritdoc />
        protected override bool UpdateEntityInternal()
        {
            if (!base.UpdateEntityInternal()) return false;

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
    }
}