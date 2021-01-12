using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(CucuServiceProvider))]
    public class CucuProviderDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var prev = EditorGUI.indentLevel;
            
            var root = EditorGUI.PrefixLabel(position, label);
            var array = new float[] {0.328f, 0.618f};
            var rects = root.GetSizedRect(array);
            
            EditorGUI.indentLevel = 0;

            
            var p_name = property.FindPropertyRelative("nameString");
            var name = p_name?.stringValue;
            name = string.IsNullOrWhiteSpace(name) ? "<undefined>" : name;

            var p_guid = property.FindPropertyRelative("guidString");
            var guid = p_guid?.stringValue;
            guid = string.IsNullOrWhiteSpace(guid) ? "<undefined>" : guid;

            EditorGUI.LabelField(rects[0], name);
            
            EditorGUI.LabelField(rects[1], guid, GetStyleGUID());
            
            EditorGUI.indentLevel = prev;
        }

        private GUIStyle GetStyleGUID()
        {
            var style = CucuGUI.GetStyleLabel();
            style.fontSize = (int) (style.fontSize / 1.5f);
            style.alignment = TextAnchor.MiddleRight;
            style.fontStyle = FontStyle.Italic;
            return style;
        }
    }
}
