using System.Linq;
using UnityEngine;

namespace CucuTools
{
    public class StateMachineBehaviour : StateMachineEntity
    {
        public override bool IsPlaying => isPlaying;

        public override bool IsLast => Current.IsLast && ((Transitions?.Length ?? 0) == 0);

        public override StateEntity Current => current;

        public TransitionEntity[] Transitions
        {
            get => transitions;
            set => transitions = value;
        }

        [SerializeField] private bool isPlaying;
        [SerializeField] private StateEntity current;
        [SerializeField] private TransitionEntity[] transitions;
        
        private StateTrigger[] _triggers;
        
        public override bool TryGetNextState(out StateEntity nextState)
        {
            nextState = null;
            return IsPlaying && Current.IsLast && ((nextState = Transitions?.FirstOrDefault(t => t.IsReady)?.Target) != null);
        }

        [CucuButton("Start", group:"State")]
        public override void StartState()
        {
            if (IsPlaying) return;

            isPlaying = true;
            
            Current.StartState();
        }

        [CucuButton("Stop", group:"State")]
        public override void StopState()
        {
            if (!IsPlaying) return;

            isPlaying = false;
            
            Current.StopState();
        }

        [CucuButton]
        private void Setup()
        {
            SetupTransitions();
            SetupTriggers();
        }
        
        private void SetupTransitions()
        {
            transitions = GetComponentsInChildren<TransitionEntity>()
                .Where(t => t.Owner == this)
                .ToArray();
        }
        
        private void SetupTriggers()
        {
            _triggers = GetComponentsInChildren<StateTrigger>()
                .Where(t => t.Owner == this)
                .ToArray();
        }
        
        private void UpdateStateMachine()
        {
            if (Current.IsLast)
            {
                return;
            }

            if (Current.TryGetNextState(out var nextState))
            {
                Current.StopState();
                current = nextState;
                Current.StartState();
            }
        }

        protected virtual void Awake()
        {
            Setup();
        }
        
        private void Update()
        {
            if (IsPlaying) UpdateStateMachine();
        }
    }
}