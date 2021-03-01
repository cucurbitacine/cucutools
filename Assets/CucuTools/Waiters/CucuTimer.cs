using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools
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
        
        [Header("Timer")]
        [SerializeField] private bool started;
        [Min(0f)]
        [SerializeField] private float delay;

        [SerializeField] private UnityEvent beforeTimer;
        [SerializeField] private UnityEvent afterTimer;
        
        private Task _task;
        
        public CucuTimer(float delay)
        {
            this.delay = delay;

            beforeTimer = new UnityEvent();
            afterTimer = new UnityEvent();
            
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