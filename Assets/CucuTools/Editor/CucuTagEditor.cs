using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor
{
    [CustomEditor(typeof(CucuTag))]
    [CanEditMultipleObjects]
    internal class CucuTagEditor : UnityEditor.Editor
    {
        private SerializedProperty p_key;
        private SerializedProperty p_gizmos;
        private SerializedProperty p_args;
        
        private readonly List<int> deleteListIndex = new List<int>();
        private string currKeyValue;
        private string prevKeyValue;
        private bool isEditMode;

        private void OnEnable()
        {
            p_key = serializedObject.FindProperty("_key");
            p_gizmos = serializedObject.FindProperty("_drawGizmos");
            p_args = serializedObject.FindProperty("args");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.BeginHorizontal();

            if (isEditMode)
            {
                DrawFieldKey();
                DrawButtonSaveKey();
                DrawButtonCancelKey();
            }
            else
            {
                DrawLabelKey();
                DrawButtonEditKey();
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(10f);

            DrawButtonGizmos();

            GUILayout.Space(10f);

            DrawButtonCreateArg();

            GUILayout.Space(5f);

            DrawListArgs();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawLabelKey()
        {
            var tag = p_key?.stringValue;
            var isValid= !string.IsNullOrWhiteSpace(tag);
            tag = isValid ? p_key.stringValue : "<empty>";

            EditorGUILayout.LabelField(tag, new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = isValid ? FontStyle.Bold : FontStyle.Italic
            });
        }

        private void DrawFieldKey()
        {
            var style = new GUIStyle(GUI.skin.textField);
            style.alignment = TextAnchor.MiddleCenter;
            
            currKeyValue = GUILayout.TextField(currKeyValue, 255, style);
        }

        private void DrawButtonEditKey()
        {
            if (CucuGUI.Button("Edit", Color.blue, GUILayout.MaxWidth(120)))
            {
                currKeyValue = p_key.stringValue;
                prevKeyValue = currKeyValue;
                isEditMode = true;
            }
        }

        private void DrawButtonSaveKey()
        {
            if (CucuGUI.Button("Save", Color.blue, GUILayout.MaxWidth(60)))
            {
                isEditMode = false;
                p_key.stringValue = currKeyValue;
            }
        }

        private void DrawButtonCancelKey()
        {
            if (CucuGUI.Button("Cancel", Color.red, GUILayout.MaxWidth(60)))
            {
                p_key.stringValue = prevKeyValue;
                isEditMode = false;
            }
        }

        private void DrawButtonCreateArg()
        {
            if (CucuGUI.Button("Create argument", Color.yellow))
                p_args.InsertArrayElementAtIndex(p_args.arraySize);
        }

        private void DrawElementArg(int i)
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(p_args.GetArrayElementAtIndex(i));

            DrawButtonDeleteArg(i);

            GUILayout.EndHorizontal();
        }

        private void DrawButtonDeleteArg(int i)
        {
            if (CucuGUI.Button("Delete", Color.red, GUILayout.MaxWidth(60)))
                deleteListIndex.Add(i);
        }

        private void DrawListArgs()
        {
            deleteListIndex.Clear();

            for (var i = 0; i < p_args.arraySize; i++)
                DrawElementArg(i);

            for (var i = deleteListIndex.Count - 1; i >= 0; i--)
                p_args.DeleteArrayElementAtIndex(deleteListIndex[i]);
        }

        private void DrawButtonGizmos()
        {
            if (CucuGUI.Button($"Draw gizmos mode : {(p_gizmos.boolValue ? "on" : "off")}",
                p_gizmos.boolValue ? Color.green : Color.red))
                p_gizmos.boolValue = !p_gizmos.boolValue;
        }
    }
}