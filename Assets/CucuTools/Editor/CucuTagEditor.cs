using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor
{
    [CustomEditor(typeof(CucuTag))]
    [CanEditMultipleObjects]
    public class CucuTagEditor : UnityEditor.Editor
    {
        private SerializedProperty p_key;
        private SerializedProperty p_gizmos;
        private SerializedProperty p_args;

        private string prev;
        private bool edit;
        
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

            if (!edit)
                EditorGUILayout.LabelField(p_key.stringValue, new GUIStyle(){alignment = TextAnchor.MiddleCenter});

            if (edit)
            {
                EditorGUILayout.PropertyField(p_key, new GUIContent(""));
                if (GUILayout.Button(new GUIContent("Save")))
                {
                    edit = false;
                }
                if (GUILayout.Button(new GUIContent("Undo")))
                {
                    p_key.stringValue = prev;
                    edit = false;
                }
            }
            else
            {
                if (GUILayout.Button(new GUIContent("Edit")))
                {
                    prev = p_key.stringValue;
                    edit = true;
                }
            }
            
            GUILayout.EndHorizontal();

            GUILayout.Space(10f);
            
            var styleGizmos = new GUIStyle(GUI.skin.button);
            styleGizmos.normal.textColor = (p_gizmos.boolValue ? Color.green : Color.red).SetColorIntensity(0.5f);
            
            if (GUILayout.Button($"Draw gizmos mode : {(p_gizmos.boolValue ? "on" : "off")}", styleGizmos))
                p_gizmos.boolValue = !p_gizmos.boolValue;
            
            GUILayout.Space(10f);
            
            if (GUILayout.Button("+"))
            {
                p_args.InsertArrayElementAtIndex(p_args.arraySize);
            }
            
            GUILayout.Space(5f);
            
            del.Clear();
            for (var i = 0; i < p_args.arraySize; i++)
            {
                GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField($"[{i}]", GUILayout.MaxWidth(25f));
                EditorGUILayout.PropertyField(p_args.GetArrayElementAtIndex(i));
                
                var style = new GUIStyle(GUI.skin.button);
                style.normal.textColor = Color.red;
                
                if (GUILayout.Button("X", style))
                    del.Add(i);
                
                GUILayout.EndHorizontal();
            }

            for (var i = del.Count - 1; i >= 0; i--)
            {
                p_args.DeleteArrayElementAtIndex(del[i]);
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        private List<int> del = new List<int>();
    }
}