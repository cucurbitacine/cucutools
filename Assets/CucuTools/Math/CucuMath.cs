using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools.Math
{
    public static class CucuMath
    {
        public static float[] LinSpace(int count, float origin = 0f, float target = 1f)
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

        public static Func<float, float> GetFunction(IEnumerable<float> args, IEnumerable<float> values, bool smooth = false)
        {
            return arg =>
            {
                GetBorderIndex(arg, args, out var iLeftArg, out var iRightArg);

                var argsArray = args.ToArray();
                var leftArg = argsArray[iLeftArg];
                var rightArg = argsArray[iRightArg];

                var valuesArray = values.ToArray();
                var leftValue = valuesArray[iLeftArg];
                var rightValue = valuesArray[iRightArg];

                var t = Mathf.Abs(rightArg - leftArg) > float.Epsilon
                    ? (arg - leftArg) / (rightArg - leftArg)
                    : 0f;

                t = Mathf.Clamp01(smooth ? (Mathf.Sin(-Mathf.PI / 2f + Mathf.PI * t) + 1f) / 2f : t);
                
                var result = leftValue + (rightValue - leftValue) * t;

                return result;
            };
        }

        public static void GetBorderIndex<Targ>(float arg, IEnumerable<Targ> args, out int iLeft, out int iRight)
            where Targ : IComparable
        {
            iLeft = -1;
            iRight = -1;

            if (args == null || !args.Any()) return;

            var argsArray = args.ToArray();

            if (arg.CompareTo(argsArray[0]) < 0)
            {
                iLeft = 0;
                iRight = 0;
                return;
            }

            if (arg.CompareTo(argsArray[argsArray.Length - 1]) > 0)
            {
                iLeft = argsArray.Length - 1;
                iRight = argsArray.Length - 1;
                return;
            }

            iLeft = args.Select((t, i) => (t, i))
                .Last(x => arg.CompareTo(x.t) >= 0f).i;

            iRight = args.Select((t, i) => (t, i))
                .First(x => arg.CompareTo(x.t) <= 0f).i;
        }
    }
}