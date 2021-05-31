using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools.Colors
{
    [Serializable]
    public class CucuPalette
    {
        /// <summary>
        /// Name of palette
        /// </summary>
        public string Name => _name;
        
        /// <summary>
        /// Colors
        /// </summary>
        public Color[] Colors => _colors.ToArray();

        [SerializeField] private string _name;
        [SerializeField] private Color[] _colors;

        private readonly Dictionary<float, Color> _bufferedColors = new Dictionary<float, Color>();

        public CucuPalette(string name, params Color[] colors)
        {
            _name = name;
            _colors = colors;
        }

        public CucuPalette(params Color[] colors) : this("", colors)
        {
        }

        /// <summary>
        /// Evaluate color by value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Color Evaluate(float value)
        {
            value = Mathf.Clamp01(value);

            if (_bufferedColors.TryGetValue(value, out var color))
                return color;

            color = _colors.LerpColor(value);
            _bufferedColors.Add(value, color);

            return color;
        }

        /// <summary>
        /// Get as <see cref="Gradient"/>
        /// </summary>
        /// <returns></returns>
        public Gradient GetGradient()
        {
            return Colors.ToGradient();
        }
    }
    
    /// <summary>
    /// Palette extentions
    /// </summary>
    public static class CucuColorPaletteExtentions
    {
        /// <summary>
        /// Convert colors to palette
        /// </summary>
        /// <param name="colors"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static CucuPalette ToColorPalette(this IEnumerable<Color> colors, string name = "")
        {
            return new CucuPalette(name, colors.ToArray());
        }

        /// <summary>
        /// Convert palette to gradient
        /// </summary>
        /// <param name="pallete">Color palette</param>
        /// <param name="mode">Gradient mode</param>
        /// <returns>Gradient</returns>
        public static Gradient ToGradient(this CucuPalette pallete, GradientMode mode = GradientMode.Blend)
        {
            var result = new Gradient {mode = mode};

            var times = Cucu.LinSpace(8);
            var colors = times.ToDictionary(t => t, pallete.Evaluate);

            result.colorKeys = times.Select(t => new GradientColorKey(colors[t], t)).ToArray();
            result.alphaKeys = times.Select(t => new GradientAlphaKey(colors[t].a, t)).ToArray();

            return result;
        }

        /// <summary>
        /// Convert colors to gradient
        /// </summary>
        /// <param name="colors">Colors</param>
        /// <param name="name"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static Gradient ToGradient(this IEnumerable<Color> colors,
            string name = "", GradientMode mode = GradientMode.Blend)
        {
            return colors.ToColorPalette(name).ToGradient(mode);
        }
        
        
    }
}