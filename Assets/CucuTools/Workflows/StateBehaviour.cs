using System.Linq;
using CucuTools.Buttons;
using CucuTools.Workflows.Core;
using UnityEngine;

namespace CucuTools.Workflows
{
    [AddComponentMenu(Cucu.AddComponent + Cucu.WorkflowGroup + ObjectName, 1)]
    public sealed class StateBehaviour : StateEntity
    {
        #region SerializeField

        [Header("Info")]
        [SerializeField] private bool isPlaying;
        
        [Header("Transitions")]
        [SerializeField] private TransitionEntity[] transitions;

        #endregion
        
        public const string ObjectName = "State";
        
        public override bool IsPlaying => isPlaying;

        public override bool IsLast => (Transitions?.Length ?? 0) == 0;

        public override TransitionEntity[] Transitions
        {
            get => transitions;
            set => transitions = value;
        }

        private IStateTrigger[] _triggers;

        #region Public API

        public override bool TryGetNextState(out StateEntity nextState)
        {
            nextState = null;
            return IsPlaying && (nextState = Transitions?.FirstOrDefault(t => t.IsReady)?.Target) != null;
        }

        public override void StartState()
        {
            if (IsPlaying) return;
            
            isPlaying = true;

            var cond = _triggers.OfType<ConditionEntity>();

            foreach (var entity in cond)
            {
                entity.Invoke(StateInvoke.OnStart);
            }

            foreach (var trigger in _triggers.Where(t => !cond.Contains(t)))
            {
                trigger.Invoke(StateInvoke.OnStart);
            }
        }

        public override void StopState()
        {
            if (!IsPlaying) return;
            
            isPlaying = false;
            
            var cond = _triggers.OfType<ConditionEntity>();

            foreach (var entity in cond)
            {
                entity.Invoke(StateInvoke.OnStop);
            }

            foreach (var trigger in _triggers.Where(t => !cond.Contains(t)))
            {
                trigger.Invoke(StateInvoke.OnStop);
            }
        }

        #endregion

        #region Private API

        private void Setup()
        {
            isPlaying = false;
            
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
            _triggers = GetComponentsInChildren<IStateTrigger>()
                .Where(t => t.Owner == this)
                .ToArray();
        }

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            Setup();
        }

        private void OnValidate()
        {
            if (!name.StartsWith("*")) name = $"* {name}";
            else if (!name.StartsWith("* ")) name = $"* {name.Substring(1)}";
        }

        #endregion
        
        #region Editor

        [CucuButton("Create Transition")]
        private void CreateTransition()
        {
            new GameObject("").AddComponent<TransitionBehaviour>().transform.SetParent(transform);
        }
        
        [CucuButton("Add Trigger")]
        private void AddTrigger()
        {
            gameObject.AddComponent<StateTrigger>();
        }

        #endregion
    }
}