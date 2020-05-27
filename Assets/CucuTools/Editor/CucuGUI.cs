using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor
{
    public static class CucuGUI
    {
        public static GUIStyle GetStyleLabel()
        {
            return new GUIStyle(GUI.skin.label);
        }
        
        public static GUIStyle GetStyleButton()
        {
            return new GUIStyle(GUI.skin.button);
        }
        
        public static Rect[] GetSizedRect(this Rect root, params float[] values)
        {
            return GetRect(root, values);
        }

        public static Rect[] GetSizedRect(this IEnumerable<float> values, Rect root)
        {
            return GetRect(root, values.ToArray());
        }

        public static Rect[] GetRect(Rect root, params float[] values)
        {
            var weights = values.Divide(values.Sum()).ToArray();

            var result = new Rect[weights.Length];
            var shift = 0f;
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = new Rect(root);
                result[i].x += shift;
                result[i].width *= weights[i];
                shift += result[i].width;
            }

            return result;
        }

        private static readonly Dictionary<Color, GUIStyle> styleButtonFromColorMap = new Dictionary<Color, GUIStyle>();

        public static bool Button(string text, Color? color = null, params GUILayoutOption[] options)
        {
            if (color == null) color = Color.black;
            return ButtonColored(new GUIContent(text), GetStyleButtonByColor(color.Value),
                color.Value.LerpTo(Color.white, 0.75f), options);
        }

        public static bool Button(string text = "Button", params GUILayoutOption[] options)
        {
            return Button(text, null, options);
        }

        public static bool Button(GUIContent content, Color? color = null, params GUILayoutOption[] options)
        {
            if (color == null) color = Color.black;
            return ButtonColored(content, GetStyleButtonByColor(color.Value), color.Value.LerpTo(Color.white, 0.75f),
                options);
        }

        public static bool Button(GUIContent content, params GUILayoutOption[] options)
        {
            return Button(content, null, options);
        }

        private static bool ButtonColored(GUIContent content, GUIStyle style, Color? backgroundColor = null,
            params GUILayoutOption[] options)
        {
            if (backgroundColor == null) backgroundColor = GUI.backgroundColor;

            var prevColor = GUI.backgroundColor;
            GUI.backgroundColor = backgroundColor.Value;

            var result = GUILayout.Button(content, style, options);

            GUI.backgroundColor = prevColor;

            return result;
        }

        private static GUIStyle GetStyleButtonByColor(Color color)
        {
            if (styleButtonFromColorMap.TryGetValue(color, out var style)) return style;

            style = GetStyleButton();
            style.normal.textColor = Color.black;
            style.hover.textColor = color.SetColorIntensity(0.25f);
            style.active.textColor = color;

            styleButtonFromColorMap.Add(color, style);

            return style;
        }
    }

    internal class DrawUnit
    {
        public Rect rect
        {
            get => _rect;
            set
            {
                _rect = value;
                if (_rect.width < minWidth)
                    _rect.width = minWidth;
            }
        }

        public SerializedProperty prop;
        public float weight;
        public float minWidth;

        private Rect _rect;

        public DrawUnit() : this(1)
        {
        }

        public DrawUnit(float w)
        {
            weight = w;
        }

        public static implicit operator Rect(DrawUnit _rect)
        {
            return _rect.rect;
        }
    }
}