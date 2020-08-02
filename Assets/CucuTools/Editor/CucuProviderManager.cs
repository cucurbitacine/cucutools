using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CucuTools.Editor
{
    public class CucuProviderManager : EditorWindow
    {
        private ConcurrentDictionary<string, bool> showServs = new ConcurrentDictionary<string, bool>();

        private ConcurrentStack<CucuServiceProvider> providers = new ConcurrentStack<CucuServiceProvider>();
        private Vector2 scroll;

        private DateTime lastUpdate;
        private readonly TimeSpan maxWait = new TimeSpan(0,0,0,2);
        
        private float waiting;
        [MenuItem(CucuGUI.MenuItemRoot + "Service providers viewer", priority = 1)]
        public static void ShowWindow()
        {
            GetWindow<CucuProviderManager>("Service providers");
        }

        private void OnDestroy()
        {
            showServs.Clear();
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

            UpdateProviders();
            
            ShowProviders(providers, CucuColorPalette.Rainbow);
        }
        
        private void UpdateProviders()
        {
            var now = DateTime.Now;

            var wait = now.Subtract(lastUpdate);

            waiting = 1f * wait.Ticks / maxWait.Ticks;
            
            if (lastUpdate != null && wait.CompareTo(maxWait) < 0)
                return;
            
            lastUpdate = now;
            
            if (!CucuServiceProvider.TryGetProviders(out var t_providers)) return;

            providers.Clear();
            providers.PushRange(t_providers.ToArray());
        }
        
        private void ShowProviders(IEnumerable<CucuServiceProvider> providers, CucuColorPalette palette)
        {
            var index = 0;
            var countProviders = providers.Count(); 
            
            scroll = GUILayout.BeginScrollView(scroll);
            
            foreach (var provider in providers)
            {
                var t = (float) index++ / (countProviders - 1);
                var rootColor = palette.Get(t);

                ShowProvider(provider, rootColor);
                
                GUILayout.Space(10f);
            }

            GUILayout.EndScrollView();
        }
        
        private void ShowProvider(CucuServiceProvider provider, Color color)
        {
            if(!provider.TryGetServices(out var services)) return;
                
            if (!showServs.TryGetValue(provider.name, out var show))
                showServs.TryAdd(provider.name, false);

            if (CucuGUI.Button(provider.name + (showServs[provider.name]?"":$" ({services.Count})"), color))
            {
                showServs[provider.name] = !show;
            }

            if (!showServs[provider.name]) return;

            GUILayout.Space(5f);
            
            foreach (var group in services.GroupBy(g => g.Value is Component))
            {
                ShowServices(group, color, group.Key);
            }
        }
        
        private void ShowServices(IEnumerable<KeyValuePair<Type,object>> services, Color rootColor, bool isComponent)
        {
            GUILayout.BeginVertical();
                    
            if (isComponent)
                foreach (var service in services)
                    ShowServiceAsComponent(service.Key, service.Value as Component, rootColor);
            else
                foreach (var service in services)
                    ShowServiceAsClass(service.Key, service.Value, rootColor);
                    
            GUILayout.EndVertical();
        }
        
        private void ShowServiceAsComponent(Type type, Object component, Color color)
        {
            GUILayout.BeginHorizontal();
            CucuGUI.Button(type.FullName, color.LerpTo(Color.white),
                GUILayout.Width(0.5f * position.width));
            EditorGUILayout.ObjectField(component, typeof(Object), true, GUILayout.Width(0.5f * position.width));
            GUILayout.EndHorizontal();
        }

        private void ShowServiceAsClass(Type type, object service, Color color)
        {
            GUILayout.BeginHorizontal();
            CucuGUI.Button(type.FullName, color.LerpTo(Color.white), GUILayout.Width(0.5f * position.width));
            CucuGUI.Button(service.GetType().Name, color.LerpTo(Color.white), GUILayout.Width(0.5f * position.width));
            GUILayout.EndHorizontal();
        }
    }
}
