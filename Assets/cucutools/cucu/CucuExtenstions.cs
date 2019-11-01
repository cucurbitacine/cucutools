using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace cucu.tools
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

        public static Color GetColorLerp(this float value, params Color[] colors)
        {
            if (colors == null || colors.Length == 0) return Color.white;
            if (colors.Length == 1) return colors.First();

            var x = Mathf.Clamp01(value);
            var dt = 1f / (colors.Length - 1);

            for (var i = 0; i < colors.Length - 1; i++)
            {
                var t = dt * i;
                if (t <= x && x <= t + dt)
                    return colors[i].LerpTo(colors[i + 1], Mathf.Clamp01((x - t) / dt));
            }

            return colors.Last();
        }

        public static CucuTag AddCucuTag(this GameObject gameObject, string tag)
        {
            var cucuTag = gameObject.AddComponent<CucuTag>();
            cucuTag.SetKey(tag);
            return cucuTag;
        }

        public static Color SetAlpha(this Color color, float a)
        {
            return new Color(color.r, color.g, color.b, a);
        }

        public static IEnumerable<CucuTag> GetTagsByArgs(this IEnumerable<CucuTag> tags, IEnumerable<TagArg> args)
        {
            return CucuTag.GetTagsByArgs(args, tags);
        }
    }
}