using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor
{
    [CustomPropertyDrawer(typeof(CucuTagArg))]
    public class CucuTagArgDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect pos, SerializedProperty pro, GUIContent label)
        {
            var rects = pos.GetSizedRect(10, 10, 35, 10, 35);

            var rectPre = rects[0];
            var rectLKe = rects[1];
            var rectKey = rects[2];
            var rectLVa = rects[3];
            var rectVal = rects[4];

            EditorGUI.LabelField(rectPre, label);

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