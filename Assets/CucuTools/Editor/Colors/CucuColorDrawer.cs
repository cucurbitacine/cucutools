using CucuTools.Colors;
using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor.Colors
{
    [CustomPropertyDrawer(typeof(CucuColorAttribute))]
    public class CucuColorDrawer : PropertyDrawer
    {
        private static readonly float[] Weights = new float[] { 0.2f, 0.425f, 0.375f };

        private static readonly float PaddingScale = 0.02f;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var pos = EditorGUI.PrefixLabel(position, label);

            pos.x -= pos.width * 0.2f;
            pos.width += pos.width * 0.2f;
            
            var count = Weights.Length;
            var padding = pos.width * PaddingScale;

            var width = (pos.width - (count - 1) * padding);
            
            var rects = new Rect[count];

            for (int i = 0; i < count; i++)
            {
                if (i == 0) rects[i] = pos;
                else
                {
                    rects[i] = rects[i - 1];
                    rects[i].x = rects[i - 1].x + rects[i - 1].width + padding;
                }

                rects[i].width = width * Weights[i];
            }
            
            var color = property.colorValue;
            color = EditorGUI.ColorField(rects[0], color);

            var hex = CucuColor.Color2Hex(color);
            var hexNew = EditorGUI.TextField(rects[1], hex.Substring(0, 6));
            
            if (CucuColor.TryGetColorFromHex(hexNew, out _))
                hex = hexNew;
            else
            {
                Debug.Log("???");
            }
            var alpha = (int) (255 * color.a);
            alpha = EditorGUI.IntSlider(rects[2], alpha, 0, 255);

            property.colorValue = hex.ToColor().AlphaTo(alpha / 255f);
        }
    }
}
