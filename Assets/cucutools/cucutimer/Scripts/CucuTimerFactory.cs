using System;
using System.Collections.Generic;
using UnityEngine;

namespace cucu.tools
{
    /// <summary>
    /// Timer factory and holder
    /// </summary>
    [DisallowMultipleComponent]
    public class CucuTimerFactory : MonoBehaviour
    {
        private static CucuTimerFactory _instance { get; set; }

        private Dictionary<Guid, CucuTimer> _timers;

        static CucuTimerFactory()
        {
            DontDestroyOnLoad(new GameObject(nameof(CucuTimerFactory), new[] { typeof(CucuTimerFactory) }));
        }

        private void Awake()
        {
            if (_instance != null) Destroy(this);
            else _instance = this;

            _timers = new Dictionary<Guid, CucuTimer>();
        }

        #region Public static

        public static CucuTimer CreateTimer()
        {
            return _instance.AddTimer();
        }

        public static bool DestroyTimer(CucuTimer timer)
        {
            return _instance.RemoveTimer(timer);
        }

        public static bool DestroyTimer(Guid guid)
        {
            return _instance.RemoveTimer(guid);
        }

        #endregion

        #region Private

        private CucuTimer AddTimer()
        {
            var timer = gameObject.AddComponent<CucuTimer>();
            _timers.Add(timer.Guid, timer);
            return timer;
        }

        private bool RemoveTimer(CucuTimer timer)
        {
            return RemoveTimer(timer.Guid);
        }

        private bool RemoveTimer(Guid guid)
        {
            return _timers.Remove(guid);
        }

        #endregion
    }

    /// <summary>
    /// Timer
    /// </summary>
    public class CucuTimer : MonoBehaviour
    {
        [HideInInspector] public CucuEvent OnStart { get; private set; }
        [HideInInspector] public CucuEvent OnTick { get; private set; }
        [HideInInspector] public CucuEvent OnStop { get; private set; }
        [HideInInspector] public CucuEvent OnForceStop { get; private set; }

        public Guid Guid => _guidTimer;

        public float Time => _time;
        public float Delay => _delay;
        public float Tick => _tick;
        public float Duration => _duration;

        public bool IsIdle => _stateCurrent is CucuTimerStateIdle;
        public bool IsСountdown => _stateCurrent is CucuTimerStateСountdown;
        public bool IsTicking => _stateCurrent is CucuTimerStateTicking;

        private CucuTimerStateBase _stateCurrent;

        private Guid _guidTimer;

        private float _time;
        private float _delay;
        private float _tick;
        private float _duration;

        private void Awake()
        {
            _guidTimer = Guid.NewGuid();

            OnStart = new CucuEvent();
            OnTick = new CucuEvent();
            OnStop = new CucuEvent();
            OnForceStop = new CucuEvent();

            _stateCurrent = new CucuTimerStateIdle(this);
        }

        private void Update()
        {
            _stateCurrent.Update();
        }

        public CucuTimer SetValues(float duration, float tick = 0.0f, float delay = 0.0f)
        {
            return SetDuration(duration).SetTick(tick).SetDelay(delay);
        }

        public void StartTimer(float duration, float tick = 0.0f, float delay = 0.0f)
        {
            SetValues(duration, tick, delay)
                .StartTimer();
        }

        public void StartTimer()
        {
            _stateCurrent.Start();
        }

        public void StopTimer()
        {
            _stateCurrent.Stop();
        }

        public CucuTimer SetDelay(float delay)
        {
            if(!IsСountdown) _delay = delay;
            return this;
        }

        public CucuTimer SetTick(float tick)
        {
            if(!IsTicking) _tick = tick;
            return this;
        }

        public CucuTimer SetDuration(float duration)
        {
            if (!IsTicking) _duration = duration;
            return this;
        }

        #region States

        /// <summary>
        /// Base abstract state
        /// </summary>
        private abstract class CucuTimerStateBase
        {
            /// <summary>
            /// Personal timer
            /// </summary>
            protected CucuTimer _timer;

            /// <summary>
            /// Base constructor
            /// </summary>
            /// <param name="timer">Timer</param>
            public CucuTimerStateBase(CucuTimer timer)
            {
                _timer = timer;
            }

            /// <summary>
            /// Start timer
            /// </summary>
            public virtual void Start() { }

            /// <summary>
            /// Stop timer
            /// </summary>
            public virtual void Stop()
            {
                _timer._stateCurrent = new CucuTimerStateIdle(_timer);
                _timer.OnForceStop.Invoke();
            }

            /// <summary>
            /// Actions execute on MonoBehaviour-Update
            /// </summary>
            public virtual void Update() { }
        }

        /// <summary>
        /// State of idle
        /// </summary>
        private class CucuTimerStateIdle : CucuTimerStateBase
        {
            /// <inheritdoc />
            public CucuTimerStateIdle(CucuTimer timer) : base(timer) { }

            /// <inheritdoc />
            public override void Start()
            {
                _timer._time = 0.0f;
                _timer._stateCurrent = new CucuTimerStateСountdown(_timer);
            }

            /// <inheritdoc />
            public override void Stop() { }
        }

        /// <summary>
        /// State of countdown before start timer
        /// </summary>
        private class CucuTimerStateСountdown : CucuTimerStateBase
        {
            /// <inheritdoc />
            public CucuTimerStateСountdown(CucuTimer timer) : base(timer)
            {
                _timer._time = _timer._delay;
            }

            public override void Update()
            {
                if (_timer._time > 0.0f)
                {
                    _timer._time -= UnityEngine.Time.deltaTime;
                }
                else
                {
                    _timer._time = 0.0f;
                    _timer._stateCurrent = new CucuTimerStateTicking(_timer);
                }
            }
        }

        /// <summary>
        /// State of ticking
        /// </summary>
        private class CucuTimerStateTicking : CucuTimerStateBase
        {
            /// <summary>
            /// Ticking mode
            /// </summary>
            private readonly bool _isTick;

            /// <summary>
            /// Time of tick
            /// </summary>
            private float _timeTick;

            /// <inheritdoc />
            public CucuTimerStateTicking(CucuTimer timer) : base(timer)
            {
                _isTick = _timer._tick > 0.0f;
                _timer.OnStart.Invoke();
            }

            /// <inheritdoc />
            public override void Update()
            {
                if (_timer._time < _timer._duration)
                {
                    var deltaTime = UnityEngine.Time.deltaTime;
                    if (_isTick)
                    {
                        if (_timeTick < _timer._tick)
                        {
                            _timeTick += deltaTime;
                        }
                        else
                        {
                            _timeTick = 0.0f;
                            _timer.OnTick.Invoke();
                        }
                    }
                    _timer._time += deltaTime;
                }
                else
                {
                    _timer._time = _timer._duration;
                    _timer._stateCurrent = new CucuTimerStateIdle(_timer);
                    _timer.OnStop.Invoke();
                }
            }
        }

        #endregion
    }
}