using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    /// <summary>
    /// Timer factory and holder
    /// </summary>
    [DisallowMultipleComponent]
    public class CucuTimerFactory : MonoBehaviour
    {
        private static CucuTimerFactory _instance { get; set; }

        private readonly Dictionary<Guid, CucuInfoTimer> _infoTimers = new Dictionary<Guid, CucuInfoTimer>();

        private readonly List<CucuInfoTimer> _activeTimers = new List<CucuInfoTimer>();
        private List<CucuInfoTimer> _internalActiveTimers;

        private float _time;

        static CucuTimerFactory()
        {
            DontDestroyOnLoad(new GameObject(nameof(CucuTimerFactory), new[] {typeof(CucuTimerFactory)}));
        }

        private void Awake()
        {
            if (_instance != null) Destroy(this);
            else _instance = this;
        }

        private void Update()
        {
            UpdateTime();

            UpdateTimers();
        }

        private void OnDestroy()
        {
            RemoveAllListeners();
        }

        #region Public static

        public static CucuTimer Create(string key)
        {
            return _instance.InternalCreate(key);
        }

        public static CucuTimer Create()
        {
            return _instance.InternalCreate();
        }

        public static float GetTime()
        {
            return _instance.InternalGetTime();
        }

        public static float GetTime(Guid guid)
        {
            return _instance.InternalGetTime(guid);
        }

        public static float GetTime(CucuTimer timer)
        {
            return GetTime(timer.Guid);
        }

        public static bool AddListenerOnStart(Guid guid, Action action)
        {
            return _instance.InternalAddListenerOnStart(guid, action);
        }

        public static bool AddListenerOnTick(Guid guid, Action action)
        {
            return _instance.InternalAddListenerOnTick(guid, action);
        }

        public static bool AddListenerOnStop(Guid guid, Action action)
        {
            return _instance.InternalAddListenerOnStop(guid, action);
        }

        public static bool AddListenerOnForceStop(Guid guid, Action action)
        {
            return _instance.InternalAddListenerOnForceStop(guid, action);
        }

        public static void StartTimer(Guid guid)
        {
            _instance.InternalStartTimer(guid);
        }

        public static void StopTimer(Guid guid)
        {
            _instance.InternalStopTimer(guid);
        }

        public static void ForceStopTimer(Guid guid)
        {
            _instance.InternalForceStopTimer(guid);
        }

        public static bool RemoveTimer(Guid guid)
        {
            return _instance.InternalRemoveTimer(guid);
        }

        public static bool RemoveTimer(CucuTimer timer)
        {
            return RemoveTimer(timer.Guid);
        }

        public static void DestroyAfterStop(Guid guid, bool value)
        {
            _instance.InternalDestroyAfterStop(guid, value);
        }

        public static void RemoveAllListeners(Guid guid)
        {
            _instance.InternalRemoveAllListeners(guid);
        }

        public static void RemoveAllListeners()
        {
            _instance.InternalRemoveAllListeners();
        }

        #endregion

        #region Private instance

        private CucuTimer InternalCreate(string key = null)
        {
            var timer = gameObject.AddComponent<CucuTimer>();

            var infoTimer = new CucuInfoTimer(timer);

            _infoTimers.Add(infoTimer.Guid, infoTimer);

            return timer.SetKey(key ?? timer.Guid.ToString());
        }

        private float InternalGetTime()
        {
            return _time;
        }

        private float InternalGetTime(Guid guid)
        {
            var time = 0.0f;

            if (TryGetTimer(guid, out var infoTimer) && infoTimer.Play)
            {
                time = _time - infoTimer.Start;
            }

            return time;
        }

        private bool InternalAddListenerOnStart(Guid guid, Action action)
        {
            if (!TryGetTimer(guid, out var timer)) return false;
            timer.OnStartEvent.AddListener(action);
            return true;
        }

        private bool InternalAddListenerOnTick(Guid guid, Action action)
        {
            if (!TryGetTimer(guid, out var timer)) return false;
            timer.OnTickEvent.AddListener(action);
            return true;
        }

        private bool InternalAddListenerOnStop(Guid guid, Action action)
        {
            if (!TryGetTimer(guid, out var timer)) return false;
            timer.OnStopEvent.AddListener(action);
            return true;
        }

        private bool InternalAddListenerOnForceStop(Guid guid, Action action)
        {
            if (!TryGetTimer(guid, out var timer)) return false;
            timer.OnForceStopEvent.AddListener(action);
            return true;
        }

        private void InternalStartTimer(Guid guid)
        {
            if (!TryGetTimer(guid, out var timer) || timer.Play) return;
            timer.Start = _time;
            timer.LastTick = _time;
            timer.Play = true;
            _activeTimers.Add(timer);
            timer.OnStartEvent.Invoke();
        }

        private void InternalStopTimer(Guid guid)
        {
            if (!TryGetTimer(guid, out var timer) || !timer.Play) return;
            _activeTimers.Remove(timer);
            timer.Play = false;
            timer.OnStopEvent.Invoke();
        }

        private void InternalForceStopTimer(Guid guid)
        {
            if (!TryGetTimer(guid, out var timer) || !timer.Play) return;
            _activeTimers.Remove(timer);
            timer.Play = false;
            timer.OnForceStopEvent.Invoke();
        }

        private bool InternalRemoveTimer(Guid guid)
        {
            InternalRemoveAllListeners(guid);
            return _infoTimers.Remove(guid);
        }

        private void InternalDestroyAfterStop(Guid guid, bool value)
        {
            if (TryGetTimer(guid, out var timer)) timer.Destroy = value;
        }

        private void InternalRemoveAllListeners(Guid guid)
        {
            if (!TryGetTimer(guid, out var timer)) return;
            timer.OnStartEvent.RemoveAllListeners();
            timer.OnTickEvent.RemoveAllListeners();
            timer.OnStopEvent.RemoveAllListeners();
        }

        private void InternalRemoveAllListeners()
        {
            foreach (var infoTimer in _infoTimers)
            {
                infoTimer.Value.OnStartEvent.RemoveAllListeners();
                infoTimer.Value.OnTickEvent.RemoveAllListeners();
                infoTimer.Value.OnStopEvent.RemoveAllListeners();
            }
        }

        private void UpdateTime()
        {
            _time += Time.deltaTime;
        }

        private void UpdateTimers()
        {
            _internalActiveTimers = _activeTimers.ToList();

            foreach (var infoTimer in _internalActiveTimers)
            {
                if (infoTimer.Timer == null)
                {
                    _activeTimers.Remove(infoTimer);
                    RemoveTimer(infoTimer.Guid);
                    continue;
                }

                var time = _time - infoTimer.Start;
                if (time >= infoTimer.Timer.Duration)
                {
                    InternalStopTimer(infoTimer.Timer.Guid);
                    if (infoTimer.Destroy) Destroy(infoTimer.Timer);
                    continue;
                }

                if (infoTimer.Timer.Tick > 0.0f)
                {
                    var timeTick = _time - infoTimer.LastTick;
                    if (timeTick >= infoTimer.Timer.Tick)
                    {
                        infoTimer.LastTick = _time;
                        infoTimer.OnTickEvent.Invoke();
                    }
                }
            }
        }

        private bool TryGetTimer(Guid guid, out CucuInfoTimer cucuTimer)
        {
            return _infoTimers.TryGetValue(guid, out cucuTimer);
        }

        #endregion
    }

    [Serializable]
    public class CucuInfoTimer
    {
        [HideInInspector] public CucuEvent OnStartEvent = new CucuEvent();
        [HideInInspector] public CucuEvent OnTickEvent = new CucuEvent();
        [HideInInspector] public CucuEvent OnStopEvent = new CucuEvent();
        [HideInInspector] public CucuEvent OnForceStopEvent = new CucuEvent();

        public Guid Guid => _guid;
        public CucuTimer Timer => _timer;

        public float Start;
        public float LastTick;
        public bool Play;
        public bool Destroy;

        private Guid _guid;
        public CucuTimer _timer;

        public CucuInfoTimer(CucuTimer timer)
        {
            _timer = timer;
            _guid = _timer.Guid;
        }
    }
}