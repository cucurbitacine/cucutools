﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools.Editor
{
    internal static class CucuGUI
    {
        public static Rect[] GetSizedRect(this Rect root,  params float[] values)
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
            return ButtonColored(new GUIContent(text), GetStyleButtonByColor(color.Value), color.Value.LerpTo(Color.white, 0.75f), options);
        }
        
        public static bool Button(string text = "Button", params GUILayoutOption[] options)
        {
            return Button(text, null, options);
        }
        
        public static bool Button(GUIContent content, Color? color = null, params GUILayoutOption[] options)
        {
            if (color == null) color = Color.black;
            return ButtonColored(content, GetStyleButtonByColor(color.Value), color.Value.LerpTo(Color.white, 0.75f), options);
        }
        
        public static bool Button(GUIContent content,  params GUILayoutOption[] options)
        {
            return Button(content, null, options);
        }
        
        private static bool ButtonColored(GUIContent content, GUIStyle style, Color? backgroundColor = null , params GUILayoutOption[] options)
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
            
            style = new GUIStyle(GUI.skin.button)
            {
                normal = {textColor = Color.black},
                hover = {textColor = color.SetColorIntensity(0.25f)},
                active = {textColor = color}
            };

            styleButtonFromColorMap.Add(color, style);
            
            return style;
        }
    }
}