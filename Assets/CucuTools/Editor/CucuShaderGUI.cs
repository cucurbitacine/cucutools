using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor
{
    public class CucuShaderGUI : ShaderGUI
    {
        static GUIContent staticLabel = new GUIContent();

        Material _target;
        MaterialEditor _editor;
        MaterialProperty[] _properties;

        public override void OnGUI(MaterialEditor editor, MaterialProperty[] properties)
        {
            _target = editor.target as Material;
            _editor = editor;
            _properties = properties;

            DoMain();
        }

        void DoMain()
        {
            GUILayout.Label("Main Maps", EditorStyles.boldLabel);

            var mainTex = FindProperty("_MainTex");
            _editor.TexturePropertySingleLine(
                MakeLabel(mainTex, "Albedo (RGB)"), mainTex, FindProperty("_Color")
            );

            DoMetallic();
            DoSmoothness();

            DoNormals();
            DoHeights();

            DoEmission();

            _editor.TextureScaleOffsetProperty(mainTex);

            DoOutline();
        }

        void DoNormals()
        {
            var normalMap = FindProperty("_BumpMap");

            _editor.TexturePropertySingleLine(MakeLabel(normalMap), normalMap,
                normalMap.textureValue ? FindProperty("_BumpScale") : null);
        }

        void DoHeights()
        {
            var heightMap = FindProperty("_ParallaxMap");
            var height = FindProperty("_Parallax");

            _editor.TexturePropertySingleLine(MakeLabel(heightMap), heightMap, height);
        }

        void DoMetallic()
        {
            var metallicMap = FindProperty("_MetallicGlossMap");

            EditorGUI.BeginChangeCheck();
            _editor.TexturePropertySingleLine(MakeLabel(metallicMap, "Metallic (R)"), metallicMap,
                metallicMap.textureValue ? null : FindProperty("_Metallic"));
            if (EditorGUI.EndChangeCheck()) SetKeyword("_METALLICGLOSSMAP", metallicMap.textureValue);
        }

        void DoSmoothness()
        {
            var smoothness = FindProperty("_GlossMapScale");

            EditorGUI.indentLevel += 2;
            _editor.ShaderProperty(smoothness, MakeLabel(smoothness));
            EditorGUI.indentLevel -= 2;
        }

        void DoEmission()
        {
            var emissionMap = FindProperty("_EmissionMap");
            var emissionColor = FindProperty("_EmissionColor");

            EditorGUI.BeginChangeCheck();
            _editor.TexturePropertyWithHDRColor(MakeLabel("Emission (RGB)"), emissionMap, emissionColor, false);
            if (EditorGUI.EndChangeCheck()) SetKeyword("_EMISSION", true);
        }

        void DoOutline()
        {
            GUILayout.Label("Outline", EditorStyles.boldLabel);
            var outlineColor = FindProperty("_OutlineColor");
            var outlineWidth = FindProperty("_OutlineWidth");

            _editor.ColorProperty(outlineColor, outlineColor.displayName);
            _editor.RangeProperty(outlineWidth, outlineWidth.displayName);
        }

        MaterialProperty FindProperty(string name)
        {
            return FindProperty(name, _properties);
        }

        static GUIContent MakeLabel(string text, string tooltip = null)
        {
            staticLabel.text = text;
            staticLabel.tooltip = tooltip;
            return staticLabel;
        }

        static GUIContent MakeLabel(MaterialProperty property, string tooltip = null)
        {
            staticLabel.text = property.displayName;
            staticLabel.tooltip = tooltip;
            return staticLabel;
        }

        void SetKeyword(string keyword, bool state)
        {
            if (state)
            {
                _target.EnableKeyword(keyword);
            }
            else
            {
                _target.DisableKeyword(keyword);
            }
        }
        
    }
}