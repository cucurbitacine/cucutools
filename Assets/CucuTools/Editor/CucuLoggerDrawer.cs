using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor
{
    [CustomPropertyDrawer(typeof(CucuLogger))]
    internal class CucuLoggerDrawer : PropertyDrawer
    {
        private readonly DrawUnit u_tag = new DrawUnit(30);
        private readonly DrawUnit u_color = new DrawUnit(20);
        private readonly DrawUnit u_type = new DrawUnit(20);
        private readonly DrawUnit u_area = new DrawUnit(20);

        private void UpdateUnits(Rect root, SerializedProperty prop)
        {
            u_tag.prop = prop.FindPropertyRelative("_tag");
            u_color.prop = prop.FindPropertyRelative("_tagColor");
            u_type.prop = prop.FindPropertyRelative("_type");
            u_area.prop = prop.FindPropertyRelative("_logArea");
            
            var queue = new[]
            {
                u_tag,
                u_color,
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

            EditorGUI.LabelField(u_tag, u_tag.prop.stringValue);
            EditorGUI.ColorField(u_color, u_color.prop.colorValue);
            EditorGUI.EnumPopup(u_type, (LogType) u_type.prop.enumValueIndex);
            EditorGUI.EnumPopup(u_area, (LogArea) u_area.prop.enumValueIndex);
        }
    }
}