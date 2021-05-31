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
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public bool UseSmooth
        {
            get => _useSmooth;
            set => _useSmooth = value;
        }

        /// <summary>
        /// Colors
        /// </summary>
        public Color[] Colors => _colors.ToArray();

        [SerializeField] private string _name;
        [SerializeField] private bool _useSmooth;
        [CucuColor]
        [SerializeField] private Color[] _colors;

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
            return _colors.BlendColor(UseSmooth ? Mathf.SmoothStep(0f, 1f, value) : value);
        }

        /// <summary>
        /// Get as <see cref="Gradient"/>
        /// </summary>
        /// <returns></returns>
        public Gradient GetGradient()
        {
            return Colors.ToGradient();
        }
        
                #region Palettes

        /// <summary>
        /// Map of palettes
        /// </summary>
        public static readonly Dictionary<CucuColorMap, CucuPalette> PaletteMaps =
            new Dictionary<CucuColorMap, CucuPalette>
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
        public static readonly CucuPalette Rainbow = new CucuPalette(
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
        public static readonly CucuPalette Jet = new CucuPalette(
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
        public static readonly CucuPalette Hot =
            new CucuPalette("Hot", Color.black, Color.red, Color.yellow, Color.white);

        /// <summary>
        /// Black to white palette
        /// </summary>
        public static readonly CucuPalette BlackToWhite =
            new CucuPalette("BlackToWhite", Color.black, Color.white);

        /// <summary>
        /// White to black palette
        /// </summary>
        public static readonly CucuPalette WhiteToBlack =
            new CucuPalette("WhiteToBlack", Color.white, Color.black);

        #endregion
        
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