using System.Linq;
using CucuTools.Math;
using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor.Math
{
    internal abstract class CucuRangeDrawerBase : PropertyDrawer
    {
        protected SerializedProperty limitFirst;
        protected SerializedProperty limitSecond;
        protected SerializedProperty value;

        protected readonly DrawUnit rectLabelMin = new DrawUnit {weight = 1, minWidth = 0};
        protected readonly DrawUnit rectLabelMax = new DrawUnit {weight = 1, minWidth = 0};
        protected readonly DrawUnit rectLabelVal = new DrawUnit {weight = 1, minWidth = 0};

        protected readonly DrawUnit rectMin = new DrawUnit {weight = 10, minWidth = 0};
        protected readonly DrawUnit rectMax = new DrawUnit {weight = 10, minWidth = 0};
        protected readonly DrawUnit rectVal = new DrawUnit {weight = 73, minWidth = 0};

        private void UpdateRect(Rect root)
        {
            DrawUnit[] queue =
            {
                rectLabelMin,
                rectMin,
                
                rectLabelMax,
                rectMax,
                
                rectLabelVal,
                rectVal,
            };

            var rects = root.GetSizedRect(queue.Select(q => q.weight).ToArray());
            for (var i = 0; i < queue.Length; i++)
                queue[i].rect = rects[i];
        }

        public override void OnGUI(Rect pos, SerializedProperty pro, GUIContent label)
        {
            limitFirst = pro.FindPropertyRelative("limitFirst");
            value = pro.FindPropertyRelative("value");
            limitSecond = pro.FindPropertyRelative("limitSecond");
            
            var root = EditorGUI.PrefixLabel(pos, label);
            EditorGUI.indentLevel = 0;
            
            UpdateRect(root);
            
            DrawLabels();
            DrawFields();
        }

        private void DrawPrefix(GUIContent content)
        {
            
        }
        
        protected virtual void DrawLabels()
        {
            EditorGUI.LabelField(rectLabelMin, "", new GUIStyle {alignment = TextAnchor.MiddleRight});
            EditorGUI.LabelField(rectLabelMax, "", new GUIStyle {alignment = TextAnchor.MiddleRight});
            EditorGUI.LabelField(rectLabelVal, "");
        }

        protected abstract void DrawFields();
    }

    [CustomPropertyDrawer(typeof(CucuRangeFloat))]
    internal class CucuRangeFloatDrawer : CucuRangeDrawerBase
    {
        protected override void DrawFields()
        {
            limitFirst.floatValue = EditorGUI.FloatField(rectMin, limitFirst.floatValue);
            limitSecond.floatValue = EditorGUI.FloatField(rectMax, limitSecond.floatValue);
            value.floatValue = EditorGUI.Slider(rectVal, value.floatValue, limitFirst.floatValue, limitSecond.floatValue);
        }
    }
    
    [CustomPropertyDrawer(typeof(CucuRangeInt))]
    internal class CucuRangeIntDrawer : CucuRangeDrawerBase
    {
        protected override void DrawFields()
        {
            limitFirst.intValue = EditorGUI.IntField(rectMin, limitFirst.intValue);
            limitSecond.intValue = EditorGUI.IntField(rectMax, limitSecond.intValue);
            value.intValue = EditorGUI.IntSlider(rectVal, value.intValue, limitFirst.intValue, limitSecond.intValue);
        }
    }
}