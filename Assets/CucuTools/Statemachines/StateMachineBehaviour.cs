using System.Linq;
using CucuTools.Attributes;
using CucuTools.Statemachines.Core;
using UnityEngine;

namespace CucuTools.Statemachines
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

        [CucuButton("Start", @group:"State", colorHex:"00AA00")]
        public override void StartState()
        {
            if (IsPlaying) return;

            isPlaying = true;
            
            foreach (var trigger in _triggers)
            {
                trigger.Invoke(StateTrigger.InvokeMode.OnStart);
            }
            
            Current.StartState();
        }

        [CucuButton("Stop", @group:"State", colorHex:"AA0000")]
        public override void StopState()
        {
            if (!IsPlaying) return;

            isPlaying = false;
            
            foreach (var trigger in _triggers)
            {
                trigger.Invoke(StateTrigger.InvokeMode.OnStop);
            }
            
            Current.StopState();
        }

        [CucuButton(colorHex:"0000AA")]
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