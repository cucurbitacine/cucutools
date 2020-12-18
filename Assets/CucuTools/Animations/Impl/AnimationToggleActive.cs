using UnityEngine;

namespace CucuTools
{
    public class AnimationToggleActive : CucuAnimationBehaviour
    {
        [SerializeField, Min(0f)] private float timeToggle;
        [SerializeField] private bool targetActive;
        [SerializeField] private GameObject target;

        protected override void LerpInternal(float t)
        {
            if (target == null) return;

            if (t == 0f)
            {
                target.SetActive(!targetActive);
                return;
            }
            
            if (CurrentTime >= timeToggle)
                target.SetActive(targetActive);
        }

        protected override void Validate()
        {
            base.Validate();

            if (timeToggle < 0f) timeToggle = 0f;
            if (timeToggle > AnimationTime) timeToggle = AnimationTime;

            if (timeToggle == 0f && AnimationTime > 0f) timeToggle = AnimationTime / 2f;
        }
    }
}