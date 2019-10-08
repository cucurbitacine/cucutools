using System;
using UnityEngine;
using UnityEngine.Events;

namespace cucu.tools
{
    /// <inheritdoc />
    public class CucuEvent : CucuEventBase
    {
    }

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
    public class CucuBoolEvent : CucuEventBase<bool>
    {
    }

    /// <inheritdoc />
    public class CucuIntEvent : CucuEventBase<int>
    {
    }

    /// <inheritdoc />
    public class CucuFloatEvent : CucuEventBase<float>
    {
    }

    /// <inheritdoc />
    public class CucuStringEvent : CucuEventBase<string>
    {
    }

    /// <inheritdoc />
    public class CucuVector2Event : CucuEventBase<Vector2>
    {
    }

    /// <inheritdoc />
    public class CucuVector3Event : CucuEventBase<Vector3>
    {
    }

    /// <inheritdoc />
    public class CucuQuaternionEvent : CucuEventBase<Quaternion>
    {
    }

    /// <inheritdoc />
    public class CucuObjectEvent : CucuEventBase<object>
    {
    }

    public interface ICucuEvent<TAction>
    {
        void AddListener(TAction action);
        void RemoveListener(TAction action);
    }
}