using UnityEngine;

namespace CucuTools
{
    public class AnimationMaterialSetFloat : CucuAnimationBehaviour
    {
        [Header("Float parameter")]
        [SerializeField] private string floatName;
        
        [SerializeField] private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        
        [Header("References")]
        [SerializeField] private Renderer renderer;
        
        protected override void LerpInternal(float t)
        {
            if (renderer == null) return;
            if (string.IsNullOrEmpty(floatName)) return;
            
            renderer.material.SetFloat(floatName, curve.Evaluate(t));
        }

        protected override void OnAwake()
        {
            if (renderer == null) renderer = GetComponent<Renderer>();
        }
    }
}
