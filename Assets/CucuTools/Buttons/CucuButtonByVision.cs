using UnityEngine;

namespace CucuTools
{
    [RequireComponent(typeof(Collider))]
    public class CucuButtonByVision : CucuButton
    {
        [Header("Settings")]
        [SerializeField, Range(0f, 100f)] private float maxDistance = 1.5f;

        [SerializeField] private string[] inputButtons = new[] {"Submit"};
        
        private void Update()
        {
            if (!Active) return;
            
            ClickHandle();
            FocusHandle();

            VirtualUpdate();
        }

        protected virtual void VirtualUpdate()
        {
            
        }
        
        private void ClickHandle()
        {
            foreach (var button in inputButtons)
            {
                if (Input.GetButtonDown(button)) TryClick();
            }
        }

        private void TryClick()
        {
            if (CucuVision.Instance == null) return;
            if (!CucuVision.Instance.TryGetTarget(out var info)) return;
            
            if (IsValidTarget(info)) Click();
        }

        private void FocusHandle()
        {
            if (CucuVision.Instance == null) return;
            if (!CucuVision.Instance.TryGetTarget(out var info))
            {
                Focus = false;
                return;
            }
            
            if (!IsValidTarget(info))
            {
                Focus = false;
                return;
            }

            Focus = true;
        }

        private bool IsValidTarget(VisionInfo target)
        {
            if (target.distance > maxDistance) return false;
            if (!target.IsThisIt(this)) return false;
            return true;
        } 
    }
}