using UnityEngine;
using UnityEngine.Events;

namespace cucu.tools
{
    /// <inheritdoc />
    public class CucuEvent : UnityEvent
    {
    }

    /// <inheritdoc />
    public class CucuBoolEvent : UnityEvent<bool>
    {
    }

    /// <inheritdoc />
    public class CucuIntEvent : UnityEvent<int>
    {
    }

    /// <inheritdoc />
    public class CucuFloatEvent : UnityEvent<float>
    {
    }

    /// <inheritdoc />
    public class CucuStringEvent : UnityEvent<string>
    {
    }

    /// <inheritdoc />
    public class CucuVector2Event : UnityEvent<Vector2>
    {
    }

    /// <inheritdoc />
    public class CucuVector3Event : UnityEvent<Vector3>
    {
    }

    /// <inheritdoc />
    public class CucuQuaternionEvent : UnityEvent<Quaternion>
    {
    }

    /// <inheritdoc />
    public class CucuObjectEvent : UnityEvent<object>
    {
    }
}