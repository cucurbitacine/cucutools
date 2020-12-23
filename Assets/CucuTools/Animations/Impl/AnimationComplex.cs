using System.Linq;
using UnityEngine;

namespace CucuTools
{
    public class AnimationComplex : CucuAnimationEntity
    {
        protected const string GroupBlockName = "Stack";
        
        public enum AnimationStartType
        {
            Queue,
            Parallel,
        }

        public AnimationStartType StartType => startType;
        public CucuAnimationEntity[] Animations => animations ?? (animations = new CucuAnimationEntity[] { });

        public override float AnimationTime
        {
            get
            {
                base.AnimationTime = 0f;

                if (StartType == AnimationStartType.Queue)
                    base.AnimationTime = Animations.Where(a => a != null && a != this).Sum(a => a.AnimationTime);

                if (StartType == AnimationStartType.Parallel)
                    base.AnimationTime = Animations.Where(a => a != null && a != this).Max(a => a.AnimationTime);
                
                return base.AnimationTime;
            }
            protected set => base.AnimationTime = value;
        }

        public override float AnimationTimeScale
        {
            get => base.AnimationTimeScale;
            set
            {
                base.AnimationTimeScale = value;

                foreach (var animationBase in Animations)
                {
                    if (animationBase == null) continue;
                    animationBase.AnimationTimeScale = AnimationTimeScale;
                }
            }
        }

        [Header("Block settings")]
        [SerializeField] private AnimationStartType startType;
        
        [Header("Animations")]
        [SerializeField] private CucuAnimationEntity[] animations;

        private int _indexCurrent;

        protected override bool StartAnimationInternal()
        {
            if (Animations.Length == 0) return false;

            Default();
            
            if (StartType == AnimationStartType.Queue)
            {
                _indexCurrent = -1;
                
                return TryStartNext();
            }

            if (StartType == AnimationStartType.Parallel)
            {
                _indexCurrent = 0;
                
                foreach (var animationBase in Animations)
                {
                    if (animationBase == null) continue;
                    animationBase.OnAnimationStop.AddListener(OnAnimationElementEnd);
                    animationBase.AnimationTimeScale = AnimationTimeScale;
                    animationBase.StartAnimation();
                }

                return true;
            }

            return false;
        }

        protected override void StopAnimationInternal()
        {
            foreach (var animationEntity in Animations)
            {
                if (animationEntity == null) continue;
                animationEntity.OnAnimationStop.RemoveListener(OnAnimationElementEnd);
                animationEntity.StopAnimation();
            }
        }

        protected override void DefaultInternal()
        {
            if (StartType == AnimationStartType.Parallel)
            {
                foreach (var animationEntity in Animations)
                {
                    animationEntity?.Default();
                }
            }

            if (StartType == AnimationStartType.Queue)
            {
                Animations.FirstOrDefault()?.Default();
            }
        }

        protected override void OnStart()
        {
            Default();
        }

        protected override void Validate()
        {
            base.Validate();

            animations = Animations.Where(a => a != this).ToArray();
            
            progressDisplay = Blend;
        }

        private bool TryStartNext()
        {
            if (_indexCurrent >= 0) Animations[_indexCurrent]?.OnAnimationStop.RemoveListener(OnAnimationElementEnd);
            
            _indexCurrent++;
            
            if (_indexCurrent < animations.Length)
            {
                if (Animations[_indexCurrent] == null)
                    return TryStartNext();
                
                Animations[_indexCurrent].OnAnimationStop.AddListener(OnAnimationElementEnd);
                
                Animations[_indexCurrent].AnimationTimeScale = AnimationTimeScale;
                Animations[_indexCurrent].StartAnimation();

                return true;
            }

            return false;
        }
        
        private void OnAnimationElementEnd()
        {
            if (StartType == AnimationStartType.Queue)
            {
                if (!TryStartNext()) StopAnimation();
                return;
            }

            if (StartType == AnimationStartType.Parallel)
            {
                _indexCurrent++;
                if (Animations.Length == _indexCurrent)
                {
                    StopAnimation();
                }
            }
        }

        [CucuButton("Find children", group: GroupBlockName)]
        private void FindChildren()
        {
            animations = GetComponentsInChildren<CucuAnimationEntity>()
                .Where(c => c != this && c.transform.parent == transform).ToArray();
        }
        

        private void Update()
        {
            if (Playing)
            {
                CurrentTime += Time.deltaTime;
                progressDisplay = Blend;
            }
        }
    }
}