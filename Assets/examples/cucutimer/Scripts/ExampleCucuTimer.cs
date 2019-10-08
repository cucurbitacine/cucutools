using cucu.tools;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace cucu.example
{
    public class ExampleCucuTimer : MonoBehaviour
    {
        [SerializeField] List<TimerUnit> _timers;

        private void Awake()
        {
            _timers = new List<TimerUnit>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                var timerUnit = new TimerUnit();

                var messageStart = $"{_timers.Count} - Старт";
                var messageTick = $"{_timers.Count} - Тик";
                var messageStop = $"{_timers.Count} - Стоп";
                var messageForceStop = $"{_timers.Count} - Ручной стоп";

                var onStart = new Action(() => { Debug.Log(messageStart); });

                var onTick = new Action(() => { Debug.Log(messageTick); });

                var onStop = new Action(() => { Debug.Log(messageStop); });

                var timer = CucuTimerFactory.CreateTimer();

                var onForceStop = new Action(() =>
                {
                    Debug.Log(messageForceStop);
                    timer.OnStart.RemoveListener(onStart);
                    timer.OnTick.RemoveListener(onTick);
                });
                
                timer.OnStart.AddListener(onStart);
                timer.OnTick.AddListener(onTick);
                timer.OnStop.AddListener(onStop);
                timer.OnForceStop.AddListener(onForceStop);

                timerUnit.timer = timer;

                _timers.Add(timerUnit);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                foreach (var timerUnit in _timers)
                {
                    timerUnit.timer.StartTimer(timerUnit.duration, timerUnit.tick, timerUnit.delay);
                }
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
            {
                foreach (var timerUnit in _timers)
                {
                    timerUnit.timer.StopTimer();
                }
            }
        }
    }

    [Serializable]
    public class TimerUnit
    {
        public CucuTimer timer;
        
        [Range(0f, 60f)] public float duration;
        [Range(0f, 30f)] public float tick;
        [Range(0f, 15f)] public float delay;
    }
}