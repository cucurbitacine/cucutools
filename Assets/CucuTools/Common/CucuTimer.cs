using System;
using System.Threading.Tasks;
using CucuTools.Waiters.Impl;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.Common
{
    [Serializable]
    public class CucuTimer : WaiterCustom
    {
        public float Delay
        {
            get => delay;
            set => delay = Mathf.Max(0f, value);
        }
        
        public bool Started => started;

        private UnityEvent beforeTimer => _beforeTimer ?? (_beforeTimer = new UnityEvent());
        private UnityEvent afterTimer => _afterTimer ?? (_afterTimer = new UnityEvent());
        
        [Header("Timer")]
        [SerializeField] private bool started;
        [Min(0f)]
        [SerializeField] private float delay;

        private UnityEvent _beforeTimer;
        private UnityEvent _afterTimer;
        
        private Task _task;
        
        public CucuTimer(float delay)
        {
            this.delay = delay;

            OnCompleted.AddListener(() =>
            {
                started = false;
                afterTimer.Invoke();
            });
        }

        public CucuTimer(float delay, UnityAction action) : this(delay)
        {
            After(action);
        }
        
        public void Start()
        {
            if (started) return;
            
            beforeTimer.Invoke();
            
            started = true;
            _task = InvokeDelayed(delay);
        }

        public void Start(float delay)
        {
            Delay = delay;
            
            Start();
        }
        
        public CucuTimer Before(params UnityAction[] actions)
        {
            foreach (var action in actions)
                beforeTimer.AddListener(action);    
            
            return this;
        }
        
        public CucuTimer After(params UnityAction[] actions)
        {
            foreach (var action in actions)
                afterTimer.AddListener(action);

            return this;
        }
        
        private async Task InvokeDelayed(float seconds)
        {
            await Task.Delay((int) (seconds * 1000));
            
            Invoke();
        }
    }
}