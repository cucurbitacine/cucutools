using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor
{
    public abstract class SelectStringDrawerBase<T> : PropertyDrawer
        where T : SelectStringAttribute
    {
        private Dictionary<string, int> _strings;

        private int _selectedString;

        public override void OnGUI(Rect pos, SerializedProperty pro, GUIContent label)
        {
            try
            {
                var t = (T) attribute;

                UpdateStrings(t);
                Show(pos, pro, label);
            }
            catch (Exception exc)
            {
                Debug.LogError(exc.ToString());
            }
        }

        private void Show(Rect pos, SerializedProperty pro, GUIContent label)
        {
            if (_strings == null || _strings.Count == 0)
            {
                EditorGUI.PropertyField(pos, pro);
                return;
            }
            
            pos = EditorGUI.PrefixLabel(pos, label);
            
            var curr = pro.stringValue;

            if (!_strings.TryGetValue(curr, out _selectedString))
            {
                var first = _strings.FirstOrDefault();
                pro.stringValue = first.Key;
                _selectedString = first.Value;
            }

            _selectedString = EditorGUI.Popup(pos, _selectedString, _strings.Select(s => s.Key).ToArray());

            pro.stringValue = _strings.SingleOrDefault(s => s.Value == _selectedString).Key;
        }

        private void UpdateStrings(T t)
        {
            _strings = GetStrings(t).Select((s, ind) => (s, ind)).ToDictionary(d => d.s, d => d.ind);
        }

        private static IEnumerable<string> GetStrings(T t)
        {
            return t.GetStrings();
        }
    }
}