using System;
using System.Linq;
using UnityEngine;

namespace cucu
{
    public static class CucuExtenstions
    {
        public static string SetColor(this object obj, Color color)
        {
            return $"<color=#{color.GetHex()}>{obj}</color>";
        }

        public static string GetHex(this Color color)
        {
            var result = "";
            for (var i = 0; i < 3; i++)
            {
                var hex = Convert.ToString((int) Mathf.Clamp(color[i] * 255, 0, 255), 16);
                if (hex.Length < 2) hex = "0" + hex;
                result += hex;
            }

            return result;
        }

        public static Color LerpTo(this Color color, Color target, float t = 0.5f)
        {
            return Color.Lerp(color, target, Mathf.Clamp01(t));
        }

        public static Color GetColorLerp(this float t, params Color[] colors)
        {
            if (colors == null || colors.Length == 0) return Color.white;
            if (colors.Length == 1) return colors.First();

            var dt = 1f / (colors.Length - 1);
            var begin = 0.0f;
            for (var i = 0; i < colors.Length-1; i++)
            {
                if (begin <= t && t <= begin + dt)
                    return colors[i].LerpTo(colors[i + 1], Mathf.Clamp01((t - begin) / dt));
                begin += dt;
            }

            return colors.Last();
        }
    }
}