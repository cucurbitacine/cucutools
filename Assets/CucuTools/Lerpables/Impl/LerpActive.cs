using UnityEngine;

namespace CucuTools
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
    }
}