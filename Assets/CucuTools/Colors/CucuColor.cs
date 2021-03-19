using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace CucuTools.Colors
{
    /// <summary>
    /// Basic logic for working with color
    /// </summary>
    public static class CucuColor
    {
        /// <summary>
        /// Set intensity. Keep alpha same
        /// </summary>
        /// <param name="color">Color</param>
        /// <param name="intensity">Intensity from 0f to 1f</param>
        /// <returns>Color</returns>
        public static Color SetIntensity(Color color, float intensity)
        {
            intensity = Mathf.Clamp01(intensity);

            return new Color(color.r * intensity, color.g * intensity, color.b * intensity, color.a);
        }

        /// <summary>
        /// Set alpha, Keep RGB same
        /// </summary>
        /// <param name="color">Color</param>
        /// <param name="alpha">Alpha from 0f to 1f</param>
        /// <returns>Color</returns>
        public static Color SetAlpha(Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        #region Lerp

        /// <summary>
        /// Lerping color.
        /// Wrapper around Lerp function of <see cref="Color"/>
        /// </summary>
        /// <param name="origin">From</param>
        /// <param name="target">To</param>
        /// <param name="value">Lerp value</param>
        /// <returns></returns>
        public static Color Lerp(Color origin, Color target, float value)
        {
            value = Mathf.Clamp01(value);

            return Color.Lerp(origin, target, value);
        }

        /// <summary>
        /// Lerping color in queue of colors. Lerping only between two neighbor colors
        /// </summary>
        /// <param name="value">Lerp value</param>
        /// <param name="colors">Colors</param>
        /// <returns>Color</returns>
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

        /// <summary>
        /// Map of palettes
        /// </summary>
        public static readonly Dictionary<CucuColorMap, CucuColorPalette> PaletteMaps =
            new Dictionary<CucuColorMap, CucuColorPalette>
            {
                {CucuColorMap.Rainbow, Rainbow},
                {CucuColorMap.Jet, Jet},
                {CucuColorMap.Hot, Hot},
                {CucuColorMap.BlackToWhite, BlackToWhite},
                {CucuColorMap.WhiteToBlack, WhiteToBlack}
            };

        /// <summary>
        /// Rainbow palette
        /// </summary>
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

        /// <summary>
        /// Jet palette
        /// </summary>
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

        
        /// <summary>
        /// Hot palette
        /// </summary>
        public static readonly CucuColorPalette Hot =
            new CucuColorPalette("Hot", Color.black, Color.red, Color.yellow, Color.white);

        /// <summary>
        /// Black to white palette
        /// </summary>
        public static readonly CucuColorPalette BlackToWhite =
            new CucuColorPalette("BlackToWhite", Color.black, Color.white);

        /// <summary>
        /// White to black palette
        /// </summary>
        public static readonly CucuColorPalette WhiteToBlack =
            new CucuColorPalette("WhiteToBlack", Color.white, Color.black);

        #endregion
    }
    
    /// <summary>
    /// Color extentions
    /// </summary>
    public static class CucuColorExtentions
    {
        /// <summary>
        /// Get string color like FFFFFF
        /// Like 
        /// </summary>
        /// <param name="color"></param>
        /// <returns>Hex string</returns>
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

        /// <summary>
        /// Get color from hex string
        /// </summary>
        /// <param name="hex">String hex color</param>
        /// <returns>Color</returns>
        public static Color ToColor(this string hex)
        {
            if (hex == null || hex.Length != 6)
            {
                Debug.LogWarning($"Not valid hex value \"{hex}\"");
                return Color.black;
            }

            var intR = 0;
            var intG = 0;
            var intB = 0;

            try
            {
                intR = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                intG = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                intB = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Exception of parsing hex color : \"{e.Message}\"");
                return Color.black;
            }

            return new Color(intR / 255f, intG / 255f, intB / 255f, 1f);
        }

        /// <summary>
        /// Get string with html color tag
        /// </summary>
        /// <param name="obj">Message</param>
        /// <param name="color">Color</param>
        /// <returns>Result</returns>
        public static string ToColoredString(this object obj, Color color)
        {
            return $"<color=#{color.ToHex()}>{obj}</color>";
        }

        /// <summary>
        /// Get string with html color tag
        /// </summary>
        /// <param name="obj">Message</param>
        /// <param name="hex">String color hex</param>
        /// <returns>Result</returns>
        public static string ToColoredString(this object obj, string hex)
        {
            return $"<color=#{hex}>{obj}</color>";
        }

        /// <summary>
        /// Lerp color to <param name="target"></param>
        /// </summary>
        /// <param name="color">Origin</param>
        /// <param name="target">Target</param>
        /// <param name="t">Lerp value</param>
        /// <returns>Color</returns>
        public static Color LerpTo(this Color color, Color target, float t = 0.5f)
        {
            return CucuColor.Lerp(color, target, t);
        }

        /// <summary>
        /// Lerping color in queue colors
        /// </summary>
        /// <param name="value">Lerp value</param>
        /// <param name="colors">Colors</param>
        /// <returns>Color</returns>
        public static Color LerpColor(this float value, params Color[] colors)
        {
            return CucuColor.Lerp(value, colors);
        }

        /// <summary>
        /// Lerping color in queue colors
        /// </summary>
        /// <param name="colors">Colors</param>
        /// <param name="value">lerp value</param>
        /// <returns>Color</returns>
        public static Color LerpColor(this IEnumerable<Color> colors, float value)
        {
            return value.LerpColor(colors.ToArray());
        }

        /// <summary>
        /// Set color alpha
        /// </summary>
        /// <param name="color">Color</param>
        /// <param name="value">Alpha</param>
        /// <returns>Color</returns>
        public static Color SetColorAlpha(this Color color, float value)
        {
            return CucuColor.SetAlpha(color, value);
        }

        /// <summary>
        /// Set color intensity
        /// </summary>
        /// <param name="color">Color</param>
        /// <param name="value">Intensity</param>
        /// <returns>Color</returns>
        public static Color SetColorIntensity(this Color color, float value)
        {
            return CucuColor.SetIntensity(color, value);
        }

        /// <summary>
        /// Convert vector3 to color
        /// </summary>
        /// <param name="vector3">Vector3</param>
        /// <param name="alpha">Alpha value</param>
        /// <returns>Color</returns>
        public static Color ToColor(this Vector3 vector3, float alpha = 1f)
        {
            return new Color(vector3.x, vector3.y, vector3.z, alpha);
        }
        
        /// <summary>
        /// Convert vector4 to color
        /// </summary>
        /// <param name="vector4">Vector4</param>
        /// <returns>Color</returns>
        public static Color ToColor(this Vector4 vector4)
        {
            return new Color(vector4.x, vector4.y, vector4.z, vector4.w);
        }
        
        /// <summary>
        /// Convert color to vectro3
        /// </summary>
        /// <param name="color">Color</param>
        /// <returns>Vector3</returns>
        public static Vector3 ToVector3(this Color color)
        {
            return new Vector3(color.r, color.g, color.b);
        }
        
        /// <summary>
        /// Convert color to vectro4
        /// </summary>
        /// <param name="color">Color</param>
        /// <returns>Vector4</returns>
        public static Vector4 ToVector4(this Color color)
        {
            return new Vector4(color.r, color.g, color.b, color.a);
        } 
    }
    
    /// <summary>
    /// Color map list
    /// </summary>
    public enum CucuColorMap
    {
        Rainbow,
        Jet,
        Hot,
        BlackToWhite,
        WhiteToBlack
    }
}