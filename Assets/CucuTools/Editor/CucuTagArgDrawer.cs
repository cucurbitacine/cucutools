using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CucuTools.Editor
{
    [CustomPropertyDrawer(typeof(CucuTagArg))]
    public class CucuTagArgDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            var keyField = new PropertyField(property.FindPropertyRelative("key"), "key");
            var valueField = new PropertyField(property.FindPropertyRelative("value"), "value");

            container.Add(keyField);
            container.Add(valueField);

            return container;
        }

        public override void OnGUI(Rect pos, SerializedProperty pro, GUIContent label)
        {
            EditorGUI.BeginProperty(pos, label, pro);
            
            EditorGUI.LabelField(
                new Rect(pos.x + 0, pos.y, pos.width / 6, pos.height), 
                " Key : ");
            
            EditorGUI.PropertyField(
                new Rect(pos.x + pos.width / 6, pos.y, pos.width / 3, pos.height),
                pro.FindPropertyRelative("key"), 
                new GUIContent(""));
            
            EditorGUI.LabelField(
                new Rect(pos.x + pos.width / 2, pos.y, pos.width / 6, pos.height), 
                " Arg : ");
            
            EditorGUI.PropertyField(
                new Rect(pos.x + 2*pos.width / 3, pos.y, pos.width / 3, pos.height),
                pro.FindPropertyRelative("value"), 
                new GUIContent(""));


            EditorGUI.EndProperty();
        }
    }
}