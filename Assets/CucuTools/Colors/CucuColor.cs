using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    public static class CucuColor
    {
        public enum ColorMap
        {
            Rainbow,
            Jet,
            Hot,
            Gray,
            Yarg,
        }

        public static readonly ColorMap[] ColorMaps = (ColorMap[]) Enum.GetValues(typeof(ColorMap));
        
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

        public static string ToHex(this Color color)
        {
            var result = "";
            for (var i = 0; i < 3; i++)
            {
                var val = (int) Mathf.Clamp(color[i] * 255, 0, 255);
                var hex = val.ToString("X");
                if (hex.Length < 2) hex = "0" + hex;
                result += hex;
            }

            return result;
        }

        public static Color ToColor(this string hex)
        {
            if (hex == null || hex.Length != 6)
            {
                Cucu.Log($"Not valid hex value \"{hex}\"", "CucuColor", logType: LogType.Warning);
                return Color.black;
            }

            var hexR = hex.Substring(0, 2);
            var hexG = hex.Substring(2, 2);
            var hexB = hex.Substring(4, 2);

            var intR = 0;
            var intG = 0;
            var intB = 0;

            try
            {
                intR = int.Parse(hexR, NumberStyles.HexNumber);
                intG = int.Parse(hexG, NumberStyles.HexNumber);
                intB = int.Parse(hexB, NumberStyles.HexNumber);
            }
            catch (Exception e)
            {
                Cucu.Log($"Exception of parsing hex color : \"{e.Message}\"", "CucuColor", logType: LogType.Warning);
                return Color.black;
            }

            var r = intR / 255f;
            var g = intG / 255f;
            var b = intB / 255f;

            return new Color(r, g, b, 1f);
        }

        public static CucuColorPalette ToColorPalette(this IEnumerable<Color> colors, string name = "")
        {
            return new CucuColorPalette(name, colors.ToArray());
        }

        public static Gradient ToGradient(this CucuColorPalette pallete, GradientMode mode = GradientMode.Blend)
        {
            var result = new Gradient {mode = mode};

            var times = CucuMath.LinSpace(8);
            var colors = times.ToDictionary(t => t, pallete.Get);

            result.colorKeys = times.Select(t => new GradientColorKey(colors[t], t)).ToArray();
            result.alphaKeys = times.Select(t => new GradientAlphaKey(colors[t].a, t)).ToArray();

            return result;
        }

        public static Gradient ToGradient(this IEnumerable<Color> colors,
            string name = "", GradientMode mode = GradientMode.Blend)
        {
            return colors.ToColorPalette(name).ToGradient(mode);
        }

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
                    "CC00FF".ToColor(),
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
        
        public static readonly Dictionary<CucuColor.ColorMap, CucuColorPalette> PaletteMaps =
            new Dictionary<CucuColor.ColorMap, CucuColorPalette>()
            {
                {CucuColor.ColorMap.Rainbow, CucuColor.Palettes.Rainbow},
                {CucuColor.ColorMap.Jet, CucuColor.Palettes.Jet},
                {CucuColor.ColorMap.Hot, CucuColor.Palettes.Hot},
                {CucuColor.ColorMap.Gray, CucuColor.Palettes.Gray},
                {CucuColor.ColorMap.Yarg, CucuColor.Palettes.Yarg},
            };
    }

    public class CucuColorPalette : IColorPalette
    {
        public string Name => _name;
        public Color[] Colors => _colors.ToArray();

        private Color[] _colors;
        private string _name;

        private readonly Dictionary<float, Color> _bufferedColors = new Dictionary<float, Color>();

        public CucuColorPalette(string name, params Color[] colors)
        {
            _name = name;
            _colors = colors;
        }

        public CucuColorPalette(params Color[] colors) : this("", colors)
        {
        }

        public Color Get(float value)
        {
            value = Mathf.Clamp01(value);

            if (_bufferedColors.TryGetValue(value, out var color))
                return color;

            color = _colors.LerpColor(value);
            _bufferedColors.Add(value, color);

            return color;
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
            return $"<color=#{color.ToHex()}>{obj}</color>";
        }

        public static string ToColoredString(this object obj, string hex)
        {
            return $"<color=#{hex}>{obj}</color>";
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