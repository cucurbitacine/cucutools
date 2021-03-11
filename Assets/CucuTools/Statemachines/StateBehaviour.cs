using System.Linq;
using CucuTools.Attributes;
using CucuTools.Statemachines.Core;
using UnityEngine;

namespace CucuTools.Statemachines
{
    public class StateBehaviour : StateEntity
    {
        public override bool IsPlaying => isPlaying;

        public override bool IsLast => (Transitions?.Length ?? 0) == 0;

        public TransitionEntity[] Transitions
        {
            get => transitions;
            set => transitions = value;
        }

        [SerializeField] private bool isPlaying;
        [SerializeField] private TransitionEntity[] transitions;
        
        private StateTrigger[] _triggers;
        
        public override bool TryGetNextState(out StateEntity nextState)
        {
            nextState = null;
            return IsPlaying && (nextState = Transitions?.FirstOrDefault(t => t.IsReady)?.Target) != null;
        }

        public override void StartState()
        {
            if (IsPlaying) return;
            
            isPlaying = true;

            foreach (var trigger in _triggers)
            {
                trigger.Invoke(StateTrigger.InvokeMode.OnStart);
            }
        }

        public override void StopState()
        {
            if (!IsPlaying) return;
            
            isPlaying = false;
            
            foreach (var trigger in _triggers)
            {
                trigger.Invoke(StateTrigger.InvokeMode.OnStop);
            }
        }

        [CucuButton(colorHex: "0000AA")]
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

        protected virtual void Awake()
        {
            Setup();
        } 
    }
}