using System;
using UnityEngine;

namespace CucuTools
{
    /// <summary>
    /// Timer
    /// </summary>
    [Serializable]
    public class CucuTimer
    {
        public string Key => _key;

        public Guid Guid => _guid;

        public static float TimeGlobal => CucuTimerManager.GetTime();
        public float TimeLocal => CucuTimerManager.GetTime(Guid);
        public float Tick => _tick;
        public float Duration => _duration;
        
        private Guid _guid;
        
        [HideInInspector] [SerializeField] private float _tick;
        [HideInInspector] [SerializeField] private float _duration;
        
        [Header("Key")]
        [SerializeField] private string _key;
        
        public CucuTimer(string key = null)
        {
            _guid = Guid.NewGuid();
            SetKey(key ?? _guid.ToString());
            
            CucuTimerManager.RegisterTimer(this);
        }

        public CucuTimer SetKey(string key)
        {
            _key = key;
            return this;
        }

        public CucuTimer SetTick(float tick)
        {
            _tick = tick < 0f ? 0f : tick;
            return this;
        }

        public CucuTimer SetTick(int count)
        {
            count = count < 0 ? 0 : count;
            var tick = Duration / (count + 1);
            return SetTick(tick);
        }

        public CucuTimer SetDuration(float duration)
        {
            _duration = duration < 0f ? 0f : duration;
            return this;
        }

        public CucuTimer OnStart(Action action)
        {
            CucuTimerManager.AddListenerOnStart(Guid, action);

            return this;
        }

        public CucuTimer OnTick(Action action)
        {
            CucuTimerManager.AddListenerOnTick(Guid, action);

            return this;
        }

        public CucuTimer OnStop(Action action)
        {
            CucuTimerManager.AddListenerOnStop(Guid, action);

            return this;
        }

        public CucuTimer OnForceStop(Action action)
        {
            CucuTimerManager.AddListenerOnForceStop(Guid, action);

            return this;
        }

        public CucuTimer StartTimer()
        {
            CucuTimerManager.StartTimer(Guid);

            return this;
        }

        public CucuTimer StopTimer()
        {
            CucuTimerManager.StopTimer(Guid);

            return this;
        }

        public CucuTimer ForceStopTimer()
        {
            CucuTimerManager.ForceStopTimer(Guid);

            return this;
        }

        public CucuTimer RemoveAllListeners()
        {
            CucuTimerManager.RemoveAllListeners(Guid);
            return this;
        }
    }
}