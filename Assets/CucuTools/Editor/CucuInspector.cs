using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CucuTools.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Object), true, isFallback = false)]
    public class CucuInspector : UnityEditor.Editor
    {
        private const string DefaultButtonsGroupName = "Cucu buttons";

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            DrawCucuButtons(target);
        }

        #region Drawing

        public static void DrawCucuButtons(Object target)
        {
            var buttons = GetButtons(target);
            
            if (buttons == null || !buttons.Any()) return;

            var grouped = buttons.GroupBy(b => b.attribute.Group).OrderBy(g => g.Min(gr => gr.attribute.Order));

            foreach (var group in grouped)
            {
                var groupName = string.IsNullOrEmpty(group.Key) ? DefaultButtonsGroupName : group.Key;
                DrawGroup(target, group, groupName);
            }
        }
        
        private static void DrawGroup(Object target, IEnumerable<ButtonInfo> buttons, string groupName)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(groupName, GetHeaderGUIStyle());

            foreach (var button in buttons.OrderBy(b => b.attribute.Order))
                DrawButton(target, button);
        } 
        
        private static void DrawButton(Object target, ButtonInfo button)
        {
            var attribute = button.attribute;
            var method = button.method;
                
            var buttonName = string.IsNullOrEmpty(attribute.Name) ? method.Name : attribute.Name;
            var backgroundColor = GUI.backgroundColor;

            try
            {
                if (!string.IsNullOrEmpty(button.attribute.Color) &&
                    ColorUtility.TryParseHtmlString(button.attribute.Color, out var color))
                    GUI.backgroundColor = color;

                if (GUILayout.Button(buttonName, GetButtonGUIStyle()))
                {
                    method.Invoke(target, null);
                }
            }
            finally
            {
                GUI.backgroundColor = backgroundColor;
            }
        }

        #endregion

        #region Getter

        private static IEnumerable<MethodInfo> GetButtonMethods(object target)
        {
            return target?.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(m => m.GetCustomAttributes(typeof(CucuButtonAttribute), true).Length > 0);
        }
        
        private static IEnumerable<ButtonInfo> GetButtons(object target)
        {
            var methods = GetButtonMethods(target)
                .Where(m => m.GetParameters().All(p => p.IsOptional));

            return methods.Select(m => new ButtonInfo(m)).Where(b => b.attribute != null);
        }

        #endregion

        #region GUIStyle

        private static GUIStyle GetButtonGUIStyle()
        {
            return new GUIStyle(GUI.skin.button);;
        }
        
        private static GUIStyle GetHeaderGUIStyle()
        {
            return new GUIStyle(EditorStyles.centeredGreyMiniLabel)
            {
                fontStyle = FontStyle.Bold, alignment = TextAnchor.UpperCenter
            };
        }

        #endregion
        
        public class ButtonInfo
        {
            public CucuButtonAttribute attribute;
            public MethodInfo method;

            public ButtonInfo(MethodInfo method)
            {
                this.method = method;

                attribute = (CucuButtonAttribute) this.method.GetCustomAttributes(typeof(CucuButtonAttribute), true)
                    .SingleOrDefault();
            }
        }
    }
}