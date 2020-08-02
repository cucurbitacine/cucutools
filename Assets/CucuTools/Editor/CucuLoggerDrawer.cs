using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor
{
    [CustomPropertyDrawer(typeof(CucuLogger))]
    internal class CucuLoggerDrawer : PropertyDrawer
    {
        private readonly DrawUnit u_tag = new DrawUnit(1);
        private readonly DrawUnit u_area = new DrawUnit(1);
        private readonly DrawUnit u_type = new DrawUnit(1);

        private List<DrawUnit> queue;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            UpdateProperties(prop);

            var root = EditorGUI.PrefixLabel(pos, label);

            UpdateRect(root);

            DrawProp();
        }

        private void UpdateProperties(SerializedProperty prop)
        {
            u_tag.prop = prop.FindPropertyRelative("_tag");
            u_type.prop = prop.FindPropertyRelative("_selectedType");
            u_area.prop = prop.FindPropertyRelative("_selectedArea");
        }

        private void UpdateRect(Rect root)
        {
            if (queue == null) queue = new List<DrawUnit>();
            if (!queue.Any()) queue.AddRange(new[] {u_tag, u_area, u_type,});

            var rects = root.GetSizedRect(queue.Select(q => q.weight).ToArray());
            for (var i = 0; i < queue.Count; i++) queue[i].rect = rects[i];
        }

        private void DrawProp()
        {
            u_tag.prop.stringValue = EditorGUI.TextField(u_tag, u_tag.prop.stringValue);

            u_area.prop.enumValueIndex =
                (int) (LogArea) EditorGUI.EnumPopup(u_area, (LogArea) u_area.prop.enumValueIndex);

            u_type.prop.enumValueIndex =
                (int) (LogType) EditorGUI.EnumPopup(u_type, (LogType) u_type.prop.enumValueIndex);
        }
    }
}