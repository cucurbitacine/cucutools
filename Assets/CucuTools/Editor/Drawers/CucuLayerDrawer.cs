using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(CucuLayerAttribute))]
    public class CucuLayerDrawer : PropertyDrawer
    {
        Dictionary<int, string> layers = new Dictionary<int, string>();
        Dictionary<int, int> numberMapLayer = new Dictionary<int, int>();

        private int selectedLayer;

        public override void OnGUI(Rect pos, SerializedProperty pro, GUIContent label)
        {
            try
            {
                UpdateLayers();
                Show(pos, pro, label);
            }
            catch
            {
            }
        }

        private void Show(Rect pos, SerializedProperty pro, GUIContent label)
        {
            pos = EditorGUI.PrefixLabel(pos, label);

            if (!layers.TryGetValue(pro.intValue, out _))
            {
                pro.intValue = 0;
                selectedLayer = 0;
            }

            if (!numberMapLayer.TryGetValue(selectedLayer, out var lay) || lay != pro.intValue)
            {
                selectedLayer = numberMapLayer.Single(s => s.Value == pro.intValue).Key;
            }
            
            selectedLayer = EditorGUI.Popup(pos, selectedLayer, layers.Select(s => s.Value).ToArray());

            pro.intValue = numberMapLayer[selectedLayer];
        }

        private void UpdateLayers()
        {
            layers = GetLayers();

            var index = 0;
            numberMapLayer = layers.ToDictionary(layer => index++, layer => layer.Key);
        }

        private Dictionary<int, string> GetLayers()
        {
            var layers = new Dictionary<int, string>();

            for (var i = 0; i < 8; i++)
            {
                if (Cucu.TryGetLayerName(i, out var name))
                    layers.Add(i, name);
            }

            var index = 8;
            var exit = false;
            while (!exit)
            {
                exit = !Cucu.TryGetLayerName(index, out var name);
                if (!exit) layers.Add(index, name);
                ++index;
            }

            return layers;
        }
    }
}