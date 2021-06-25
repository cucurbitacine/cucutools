using CucuTools.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.Blend
{
    public class CucuTimer : CucuBehaviour
    {
        [CucuReadOnly]
        public bool IsPlaying;
        [CucuReadOnly]
        [Range(0f, 1f)] public float Progress = 0f;
        [CucuReadOnly]
        [Min(0)] public float Timer = 0f;
        [Min(0)] public float Scale = 1f;
        [Min(0)] public float Duration = 1f;

        public UnityEvent<float> onProgressChange;

        [CucuButton()]
        public void StartTimer()
        {
            if (IsPlaying) return;

            IsPlaying = true;
            Timer = 0f;
            Progress = 0f;
            onProgressChange.Invoke(Progress);
        }

        [CucuButton()]
        public void StopTimer()
        {
            if (!IsPlaying) return;

            IsPlaying = false;
            Timer = Duration;
            Progress = 1f;
            onProgressChange.Invoke(Progress);
        }

        private void UpdateTimer(float deltaTime)
        {
            if (Timer >= Duration)
            {
                StopTimer();
                return;
            }

            Timer += deltaTime * Scale;

            Progress = Duration > 0 ? Mathf.Clamp01(Timer / Duration) : 0f;

            onProgressChange.Invoke(Progress);
        }

        private void Update()
        {
            if (IsPlaying) UpdateTimer(Time.deltaTime);
        }
    }
}