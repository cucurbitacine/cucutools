using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor
{
    public abstract class CucuRangeDrawerBase : PropertyDrawer
    {
        protected SerializedProperty limitFirst;
        protected SerializedProperty limitSecond;
        protected SerializedProperty value;
        
        protected class _Rect
        {
            public Rect rect
            {
                get => _rect;
                set
                {
                    _rect = value;
                    if (_rect.width < minWidth)
                        _rect.width = minWidth;
                }
            }

            private Rect _rect;
            public float weight;
            public float minWidth;

            public static implicit operator Rect(_Rect _rect)
            {
                return _rect.rect;
            }
        }

        protected readonly _Rect rectPrefix = new _Rect {rect = new Rect(), weight = 10, minWidth = 25};

        protected readonly _Rect rectLabelMin = new _Rect {rect = new Rect(), weight = 5, minWidth = 25};
        protected readonly _Rect rectLabelMax = new _Rect {rect = new Rect(), weight = 5, minWidth = 25};
        protected readonly _Rect rectLabelVal = new _Rect {rect = new Rect(), weight = 5, minWidth = 25};

        protected readonly _Rect rectMin = new _Rect {rect = new Rect(), weight = 15, minWidth = 50};
        protected readonly _Rect rectMax = new _Rect {rect = new Rect(), weight = 15, minWidth = 50};
        protected readonly _Rect rectVal = new _Rect {rect = new Rect(), weight = 45, minWidth = 10};

        private void UpdateRect(Rect root)
        {
            _Rect[] queue =
            {
                rectPrefix,

                rectLabelMin,
                rectMin,

                rectLabelVal,
                rectVal,

                rectLabelMax,
                rectMax,
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

            UpdateRect(pos);

            DrawPrefix(label);
            DrawLabels();
            DrawFields();
        }

        private void DrawPrefix(GUIContent content)
        {
            EditorGUI.PrefixLabel(rectPrefix, content);
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
    public class CucuRangeFloatDrawer : CucuRangeDrawerBase
    {
        protected override void DrawFields()
        {
            limitFirst.floatValue = EditorGUI.FloatField(rectMin, limitFirst.floatValue);
            limitSecond.floatValue = EditorGUI.FloatField(rectMax, limitSecond.floatValue);
            value.floatValue = EditorGUI.Slider(rectVal, value.floatValue, limitFirst.floatValue, limitSecond.floatValue);
        }
    }
    
    [CustomPropertyDrawer(typeof(CucuRangeInt))]
    public class CucuRangeIntDrawer : CucuRangeDrawerBase
    {
        protected override void DrawFields()
        {
            limitFirst.intValue = EditorGUI.IntField(rectMin, limitFirst.intValue);
            limitSecond.intValue = EditorGUI.IntField(rectMax, limitSecond.intValue);
            value.intValue = EditorGUI.IntSlider(rectVal, value.intValue, limitFirst.intValue, limitSecond.intValue);
        }
    }
}