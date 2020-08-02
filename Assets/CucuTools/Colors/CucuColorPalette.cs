using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools
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

        public static readonly CucuColorPalette BlackToWhite = new CucuColorPalette("BlackToWhite", Color.black, Color.white);

        public static readonly CucuColorPalette WhiteToBlack = new CucuColorPalette("WhiteToBlack", Color.white, Color.black);

        #endregion
    }

    public static class CucuColorPaletteExt
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