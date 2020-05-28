using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace CucuTools.Editor
{
    public class CucuAsmDefManager : EditorWindow
    {
        private ConcurrentDictionary<string, bool> showRefs = new ConcurrentDictionary<string, bool>();
        private List<AsmDefLinks> assemblies = new List<AsmDefLinks>();
        private List<AsmDefLinks> assembliesFiltered = new List<AsmDefLinks>();

        private Vector2 scroll;

        private float timer = 0.0f;
        private string searchField;
        private int searchType;
        
        private static float updateTime = 1f;
        
        [MenuItem(CucuGUI.CUCU + "Assembly definitions")]
        public static void ShowWindow()
        {
            GetWindow<CucuAsmDefManager>("Assembly definitions");
        }

        private void OnGUI()
        {
            UpdateAssemblies();

            GUILayout.Space(10);

            ShowUpdateTimeSlider();
            
            GUILayout.Space(20);

            ShowSearchField();

            if (!(assembliesFiltered?.Any() ?? false)) return;
            
            GUILayout.Space(10);

            ShowHeader();

            GUILayout.Space(10);

            scroll = GUILayout.BeginScrollView(scroll);

            foreach (var assembly in assembliesFiltered)
            {
                ShowAsmDefMain(assembly);
                GUILayout.Space(5);
            }


            GUILayout.EndScrollView();
        }

        private void UpdateAssemblies()
        {
            if (string.IsNullOrWhiteSpace(searchField))
                assembliesFiltered = assemblies;
            else
            {
                assembliesFiltered = assemblies;
                if (searchType == 0)
                {
                    assembliesFiltered = assembliesFiltered.FindAll(f =>
                        f.name.ToLower().Contains(searchField.ToLower()) ||
                        f.refToMe.Any(r => r.name.ToLower().Contains(searchField.ToLower())) ||
                        f.refTo.Any(r => r.name.ToLower().Contains(searchField.ToLower())));
                }
                
                if (searchType == 1)
                {
                    assembliesFiltered = assembliesFiltered.FindAll(f =>
                        f.refToMe.Any(r => r.name.ToLower().Contains(searchField.ToLower())));
                }
                
                if (searchType == 2)
                {
                    assembliesFiltered = assembliesFiltered.FindAll(f =>
                        f.refTo.Any(r => r.name.ToLower().Contains(searchField.ToLower())));
                }
                
            }

            if (timer > 0)
            {
                timer -= Time.deltaTime;
                return;
            }

            if (updateTime < 1f) updateTime = 1f;
            timer = updateTime;

            assemblies =
                CompilationPipeline.GetAssemblies(AssembliesType.Player)
                    .Select(s => new AsmDefLinks(s))
                    .OrderBy(o => o.name)
                    .ToList();

            foreach (var asmLinks in assemblies)
            {
                asmLinks.refTo.AddRange(
                    assemblies
                        .Where(all => all != asmLinks)
                        .Where(notMe => asmLinks.assembly.assemblyReferences.Contains(notMe.assembly)));
            }

            foreach (var asmLinks in assemblies)
            {
                asmLinks.refToMe.AddRange(
                    assemblies
                        .Where(all => all != asmLinks)
                        .Where(notMe => notMe.assembly.assemblyReferences.Contains(asmLinks.assembly)));
            }

            for (var i = 0; i < assemblies.Count; i++)
            {
                assemblies[i].SetColor(CucuColor.Palettes.Rainbow.Get((float) i / (assemblies.Count - 1)));
            }
        }

        private void ShowUpdateTimeSlider()
        {
            GUILayout.Label($"Update time : {Math.Round(updateTime, 2)} sec");
            updateTime = GUILayout.HorizontalSlider(updateTime, 1f, 10f);
        }
        
        private void ShowSearchField()
        {
            GUILayout.BeginHorizontal();
            
            searchType = GUILayout.Toolbar(searchType, new[] {"All", "To me", "To"}, GUILayout.Width(position.width *0.3f));

            GUILayout.Label("Search : ", new GUIStyle() {alignment = TextAnchor.MiddleRight},
                GUILayout.Width(position.width * 0.1f));
            searchField = GUILayout.TextArea(searchField, GUILayout.Width(position.width * 0.5f));
            GUILayout.EndHorizontal();
        }

        private void ShowHeader()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("References to me", new GUIStyle() {alignment = TextAnchor.MiddleCenter},
                GUILayout.Width(position.width / 2));
            GUILayout.Label("My references to", new GUIStyle() {alignment = TextAnchor.MiddleCenter},
                GUILayout.Width(position.width / 2));
            GUILayout.EndHorizontal();
        }

        private void ShowAsmDefMain(AsmDefLinks asmdef)
        {
            if (!showRefs.TryGetValue(asmdef.name, out var show))
                showRefs.TryAdd(asmdef.name, false);

            if (CucuGUI.Button(asmdef.name, asmdef.color,  GUILayout.Height(20)))
            {
                //Debug.Log(asmdef.assembly.outputPath);
                showRefs[asmdef.name] = !show;
            }

            if (!showRefs[asmdef.name]) return;

            GUILayout.Space(6);

            ShowAsmDefRefs(asmdef);
            
            GUILayout.Space(10);
        }

        private void ShowAsmDefRefs(AsmDefLinks assembly)
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            if (assembly.refToMe.Count == 0)
                CucuGUI.Button("<empty>", Color.gray, GUILayout.Width(position.width / 2));
            foreach (var asmdef in assembly.refToMe)
                ShowAsmDefSimple(asmdef);

            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            if (assembly.refTo.Count == 0) CucuGUI.Button("<empty>", Color.gray, GUILayout.Width(position.width / 2));
            foreach (var asmdef in assembly.refTo)
                ShowAsmDefSimple(asmdef);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void ShowAsmDefSimple(AsmDefLinks asmdef)
        {
            if (CucuGUI.Button(asmdef.name, asmdef.color, GUILayout.Width(position.width / 2)))
            {
                //Debug.Log(asmdef.assembly.outputPath);
            }
        }
    }

    internal class AsmDefLinks
    {
        public string name => assembly.name;
        public Color color => _color;

        public Assembly assembly { get; private set; }

        public List<AsmDefLinks> refTo =>
            _refTo ?? (_refTo = new List<AsmDefLinks>());

        public List<AsmDefLinks> refToMe =>
            _refToMe ?? (_refToMe = new List<AsmDefLinks>());

        private List<AsmDefLinks> _refTo;
        private List<AsmDefLinks> _refToMe;
        private Color _color;

        public AsmDefLinks(Assembly assembly)
        {
            this.assembly = assembly;
            _color = Color.grey;
        }

        public void SetColor(Color color)
        {
            _color = color;
        }
    }
}