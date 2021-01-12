using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(CucuTagArg))]
    internal class CucuTagArgDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect pos, SerializedProperty pro, GUIContent label)
        {
            var root = EditorGUI.PrefixLabel(pos, label);
            EditorGUI.indentLevel = 0;
            var rects = root.GetSizedRect(15, 35, 15, 35);

            var rectLKe = rects[0];
            var rectKey = rects[1];
            var rectLVa = rects[2];
            var rectVal = rects[3];

            EditorGUI.LabelField(
                rectLKe,
                "Key : ",
                new GUIStyle {alignment = TextAnchor.MiddleRight});

            EditorGUI.PropertyField(
                rectKey,
                pro.FindPropertyRelative("key"),
                new GUIContent());

            EditorGUI.LabelField(
                rectLVa,
                "Arg : ",
                new GUIStyle {alignment = TextAnchor.MiddleRight});

            EditorGUI.PropertyField(
                rectVal,
                pro.FindPropertyRelative("value"),
                new GUIContent());
        }
    }
}