using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc />
    public class RaycastEffectShowHit : RaycastEffectBase
    {
        [Header("ShowHit")]
        [SerializeField] private ParticleSystem effect;

        /// <inheritdoc />
        public override void UpdateEffect()
        {
            var hasHit = Raycaster.Raycast(out var hit);

            if (hasHit)
            {
                effect.transform.position = hit.point + hit.normal * 0.001f;
                effect.transform.up = hit.normal;
                
                effect.gameObject.SetActive(true);
                effect.Play();
            }
            else
            {
                effect.Stop();
                effect.gameObject.SetActive(false);
            }
        }
    }
}