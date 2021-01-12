using System;
using UnityEditor;
using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc />
    public class RaycastEffectWithParticles : RaycastEffectBase
    {
        [Header("ShowHit")]
        [SerializeField] private bool hasHit;
        [SerializeField] private EffectUnit[] effectUnits;

        /// <inheritdoc />
        public override void UpdateEffect()
        {
            if (effectUnits == null) return;
            
            hasHit = Raycaster.Raycast(out var hit);

            if (hasHit)
            {
                foreach (var effectUnit in effectUnits)
                {
                    if (effectUnit.effect == null) continue;
                    if (!effectUnit.atHitPoint) continue;
                    
                    effectUnit.effect.transform.position = hit.point + hit.normal * 0.001f;

                    switch (effectUnit.normalDirection)
                    {
                        case EffectUnit.NormalDirection.Up:
                            effectUnit.effect.transform.up = hit.normal;
                            break;
                        case EffectUnit.NormalDirection.Forward:
                            effectUnit.effect.transform.forward = hit.normal;
                            break;
                        case EffectUnit.NormalDirection.Right:
                            effectUnit.effect.transform.right = hit.normal;
                            break;
                    }
                }
                
                Play();
            }
            else
            {
                Stop();
            }
        }

        [CucuButton(group: "Particles")]
        private void Play()
        {
            if (effectUnits == null) return;
            
            foreach (var effectUnit in effectUnits)
            {
                if (effectUnit.effect == null) continue;

                if (!effectUnit.effect.isPlaying)
                    effectUnit.effect.Play();
            }
        }
        
        [CucuButton(group: "Particles")]
        private void Stop()
        {
            if (effectUnits == null) return;
            
            foreach (var effectUnit in effectUnits)
            {
                if (effectUnit.effect == null) continue;

                if (effectUnit.effect.isPlaying)
                    effectUnit.effect.Stop();
            }
        }
        
        
    }
    
    [Serializable]
    public class  EffectUnit
    {
        public enum NormalDirection
        {
            Up,
            Forward,
            Right
        }
            
        public ParticleSystem effect;
        public bool atHitPoint;
        public NormalDirection normalDirection;
    }
}