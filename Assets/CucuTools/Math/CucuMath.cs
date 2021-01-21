using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    public static class CucuMath
    {
        #region Simple math

        public static float Sin(this float value)
        {
            return Mathf.Sin(value);
        }

        public static float Cos(this float value)
        {
            return Mathf.Cos(value);
        }
        
        public static float Tan(this float value)
        {
            return Mathf.Tan(value);
        }
        
        public static float Asin(this float value)
        {
            return Mathf.Asin(value);
        }

        public static float Acos(this float value)
        {
            return Mathf.Acos(value);
        }
        
        public static float Atan(this float value)
        {
            return Mathf.Atan(value);
        }

        public static float Clamp01(this float value)
        {
            return Mathf.Clamp01(value);
        }
        
        public static float Abs(this float value)
        {
            return Mathf.Abs(value);
        }

        public static float Round(this float value)
        {
            return Mathf.Round(value);
        }
        
        public static float Sign(this float value)
        {
            return Mathf.Sign(value);
        }
        
        public static float Sqrt(this float value)
        {
            return Mathf.Sqrt(value);
        }
        
        public static float Pow(this float value, float power)
        {
            return Mathf.Pow(value, power);
        }
        
        public static float Sin(this int value)
        {
            return Mathf.Sin(value);
        }

        public static float Cos(this int value)
        {
            return Mathf.Cos(value);
        }
        
        public static float Tan(this int value)
        {
            return Mathf.Tan(value);
        }
        
        public static float Asin(this int value)
        {
            return Mathf.Asin(value);
        }

        public static float Acos(this int value)
        {
            return Mathf.Acos(value);
        }
        
        public static float Atan(this int value)
        {
            return Mathf.Atan(value);
        }

        public static int Abs(this int value)
        {
            return Mathf.Abs(value);
        }

        public static float Sign(this int value)
        {
            return Mathf.Sign(value);
        }
        
        public static float Sqrt(this int value)
        {
            return Mathf.Sqrt(value);
        }
        
        public static float Pow(this int value, float power)
        {
            return Mathf.Pow(value, power);
        }
        
        #endregion

        #region LinSpace

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

        #endregion

        #region GetFunction

        public static Func<float, float> GetFunction(float[] args, float[] values)
        {
            return arg =>
            {
                GetEdges(arg, out var iLeftArg, out var iRightArg, args);

                var t = Mathf.Abs(args[iRightArg] - args[iLeftArg]) > float.Epsilon
                    ? (arg - args[iLeftArg]) / (args[iRightArg] - args[iLeftArg])
                    : 0f;

                var result = values[iLeftArg] + (values[iRightArg] - values[iLeftArg]) * t;

                return result;
            };
        }
        
        public static Func<float, float> GetFunction(params float[] values)
        {
            return GetFunction(LinSpace(values.Length), values);
        }
        
        public static Func<float, float> GetFunction(IEnumerable<float> args, IEnumerable<float> values)
        {
            return GetFunction(args.ToArray(), values.ToArray());
        }

        public static Func<float, float> GetFunction(IEnumerable<float> values)
        {
            return GetFunction(values.ToArray());
        }

        #endregion

        #region Edges

        public static void GetLeftEdge<TArg>(TArg arg, out int iLeft, params TArg[] args)
            where TArg : IComparable
        {
            iLeft  = -1;
            
            if (args == null || args.Length == 0) return;
            
            if (arg.CompareTo(args[0]) < 0)
            {
                iLeft  = -1;
                return;
            }
            
            if (arg.CompareTo(args[args.Length - 1]) > 0)
            {
                iLeft  = args.Length - 1;
                return;
            }

            iLeft = args.Select((t, i) => (t, i)).Last(x => arg.CompareTo(x.t) >= 0f).i;
        }

        public static void GetLeftEdge<TArg>(TArg arg, out int iLeft, IEnumerable<TArg> args)
            where TArg : IComparable
        {
            GetLeftEdge(arg, out iLeft, args.ToArray());
        }
        
        public static void GetRightEdge<TArg>(TArg arg, out int iRight, params TArg[] args)
            where TArg : IComparable
        {
            iRight  = -1;
            
            if (args == null || args.Length == 0) return;

            if (arg.CompareTo(args[0]) < 0) // x O--------o
            {
                iRight  = 0;
                return;
            }
            
            if (arg.CompareTo(args[args.Length - 1]) > 0) // o--------o x
            {
                iRight  = -1;
                return;
            }

            iRight = args.Select((t, i) => (t, i)).First(x => arg.CompareTo(x.t) <= 0f).i;
        }

        public static void GetRightEdge<TArg>(TArg arg, out int iRight, IEnumerable<TArg> args)
            where TArg : IComparable
        {
            GetRightEdge(arg, out iRight, args.ToArray());
        }
        
        public static void GetEdges<TArg>(TArg arg, out int iLeft, out int iRight, params TArg[] args)
            where TArg : IComparable
        {
            iLeft  = -1;
            iRight = -1;

            if (args == null || !args.Any()) return;

            if (arg.CompareTo(args[0]) < 0)
            {
                iLeft  = -1;
                iRight = 0;
                return;
            }

            if (arg.CompareTo(args[args.Length - 1]) > 0)
            {
                iLeft  = args.Length - 1;
                iRight = -1;
                return;
            }

            iLeft  = args.Select((t, i) => (t, i)) .Last(x => arg.CompareTo(x.t) >= 0f).i;
            iRight = args.Select((t, i) => (t, i)).First(x => arg.CompareTo(x.t) <= 0f).i;
        }

        public static void GetEdges<TArg>(TArg arg, out int iLeft, out int iRight, IEnumerable<TArg> args)
            where TArg : IComparable
        {
            GetEdges(arg, out iLeft, out iRight, args.ToArray());
        }
        
        #endregion
    }

    public static class CucuMathExt
    {
        public static Vector3 Sum(this IEnumerable<Vector3> vectors)
        {
            return vectors.Aggregate(Vector3.zero, (current, vector) => current + vector);
        }
        
        public static Vector2 XY(this Vector3 a)
        {
            return new Vector2(a.x, a.y);
        }
        
        public static Vector2 ZY(this Vector3 a)
        {
            return new Vector2(a.z, a.y);
        }
        
        public static Vector2 XZ(this Vector3 a)
        {
            return new Vector2(a.x, a.z);
        }

        public static Vector3 Abs(this Vector3 a)
        {
            return new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
        }
        
        public static Vector3 IgnoreX(this Vector3 vector)
        {
            return new Vector3(0f, vector.y, vector.z);
        }
        
        public static Vector3 IgnoreY(this Vector3 vector)
        {
            return new Vector3(vector.x, 0f, vector.z);
        }
        
        public static Vector3 IgnoreZ(this Vector3 vector)
        {
            return new Vector3(vector.x, vector.y, 0f);
        }
        
        public static float Mean(this Vector3 a)
        {
            return (a.x + a.y + a.z) / 3f;
        }
        
        public static IEnumerable<float> Add(this IEnumerable<float> array, float value)
        {
            return array.Select(a => a + value);
        }
        
        public static IEnumerable<float> Multi(this IEnumerable<float> array, float value)
        {
            return array.Select(a => a * value);
        }
        
        public static IEnumerable<float> Divide(this IEnumerable<float> array, float value)
        {
            if (!(Mathf.Abs(value) <= float.Epsilon)) return array.Multi(1 / value);
            
            Debug.Log("Divide by zero");
            return array;
        }
        
        public static IEnumerable<float> Sub(this IEnumerable<float> array, float value)
        {
            return array.Add(-value);
        }

        public static void GetEdges<TArg>(this IEnumerable<TArg> array, TArg arg, out int iLeft, out int iRight)
            where TArg : IComparable
        {
            CucuMath.GetEdges<TArg>(arg, out iLeft, out iRight, array);
        }
    }
}