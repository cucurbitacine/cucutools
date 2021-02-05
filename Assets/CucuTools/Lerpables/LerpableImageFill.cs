using UnityEngine;
using UnityEngine.UI;

namespace CucuTools
{
    public class LerpableImageFill : LerpableBehavior
    {
        [Header("Image")]
        [SerializeField] private Image image;

        [Header("Fill settings")]
        [SerializeField] private Image.FillMethod fillMethod;
        [SerializeField] private bool fillClockwise;
        
        protected override bool UpdateEntityInternal()
        {
            if (image == null) return false;
            
            image.fillAmount = LerpValue;

            return true;
        }

        private void Validate()
        {
            if (image == null) return;
            
            image.fillClockwise = fillClockwise;
            image.fillMethod = fillMethod;
        }
        
        private void Update()
        {
            Validate();
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            
            Validate();
        }
    }
}