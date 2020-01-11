using System;
using System.Collections.Generic;
using System.Linq;
using Cucu.Common;
using UnityEngine;

namespace Cucu.Colors
{
    public static class CucuColor
    {
        public static class Palettes
        {
            public static readonly CucuColorPalette Rainbow = new CucuColorPalette("Rainbow",
                new[]
                {
                    Color.red,
                    Color.red.LerpTo(Color.yellow),
                    Color.yellow,
                    Color.green,
                    Color.cyan,
                    Color.blue,
                    Color.magenta
                });

            public static readonly CucuColorPalette Jet = new CucuColorPalette("Jet",
                new[]
                {
                    new Color(0f, 0f, 0.666f, 1f),
                    new Color(0f, 0f, 1f, 1f),
                    new Color(0f, 0.333f, 1f, 1f),
                    new Color(0f, 0.666f, 1f, 1f),
                    new Color(0f, 1f, 1f, 1f),
                    new Color(0.5f, 1f, 0.5f, 1f),
                    new Color(1f, 1f, 0.0f, 1f),
                    new Color(1f, 0.666f, 0.0f, 1f),
                    new Color(1f, 0.333f, 0.0f, 1f),
                    new Color(1f, 0f, 0.0f, 1f),
                    new Color(0.666f, 0f, 0.0f, 1f),
                });

            public static readonly CucuColorPalette Hot = new CucuColorPalette("Hot",
                new[]
                {
                    Color.black,
                    Color.red,
                    Color.yellow,
                    Color.white
                });

            public static readonly CucuColorPalette Gray = new CucuColorPalette("Gray",
                new[]
                {
                    Color.black,
                    Color.white,
                });

            public static readonly CucuColorPalette Yarg = new CucuColorPalette("Yarg",
                new[]
                {
                    Color.white,
                    Color.black,
                });
        }

        public static Color SetIntensity(Color color, float intensity)
        {
            intensity = Mathf.Clamp01(intensity);

            return new Color(color.r * intensity, color.g * intensity, color.b * intensity, color.a);
        }

        public static Color SetAlpha(Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        public static Color Lerp(Color origin, Color target, float value)
        {
            value = Mathf.Clamp01(value);

            return Color.Lerp(origin, target, value);
        }

        public static Color Lerp(float value, params Color[] colors)
        {
            if (colors == null || colors.Length == 0) return Color.white;
            if (colors.Length == 1) return colors.First();

            value = Mathf.Clamp01(value);

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

        public static Color GetColor(string hex)
        {
            return Color.black;
        }
    }

    public class CucuColorPalette : IColorPalette
    {
        public string Name => _name;

        private Color[] _colors;
        private string _name;

        public CucuColorPalette(string name, params Color[] colors)
        {
            _name = name;
            _colors = colors;
        }

        public Color Get(float value)
        {
            return _colors.LerpColor(value);
        }
    }

    public interface IColorPalette
    {
        Color Get(float value);
    }

    public static class CucuColorExt
    {
        public static string ToColoredString(this object obj, Color color)
        {
            return $"<color=#{color.GetHex()}>{obj}</color>";
        }

        public static Color LerpTo(this Color color, Color target, float t = 0.5f)
        {
            return CucuColor.Lerp(color, target, t);
        }

        public static Color LerpColor(this float value, params Color[] colors)
        {
            return CucuColor.Lerp(value, colors);
        }

        public static Color LerpColor(this IEnumerable<Color> colors, float value)
        {
            return value.LerpColor(colors.ToArray());
        }

        public static Color SetColorAlpha(this Color color, float value)
        {
            return CucuColor.SetAlpha(color, value);
        }

        public static Color SetColorIntensity(this Color color, float value)
        {
            return CucuColor.SetIntensity(color, value);
        }
    }
}