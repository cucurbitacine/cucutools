using System.Linq;
using CucuTools.Buttons;
using CucuTools.Workflows.Core;
using UnityEngine;

namespace CucuTools.Workflows
{
    [AddComponentMenu(Cucu.AddComponent + Cucu.WorkflowGroup + ObjectName, 0)]
    public sealed class WorkflowBehaviour : WorkflowEntity
    {
        #region SerializeField

        [Header("Info")]
        [SerializeField] private bool isPlaying;
        [SerializeField] private bool paused;
        [SerializeField] private StateEntity current;
        
        [Header("First")]
        [SerializeField] private StateEntity first;
        
        [Header("Transitions")]
        [SerializeField] private TransitionEntity[] transitions;

        [Header("Editor")]
        [SerializeField] private bool debugLog;

        #endregion
        
        public const string ObjectName = "Workflow";

        public override bool IsPlaying => isPlaying;

        public override bool IsLast => Current.IsLast && ((Transitions?.Length ?? 0) == 0);
        
        public override StateEntity Current => current;

        public override bool Paused
        {
            get => paused;
            set => paused = value;
        }
        
        public override TransitionEntity[] Transitions
        {
            get => transitions;
            set => transitions = value;
        }

        public StateEntity First => first;
        
        private IStateTrigger[] _triggers;

        #region Public API

        public void SetCurrent(StateEntity state)
        {
            if (debugLog) Debug.Log($"{(current != null ? current.name : "")} -> {(state.name)}");
            
            current = state;
        }
        
        public override bool TryGetNextState(out StateEntity nextState)
        {
            nextState = null;
            return IsPlaying && Current.IsLast && ((nextState = Transitions?.FirstOrDefault(t => t.IsReady)?.Target) != null);
        }

        public override void StartState()
        {
            if (IsPlaying) return;

            SetCurrent(First);
            
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

            Current.StartState();
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
            
            Current.StopState();
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
        
        private void UpdateStateMachine()
        {
            if (Paused) return;
            
            if (Current.IsLast)
            {
                return;
            }

            if (Current.TryGetNextState(out var nextState))
            {
                Current.StopState();
                SetCurrent(nextState);
                Current.StartState();
            }
        }

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            Setup();
        }
        
        private void Update()
        {
            if (IsPlaying) UpdateStateMachine();
        }

        private void OnValidate()
        {
            if (!name.StartsWith("#")) name = $"# {name}";
            else if (!name.StartsWith("# ")) name = $"# {name.Substring(1)}";
        }

        #endregion
        
        #region Editor

        [CucuButton("Create State")]
        public void CreateState()
        {
            new GameObject("State").AddComponent<StateBehaviour>().transform.SetParent(transform);
        }
        
        [CucuButton("Create Transition")]
        private void CreateTransition()
        {
            new GameObject("").AddComponent<TransitionBehaviour>().transform.SetParent(transform);
        }

        #endregion
    }
}