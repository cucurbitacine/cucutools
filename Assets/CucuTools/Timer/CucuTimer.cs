using System;
using UnityEngine;

namespace CucuTools
{
    /// <summary>
    /// Timer
    /// </summary>
    public class CucuTimer : MonoBehaviour
    {
        public string Key => _key;

        public Guid Guid => _guid;

        public static float TimeGlobal => CucuTimerFactory.GetTime();
        public float TimeLocal => CucuTimerFactory.GetTime(Guid);
        public float Tick => _tick;
        public float Duration => _duration;

        private float _tick;
        private float _duration;

        [Header("Key")] [SerializeField] private string _key;
        private Guid _guid;

        private void Awake()
        {
            _guid = Guid.NewGuid();
        }

        private void OnDestroy()
        {
            CucuTimerFactory.RemoveTimer(this);
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
            CucuTimerFactory.AddListenerOnStart(Guid, action);

            return this;
        }

        public CucuTimer OnTick(Action action)
        {
            CucuTimerFactory.AddListenerOnTick(Guid, action);

            return this;
        }

        public CucuTimer OnStop(Action action)
        {
            CucuTimerFactory.AddListenerOnStop(Guid, action);

            return this;
        }

        public CucuTimer OnForceStop(Action action)
        {
            CucuTimerFactory.AddListenerOnForceStop(Guid, action);

            return this;
        }

        public CucuTimer StartTimer()
        {
            CucuTimerFactory.StartTimer(Guid);

            return this;
        }

        public CucuTimer StopTimer()
        {
            CucuTimerFactory.StopTimer(Guid);

            return this;
        }

        public CucuTimer ForceStopTimer()
        {
            CucuTimerFactory.ForceStopTimer(Guid);

            return this;
        }

        public CucuTimer DestroyAfterStop(bool value = true)
        {
            CucuTimerFactory.DestroyAfterStop(Guid, value);

            return this;
        }

        public CucuTimer DontDestroyAfterStop()
        {
            return DestroyAfterStop(false);
        }

        public CucuTimer RemoveAllListeners()
        {
            CucuTimerFactory.RemoveAllListeners(Guid);
            return this;
        }
    }
}