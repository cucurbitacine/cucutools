using UnityEngine;
using UnityEngine.UI;

namespace CucuTools
{
    public class AnimationTextPrinting : CucuAnimationBehaviour
    {
        [Header("Text")]
        [SerializeField] private string text;
        
        [Header("References")]
        [SerializeField] private Text uiText;

        protected override void LerpInternal(float t)
        {
            if (t >= 1f)
            {
                uiText.text = text;
                return;
            }
            
            var count = (int) Mathf.Lerp(0f, text.Length, t);
            uiText.text = text.Substring(0, Mathf.Clamp(count, 0, text.Length));
        }
    }
}