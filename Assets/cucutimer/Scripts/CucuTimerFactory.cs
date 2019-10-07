using System;
using System.Collections.Generic;
using UnityEngine;

namespace cucu.tools
{
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
            return _instance.p_CreateTimer();
        }

        #endregion

        #region Private

        private CucuTimer p_CreateTimer()
        {
            var timer = gameObject.AddComponent<CucuTimer>();
            _timers.Add(timer.Guid, timer);
            return timer;
        }

        #endregion
    }

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

        private CucuTimerStateBase _stateCurrent;

        private Guid _guidTimer;

        [SerializeField] private float _time;
        [SerializeField] private float _delay;
        [SerializeField] private float _tick;
        [SerializeField] private float _duration;

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

        public void Initial(float duration, float tick = 0.0f, float delay = 0.0f)
        {
            _stateCurrent.Initial(new CucuTimerArgs { duration = duration, tick = tick, delay = delay });
        }

        public void StartTimer(float duration, float tick = 0.0f, float delay = 0.0f)
        {
            Initial(duration, tick, delay);
            _stateCurrent.Start();
        }

        public void StartTimer()
        {
            _stateCurrent.Start();
        }

        public void StopTimer()
        {
            _stateCurrent.Stop();
        }

        #region States

        private abstract class CucuTimerStateBase
        {
            protected CucuTimer _timer;
            protected Guid _guid;

            public CucuTimerStateBase(CucuTimer timer)
            {
                _timer = timer;
            }

            public virtual void Start() { }
            public virtual void Stop()
            {
                _timer._stateCurrent = new CucuTimerStateIdle(_timer);
                _timer.OnForceStop.Invoke();
            }

            public virtual void Update() { }

            public virtual void Initial(CucuTimerArgs args) { }
        }

        private class CucuTimerStateIdle : CucuTimerStateBase
        {
            private bool _isInitial;

            public CucuTimerStateIdle(CucuTimer timer) : base(timer) { }

            public override void Start()
            {
                if (!_isInitial) throw new Exception("Timer is not initialized");
                _timer._time = 0.0f;
                _timer._stateCurrent = new CucuTimerStateСountdown(_timer);
            }

            public override void Stop() { }

            public override void Initial(CucuTimerArgs args)
            {
                _isInitial = CucuTimerArgs.IsValid(args, out var message);
                if (!_isInitial) throw new Exception(message);
                _timer._delay = args.delay;
                _timer._tick = args.tick;
                _timer._duration = args.duration;
            }
        }

        private class CucuTimerStateСountdown : CucuTimerStateBase
        {
            public CucuTimerStateСountdown(CucuTimer timer) : base(timer) { }

            public override void Update()
            {
                if (_timer._time < _timer._delay)
                {
                    _timer._time += UnityEngine.Time.deltaTime;
                }
                else
                {
                    _timer._time = 0.0f;
                    _timer._stateCurrent = new CucuTimerStateTicking(_timer);
                }
            }
        }

        private class CucuTimerStateTicking : CucuTimerStateBase
        {
            private float _time;
            private bool _isTick;

            public CucuTimerStateTicking(CucuTimer timer) : base(timer)
            {
                _isTick = _timer._delay > 0.0f;
                _timer.OnStart.Invoke();
            }

            public override void Update()
            {
                if (_timer._time < _timer._duration)
                {
                    var deltaTime = UnityEngine.Time.deltaTime;
                    if (_isTick)
                    {
                        if (_time < _timer._delay)
                        {
                            _time += deltaTime;
                        }
                        else
                        {
                            _time = 0.0f;
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

        private struct CucuTimerArgs
        {
            public float delay;
            public float tick;
            public float duration;

            public static bool IsValid(CucuTimerArgs args, out string message)
            {
                if (args.delay < 0)
                {
                    message = $"Delay is less then zero!";
                    return false;
                }

                if (args.tick < 0)
                {
                    message = $"Tick is less then zero!";
                    return false;
                }

                if (args.duration < 0)
                {
                    message = $"Duration is less then zero!";
                    return false;
                }

                message = $"OK";
                return true;
            }
        }

        #endregion
    }
}