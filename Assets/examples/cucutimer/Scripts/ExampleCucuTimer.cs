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
                _timers.Add(new TimerUnit() { timer = CucuTimerFactory.CreateTimer()});
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                foreach (var timerUnit in _timers)
                {
                    timerUnit.Initail();
                    timerUnit.timer.StartTimer();
                }
            }
        }
    }

    [Serializable]
    public class TimerUnit
    {
        public float delay;
        public string messageStart;
        public float tick;
        public string messageTick;
        public float duration;
        public string messageStop;
        public CucuTimer timer;

        public void Initail()
        {
            var onStart = new UnityAction(() =>
            {
                Debug.Log(messageStart);
            });

            var onTick = new UnityAction(() =>
            {
                Debug.Log(messageTick);
            });

            var onStop = new UnityAction(() =>
            {
                Debug.Log(messageStop);
            });

            timer.Initial(duration, tick, delay);

            timer.OnStart.AddListener(onStart);
            timer.OnStart.AddListener(onTick);
            timer.OnStart.AddListener(onStop);
        }
    }
}