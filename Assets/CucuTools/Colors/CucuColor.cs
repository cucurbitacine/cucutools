using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace CucuTools.Colors
{
    public static class CucuColor
    {
        public static Color SetIntensity(Color color, float intensity)
        {
            intensity = Mathf.Clamp01(intensity);

            return new Color(color.r * intensity, color.g * intensity, color.b * intensity, color.a);
        }

        public static Color SetAlpha(Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        #region Lerp

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

        #endregion

        #region Palettes

        public static readonly Dictionary<CucuColorMap, CucuColorPalette> PaletteMaps =
            new Dictionary<CucuColorMap, CucuColorPalette>
            {
                {CucuColorMap.Rainbow, Rainbow},
                {CucuColorMap.Jet, Jet},
                {CucuColorMap.Hot, Hot},
                {CucuColorMap.BlackToWhite, BlackToWhite},
                {CucuColorMap.WhiteToBlack, WhiteToBlack}
            };

        public static readonly CucuColorPalette Rainbow = new CucuColorPalette(
            "Rainbow",
            new[]
            {
                Color.red,
                Color.red.LerpTo(Color.yellow),
                Color.yellow,
                Color.green,
                Color.cyan,
                Color.blue,
                "CC00FF".ToColor()
            });

        public static readonly CucuColorPalette Jet = new CucuColorPalette(
            "Jet",
            new[]
            {
                new Color(0.000f, 0.000f, 0.666f, 1.000f),
                new Color(0.000f, 0.000f, 1.000f, 1.000f),
                new Color(0.000f, 0.333f, 1.000f, 1.000f),
                new Color(0.000f, 0.666f, 1.000f, 1.000f),
                new Color(0.000f, 1.000f, 1.000f, 1.000f),
                new Color(0.500f, 1.000f, 0.500f, 1.000f),
                new Color(1.000f, 1.000f, 0.000f, 1.000f),
                new Color(1.000f, 0.666f, 0.000f, 1.000f),
                new Color(1.000f, 0.333f, 0.000f, 1.000f),
                new Color(1.000f, 0.000f, 0.000f, 1.000f),
                new Color(0.666f, 0.000f, 0.000f, 1.000f)
            });

        public static readonly CucuColorPalette Hot =
            new CucuColorPalette("Hot", Color.black, Color.red, Color.yellow, Color.white);

        public static readonly CucuColorPalette BlackToWhite =
            new CucuColorPalette("BlackToWhite", Color.black, Color.white);

        public static readonly CucuColorPalette WhiteToBlack =
            new CucuColorPalette("WhiteToBlack", Color.white, Color.black);

        #endregion
    }

    public static class CucuColorExtentions
    {
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
                Debug.LogWarning($"Not valid hex value \"{hex}\"");
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
                Debug.LogWarning($"Exception of parsing hex color : \"{e.Message}\"");
                return Color.black;
            }

            var r = intR / 255f;
            var g = intG / 255f;
            var b = intB / 255f;

            return new Color(r, g, b, 1f);
        }

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

        public static Color ToColor(this Vector3 vector3, float alpha = 1f)
        {
            return new Color(vector3.x, vector3.y, vector3.z, alpha);
        }
        
        public static Color ToColor(this Vector4 vector4)
        {
            return new Color(vector4.x, vector4.y, vector4.z, vector4.w);
        }
        
        public static Vector3 ToVector3(this Color color)
        {
            return new Vector3(color.r, color.g, color.b);
        }
        
        public static Vector4 ToVector4(this Color color)
        {
            return new Vector4(color.r, color.g, color.b, color.a);
        } 
    }
}