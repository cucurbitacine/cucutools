using System.Linq;
using UnityEngine;

namespace CucuTools
{
    public class TransitionBehaviour : TransitionEntity
    {
        public override bool IsReady => Mode == ConditionMode.All ? Conditions.All(c => c.Done) : Conditions.Any(c => c.Done);

        public override StateEntity Target
        {
            get => target;
            set => target = value;
        }

        public override StateEntity Owner => GetOwner();

        public ConditionMode Mode
        {
            get => mode;
            set => mode = value;
        }

        public ConditionEntity[] Conditions
        {
            get => conditions;
            set => conditions = value;
        }

        [SerializeField] private StateEntity target;
        [SerializeField] private ConditionMode mode;
        [SerializeField] private ConditionEntity[] conditions;

        private StateEntity _ownerCache;
        
        [CucuButton]
        private void Setup()
        {
            conditions = GetComponentsInChildren<ConditionEntity>();
        }

        private StateEntity GetOwner()
        {
            return _ownerCache != null ? _ownerCache : (_ownerCache = GetOwner(transform));
        }
        
        private static StateEntity GetOwner(Transform root)
        {
            if (root == null) return null;

            var state = root.GetComponent<StateEntity>();

            if (state != null) return state;

            return GetOwner(root.parent);
        }
        
        protected virtual void Awake()
        {
            Setup();
        }
        
        public enum ConditionMode
        {
            All,
            Any
        }
    }
}