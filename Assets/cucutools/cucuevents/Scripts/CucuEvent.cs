using System;
using UnityEngine;
using UnityEngine.Events;

namespace cucu.tools
{
    /// <inheritdoc />
    [Serializable]
    public class CucuEvent : CucuEventBase
    {
    }

    [Serializable]
    public abstract class CucuEventBase : UnityEvent, ICucuEvent<Action>
    {
        public void AddListener(Action action)
        {
            base.AddListener(action.Target, action.Method);
        }

        public void RemoveListener(Action action)
        {
            base.RemoveListener(action.Target, action.Method);
        }
    }

    [Serializable]
    public abstract class CucuEventBase<T> : UnityEvent<T>, ICucuEvent<Action<T>>
    {
        public void AddListener(Action<T> action)
        {
            base.AddListener(action.Target, action.Method);
        }

        public void RemoveListener(Action<T> action)
        {
            base.RemoveListener(action.Target, action.Method);
        }
    }

    /// <inheritdoc />
    [Serializable]
    public class CucuBoolEvent : CucuEventBase<bool>
    {
    }

    /// <inheritdoc />
    [Serializable]
    public class CucuIntEvent : CucuEventBase<int>
    {
    }

    /// <inheritdoc />
    [Serializable]
    public class CucuFloatEvent : CucuEventBase<float>
    {
    }

    /// <inheritdoc />
    [Serializable]
    public class CucuStringEvent : CucuEventBase<string>
    {
    }

    /// <inheritdoc />
    [Serializable]
    public class CucuVector2Event : CucuEventBase<Vector2>
    {
    }

    /// <inheritdoc />
    [Serializable]
    public class CucuVector3Event : CucuEventBase<Vector3>
    {
    }

    /// <inheritdoc />
    [Serializable]
    public class CucuQuaternionEvent : CucuEventBase<Quaternion>
    {
    }

    /// <inheritdoc />
    [Serializable]
    public class CucuObjectEvent : CucuEventBase<object>
    {
    }

    public interface ICucuEvent<TAction>
    {
        void AddListener(TAction action);
        void RemoveListener(TAction action);
    }
}