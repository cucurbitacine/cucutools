using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor
{
    public class CucuTagManagerWindow : EditorWindow
    {
        private ConcurrentDictionary<int, CucuTag> currentTags = new ConcurrentDictionary<int, CucuTag>();

        private ConcurrentDictionary<string, List<CucuTag>> groupedTags =
            new ConcurrentDictionary<string, List<CucuTag>>();

        private List<int> tempIDs = new List<int>();
        private List<CucuTag> tempTags = new List<CucuTag>();

        private bool _isActive = true;

        private float bW => position.width * 0.10f;
        private float tW => position.width * 0.40f;
        private float gW => position.width * 0.50f;

        private Vector2 scroll;

        [MenuItem(CucuGUI.CUCU + "Cucu tags viewer", priority = 0)]
        public static void ShowWindow()
        {
            GetWindow<CucuTagManagerWindow>("Cucu Tags");
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
            if (CucuGUI.Button(_isActive ? "Stop" : "Start", _isActive ? Color.red : Color.green))
            {
                _isActive = !_isActive;
            }

            if (_isActive)
            {
                UpdateTags();
                ShowHeaderTags();
                ShowTags();
            }
        }
        
        private void ShowHeaderTags()
        {
            GUILayout.Space(20);
            
            GUILayout.Label($"Total tags : {currentTags.Count}");

            GUILayout.BeginHorizontal();

            GUILayout.Label("Select", GUILayout.Width(bW));
            GUILayout.Label("GameObject", GUILayout.Width(gW));
            GUILayout.Label("Tag", GUILayout.Width(tW));

            GUILayout.EndHorizontal();
            
            GUILayout.Space(20);
        }

        private void ShowTags()
        {
            scroll = GUILayout.BeginScrollView(scroll);

            var indexGroup = 0;
            var countGroup = groupedTags.Count;
            foreach (var groupTags in groupedTags)
            {
                var color = CucuColor.Palettes.Jet.Get((float) indexGroup / (countGroup - 1));

                if (true || groupTags.Value.Count > 1)
                {
                    var groupKey = string.IsNullOrWhiteSpace(groupTags.Key) ? "<empty>" : groupTags.Key;
                    if (CucuGUI.Button($"{groupKey} ({groupTags.Value.Count})", color))
                    {
                        var listTags = groupTags.Value;
                        Selection.objects = listTags.Select(s => s.gameObject).ToArray();
                        Focus(listTags.Select(s => s.transform.position).Sum() / listTags.Count);
                    }
                }

                var indexTag = 0;
                foreach (var tag in groupTags.Value)
                {
                    GUILayout.BeginHorizontal();
                    ShowTag(tag, indexTag + 1, color.LerpTo(Color.white, 0.5f).SetColorIntensity(0.75f));
                    GUILayout.EndHorizontal();
                    ++indexTag;
                }

                GUILayout.Space(10);

                ++indexGroup;
            }

            GUILayout.EndScrollView();
        }

        private void ShowTag(CucuTag tag, int index, Color color)
        {
            if (CucuGUI.Button($"{index}", color, GUILayout.Width(bW)))
            {
                Selection.SetActiveObjectWithContext(tag.gameObject, tag);

                Focus(tag.transform.position);
            }

            GUILayout.Label(tag.gameObject.name, GUILayout.Width(gW));
            GUILayout.Label(string.IsNullOrWhiteSpace(tag.Key) ? "<empty>" : tag.Key, GUILayout.Width(tW));
        }

        private void UpdateTags()
        {
            // Clear from nulls
            foreach (var i in currentTags.Where(w => w.Value == null).Select(s => s.Key))
                currentTags.TryRemove(i, out _);

            // Get all tags
            tempTags.Clear();
            if (!(CucuTag.Tags?.Any() ?? false))
            {
                tempTags.AddRange(FindObjectsOfType<CucuTag>().OrderBy(o => o.Key));
            }
            else
            {
                tempTags.AddRange(CucuTag.Tags.OrderBy(o => o.Key));
            }

            // Get all tag IDs
            tempIDs.Clear();
            tempIDs.AddRange(tempTags.Select(s => s.GetInstanceID()));

            // Remove tags which doesn't exist any more
            foreach (var i in currentTags.Where(w => !tempIDs.Contains(w.Key)).Select(s => s.Key))
                currentTags.TryRemove(i, out _);

            // Add new tags
            foreach (var tag in tempTags.Where(w => !currentTags.ContainsKey(w.GetInstanceID())))
            {
                var id = tag.GetInstanceID();
                if (!currentTags.TryGetValue(id, out _))
                    currentTags.TryAdd(id, tag);
            }

            // Get ordered list tags
            groupedTags.Clear();
            foreach (var t
                in currentTags
                    .GroupBy(t => t.Value.Key)
                    .OrderBy(o => o.Key))
                groupedTags.TryAdd(t.Key ?? "", t.Select(s => s.Value).ToList());
        }

        private void Move(Vector3 point)
        {
            SceneView.lastActiveSceneView.pivot = point;
        }

        private void Focus(Vector3 point)
        {
            var forward = SceneView.lastActiveSceneView.rotation * Vector3.forward;

            Move(point - forward.normalized / 2);
        }
    }
}