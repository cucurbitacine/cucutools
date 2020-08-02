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

        private string searchField;
        
        private int searchIdToolbar = 1;
        private SearchType searchType
        {
            get
            {
                switch (searchIdToolbar)
                {
                    case 0:
                        return SearchType.OnlyRefsOn;
                    case 1:
                        return SearchType.OnlyName;
                    case 2:
                        return SearchType.OnlyRefsTo;
                    default:
                        throw new Exception("Oops");
                }
            }
        }

        private float waiting;
        private DateTime lastUpdate;
        private readonly TimeSpan maxWait = new TimeSpan(0,0,0,2);
        
        [MenuItem(CucuGUI.MenuItemRoot + "Assembly definitions viewer", priority = 1)]
        public static void ShowWindow()
        {
            GetWindow<CucuAsmDefManager>("Assembly definitions");
        }

        private void OnGUI()
        {
            try
            {
                OnGUIInternal();
            }
            catch
            {
            }
        }

        private void OnGUIInternal()
        {
            EditorGUILayout.Knob(new Vector2(20f, 20f),
                waiting, 0f, 1f, "update",
                Color.green.SetColorIntensity(0.2f), Color.green,
                false);
            EditorGUILayout.Separator();
            
            UpdateAssemblies(CucuColorPalette.Rainbow);

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
        
        private void UpdateAssemblies(CucuColorPalette palette = null)
        {
            if (string.IsNullOrWhiteSpace(searchField))
                assembliesFiltered = assemblies;
            else
            {
                assembliesFiltered = assemblies;
                if (searchType == SearchType.OnlyName)
                {
                    assembliesFiltered = assembliesFiltered.FindAll(f =>
                        f.name.ToLower().Contains(searchField.ToLower()));
                }
                
                if (searchType == SearchType.OnlyRefsOn)
                {
                    assembliesFiltered = assembliesFiltered.FindAll(f =>
                        f.refToMe.Any(r => r.name.ToLower().Contains(searchField.ToLower())));
                }
                
                if (searchType == SearchType.OnlyRefsTo)
                {
                    assembliesFiltered = assembliesFiltered.FindAll(f =>
                        f.refTo.Any(r => r.name.ToLower().Contains(searchField.ToLower())));
                }
                
            }

            var now = DateTime.Now;

            var wait = now.Subtract(lastUpdate);

            waiting = 1f * wait.Ticks / maxWait.Ticks;
            
            if (lastUpdate != null && wait.CompareTo(maxWait) < 0)
                return;
            
            lastUpdate = now;

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

            if (palette == null)
                palette = CucuColorPalette.Rainbow;
            
            for (var i = 0; i < assemblies.Count; i++)
            {
                assemblies[i].SetColor(palette.Get((float) i / (assemblies.Count - 1)));
            }
        }

        private void ShowSearchField()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("", GUILayout.Width(position.width * 0.15f));
                    GUILayout.Label("Search : ", new GUIStyle() {alignment = TextAnchor.MiddleRight},
                        GUILayout.Width(position.width * 0.1f));
                    searchField = GUILayout.TextArea(searchField, GUILayout.Width(position.width * 0.5f));
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(20f);

                searchIdToolbar = GUILayout.Toolbar(searchIdToolbar, new[] {"Dependents", "Name", "Dependencies"});
            }
            GUILayout.EndVertical();
        }

        private void ShowHeader()
        {
            GUILayout.BeginHorizontal();
            {
                var style = new GUIStyle()
                    {alignment = TextAnchor.MiddleCenter};
                
                style.fontStyle = searchType == SearchType.OnlyRefsOn ? FontStyle.Bold : FontStyle.Italic;
                style.normal.textColor = searchType == SearchType.OnlyRefsOn ? Color.black : Color.gray;
                GUILayout.Label("Dependents", style, GUILayout.Width(position.width / 2));

                style.fontStyle = searchType == SearchType.OnlyRefsTo ? FontStyle.Bold : FontStyle.Italic;
                style.normal.textColor = searchType == SearchType.OnlyRefsTo ? Color.black : Color.gray;
                GUILayout.Label("Dependencies", style, GUILayout.Width(position.width / 2));
            }
            GUILayout.EndHorizontal();
        }

        private void ShowAsmDefMain(AsmDefLinks asmdef)
        {
            if (!showRefs.TryGetValue(asmdef.name, out var show))
                showRefs.TryAdd(asmdef.name, false);

            if (CucuGUI.Button(asmdef.name, asmdef.color,  GUILayout.Height(20)))
            {
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
            {
                GUILayout.BeginVertical();
                {
                    if (assembly.refToMe.Count == 0) GUILayout.Label("", GUILayout.Width(position.width / 2));
                    foreach (var asmdef in assembly.refToMe)
                        ShowAsmDefSimple(asmdef);
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                {
                    if (assembly.refTo.Count == 0) GUILayout.Label("", GUILayout.Width(position.width / 2));
                    foreach (var asmdef in assembly.refTo)
                        ShowAsmDefSimple(asmdef);
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private void ShowAsmDefSimple(AsmDefLinks asmdef)
        {
            CucuGUI.Button(asmdef.name, asmdef.color, GUILayout.Width(position.width / 2));
        }

        private enum SearchType
        {
            OnlyName,
            OnlyRefsOn,
            OnlyRefsTo,
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