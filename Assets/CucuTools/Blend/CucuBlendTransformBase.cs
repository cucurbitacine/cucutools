using UnityEngine;

namespace CucuTools.Blend
{
    public abstract class CucuBlendTransformBase : CucuBlendEntity
    {
        public virtual Transform Target
        {
            get => target;
            set
            {
                target = value;

                UpdateEntity();
            }
        }

        [Header("Target")]
        [SerializeField] private Transform target;

        protected override void OnValidate()
        {
            if (Target == null) Target = transform;

            base.OnValidate();
        }
    }

    public abstract class InfoParamBase<T>
    {
        public abstract T Evaluate(float t);
    }
}