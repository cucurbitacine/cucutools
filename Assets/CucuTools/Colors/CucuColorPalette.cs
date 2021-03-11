using System;
using System.Collections.Generic;
using System.Linq;
using CucuTools.Math;
using UnityEngine;

namespace CucuTools.Colors
{
    [Serializable]
    public class CucuColorPalette
    {
        public string Name => _name;
        public Color[] Colors => _colors.ToArray();

        [SerializeField] private string _name;
        [SerializeField] private Color[] _colors;

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

        public Gradient GetGradient()
        {
            return Colors.ToGradient();
        }
    }

    public static class CucuColorPaletteExtentions
    {
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
    }
}