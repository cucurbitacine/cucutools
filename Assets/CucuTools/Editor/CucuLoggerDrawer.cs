using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor
{
    [CustomPropertyDrawer(typeof(CucuLogger))]
    internal class CucuLoggerDrawer : PropertyDrawer
    {
        private readonly DrawUnit u_tag = new DrawUnit(0.382f);
        private readonly DrawUnit u_color = new DrawUnit(0);
        private readonly DrawUnit u_type = new DrawUnit(0.618f/2f);
        private readonly DrawUnit u_area = new DrawUnit(0.618f/2f);

        private void UpdateUnits(Rect root, SerializedProperty prop)
        {
            u_tag.prop = prop.FindPropertyRelative("_tag");
            u_color.prop = prop.FindPropertyRelative("_tagColor");
            u_type.prop = prop.FindPropertyRelative("_type");
            u_area.prop = prop.FindPropertyRelative("_logArea");
            
            var queue = new[]
            {
                u_tag,
                u_type,
                u_area
            };
            
            var rects = root.GetSizedRect(queue.Select(q => q.weight).ToArray());
            for (var i = 0; i < queue.Length; i++)
                queue[i].rect = rects[i];
        }
        
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            var root = EditorGUI.PrefixLabel(pos, label);
            
            UpdateUnits(root, prop);

            var name = string.IsNullOrWhiteSpace(u_tag.prop.stringValue) ? "<empty>" : $"[{u_tag.prop.stringValue}]";
            var type = (LogType) u_type.prop.enumValueIndex;
            var area = (LogArea) u_area.prop.enumValueIndex;
            
            EditorGUI.LabelField(u_tag, name, GetStyleLabel(u_color.prop.colorValue));
            EditorGUI.LabelField(u_type, type.ToString(), GetStyleLogType(type));
            EditorGUI.LabelField(u_area, area.ToString(), GetStyleLogArea(area));
        }

        private GUIStyle GetStyleLabel(Color color)
        {
            var style = CucuGUI.GetStyleLabel();
            style.normal.textColor = color;
            return style;
        }
        
        private GUIStyle GetStyleLogType(LogType type)
        {
            var style = CucuGUI.GetStyleLabel();
            style.alignment = TextAnchor.MiddleCenter;
            style.fontStyle = FontStyle.Italic;
            
            var color = Color.black;

            switch (type)
            {
                case LogType.Exception:
                case LogType.Error:
                case LogType.Assert:
                    color = Color.red.LerpTo(Color.black);
                    break;
                case LogType.Warning:
                    color = Color.yellow.LerpTo(Color.black);
                    break;
                default:
                    color = Color.black;
                    break;
            }

            style.normal.textColor = color;
            
            return style;
        }
        
        private GUIStyle GetStyleLogArea(LogArea area)
        {
            var style = CucuGUI.GetStyleLabel();
            style.alignment = TextAnchor.MiddleRight;
            style.fontStyle = FontStyle.Italic;
            
            var color = Color.black;

            switch (area)
            {
                case LogArea.Editor:
                    color = Color.green.LerpTo(Color.black);
                    break;
                case LogArea.Build:
                    color = Color.gray;
                    break;
                case LogArea.File:
                    color = Color.red.LerpTo(Color.black);
                    break;
                case LogArea.Nowhere:
                    color = Color.white;
                    break;
                default:
                    color = Color.black;
                    break;
            }

            style.normal.textColor = color;
            
            return style;
        }
    }
}