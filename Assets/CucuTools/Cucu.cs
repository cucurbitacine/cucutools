using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    public static class Cucu
    {
        public const string Root = "CucuTools";
        public const string CreateAsset = Root + "/";
        public const string CreateGameObject = "GameObject/" + CreateAsset;
        public const string AddComponent = CreateAsset;
        public const string Tools = "Tools/" + CreateAsset;
        
        public const string WorkflowGroup = "Workflow/";
        
        public static float[] LinSpace(float origin, float target, int count)
        {
            var isForward = count >= 0;

            count = Mathf.Abs(count);

            var result = new float[count];

            switch (count)
            {
                case 0:
                    return result;
                case 1:
                    result[0] = isForward ? origin : target;
                    return result;
                case 2:
                    result[0] = isForward ? origin : target;
                    result[1] = isForward ? target : origin;
                    return result;
                default:
                {
                    result[0] = isForward ? origin : target;
                    result[count - 1] = isForward ? target : origin;
                    var step = (isForward ? 1 : -1) * (target - origin) / (count - 1);
                    for (var i = 1; i < count - 1; i++) result[i] = result[0] + step * i;
                    return result;
                }
            }
        }

        public static float[] LinSpace(int count)
        {
            return LinSpace(0f, 1f, count);
        }

        public static void IndexesOfBorder(out int left, out int right, float value, params float[] values)
        {
            left = -1;
            right = -1;

            for (int i = 0; i < values.Length; i++)
            {
                if (left < 0 && values[values.Length - 1 - i] <= value) left = values.Length - 1 - i;
                if (right < 0 && value <= values[i]) right = i;

                if (left >= 0 && right >= 0) return;
            }
        }

        public static void IndexesOfBorder<T>(out int left, out int right, float value, params T[] values)
            where T : IComparable<float>
        {
            left = -1;
            right = -1;

            for (int i = 0; i < values.Length; i++)
            {
                if (left < 0 && values[values.Length - 1 - i].CompareTo(value) <= 0) left = values.Length - 1 - i;
                if (right < 0 && values[i].CompareTo(value) > 0) right = i;

                if (left >= 0 && right >= 0) return;
            }
        }

        public static void IndexesOfBorder<T>(out int left, out int right, float value, IEnumerable<T> values)
            where T : IComparable<float>
        {
            IndexesOfBorder<T>(out left, out right, value, values.ToArray());
        }
        
        public static void Sync(this Rigidbody rigidbody, Transform target,  bool syncPosition = true, bool syncRotation = true, float maxVelocity = 500f, float syncWeight = 1f, float? deltaTime = null)
        {
            if (deltaTime == null) deltaTime = Time.fixedDeltaTime;
            
            if (syncPosition)
            {
                var dPos = target.position - rigidbody.transform.position;
                rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, Vector3.ClampMagnitude(dPos / deltaTime.Value, maxVelocity),
                    syncWeight);
            }

            if (syncRotation)
            {
                var from = rigidbody.transform.rotation;
                var to = target.rotation;
                var conj = new Quaternion(-from.x, -from.y, -from.z, from.w);
                var dq = new Quaternion((to.x - from.x) * 2.0f, 2.0f * (to.y - from.y), 2.0f * (to.z - from.z),
                    2.0f * (to.w - from.w));
                var c = dq * conj;
                var dRot = new Vector3(c.x, c.y, c.z);

                rigidbody.angularVelocity = Vector3.Lerp(rigidbody.angularVelocity, dRot / deltaTime.Value, syncWeight);
            }
        }
    }
}
