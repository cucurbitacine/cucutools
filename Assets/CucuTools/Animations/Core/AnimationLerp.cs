using UnityEngine;

namespace CucuTools
{
    public class AnimationLerp : AnimationEntity
    {
        public LerpableEntity Target
        {
            get => _target;
            set
            {
                _target = value;
                UpdateEntity();
            }
        }

        [Header("Lerpable target")]
        [SerializeField] private LerpableEntity _target;

        [SerializeField] private bool looped;
        [SerializeField] private bool reverseLoop;
        
        protected override bool UpdateEntityInternal()
        {
            if (_target == null) return false;
            
            _target.Lerp(LerpValue);
            return true;
        }

        protected override void OnAwake()
        {
            if (looped) OnAnimationStop.AddListener(StartAnimation);
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (reverseLoop) looped = true;
        }
    }
}