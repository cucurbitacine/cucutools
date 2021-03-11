using UnityEngine;

namespace CucuTools.Lerpables.Animations
{
    /// <inheritdoc />
    [AddComponentMenu(AnimLerpMenuRoot + nameof(AnimationLerp))]
    public class AnimationLerp : AnimationBehaviour
    {
        public LerpBehavior Target
        {
            get => _target;
            set
            {
                _target = value;
                
                OnObserverUpdated();
            }
        }

        [Header("Lerpable target")]
        [SerializeField] private LerpBehavior _target;
        
        protected override bool UpdateBehaviour()
        {
            if (_target == null) return false;
            if (_target == this) return false;

            _target.Lerp(LerpValue);
            return true;
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (_target == this) _target = null;
        }
    }
}