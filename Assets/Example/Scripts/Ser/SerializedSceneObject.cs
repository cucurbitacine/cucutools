using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CucuTools.Serializing;
using CucuTools.Serializing.Components;
using UnityEngine;

namespace Example.Scripts.Ser
{
    [CreateAssetMenu(fileName = "SerializedComponents", menuName = "CucuCreate", order = 0)]
    public class SerializedSceneObject : ScriptableObject, ISerializedSceneProvider
    {
        private List<SerializedScene> storage => _storage ?? (_storage = new List<SerializedScene>());
        
        [SerializeField] private List<SerializedScene> _storage;
        
        public Task CreateScene(string sceneDataName, params SerializedComponent[] components)
        {
            storage.Clear();

            var scene = new SerializedScene(sceneDataName);
            scene.CreateComponents(components);
            
            storage.Add(scene);
            
            return Task.CompletedTask;
        }

        public Task<SerializedComponent[]> ReadScene(string sceneDataName)
        {
            return Task.FromResult(storage.FirstOrDefault(s => s.SceneName == sceneDataName)?.ReadComponents() ??
                                   new SerializedComponent[0]);
        }

        public Task UpdateScene(string sceneDataName, params SerializedComponent[] components)
        {
            var scene = storage.FirstOrDefault(s => s.SceneName == sceneDataName);
            
            if (scene == null)
            {
                scene = new SerializedScene(sceneDataName);
                storage.Add(scene);
            }

            scene.UpdateComponents(components);
            
            return Task.CompletedTask;
        }

        public Task DeleteScenes(params string[] sceneDataNames)
        {
            storage.RemoveAll(s => sceneDataNames.Any(sn => sn == s.SceneName));
            
            return Task.CompletedTask;
        }
        
        [Serializable]
        public class SerializedScene : ISerializedComponentProvider
        {
            public string SceneName => sceneName;
            
            [SerializeField] private string sceneName;
            [SerializeField] private List<SerializedComponent> _storage;

            protected List<SerializedComponent> storage => _storage ?? (_storage = new List<SerializedComponent>());

            public SerializedScene(string sceneName)
            {
                this.sceneName = sceneName;
            }
            
            public void CreateComponents(params SerializedComponent[] components)
            {
                storage.Clear();

                foreach (var component in components)
                {
                    if (component == null) continue;
                    storage.Add(component);
                }
            }

            public SerializedComponent[] ReadComponents()
            {
                return storage.ToArray();
            }

            public void UpdateComponents(params SerializedComponent[] components)
            {
                foreach (var component in components)
                {
                    if (component == null) continue;

                    var current = storage.FirstOrDefault(s => s.guid == component.guid);
                    if (current == null)
                    {
                        current = new SerializedComponent(component.guid);
                        storage.Add(current);
                    }
                    current.serializedData = component.serializedData;
                }
            }

            public void DeleteComponents(params SerializedComponent[] components)
            {
                storage.RemoveAll(s => components.Any(c => c.guid == s.guid));
            }
        }
    }

    public interface ISerializedComponentProvider
    {
        void CreateComponents(params SerializedComponent[] components);
        SerializedComponent[] ReadComponents();
        void UpdateComponents(params SerializedComponent[] components);
        void DeleteComponents(params SerializedComponent[] components);
    }
}