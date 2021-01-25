using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc />
    [AddComponentMenu(AnimLerpMenuRoot + nameof(AnimationLerp))]
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

        /// <inheritdoc />
        protected override bool UpdateEntityInternal()
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