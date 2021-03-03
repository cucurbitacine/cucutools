using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CucuTools
{
    public class SceneSerializator : MonoBehaviour
    {
        public string SceneName => SceneManager.GetActiveScene().name;
        
        public SerializedSceneProvider provider;
        
        public SerializableComponent[] components;
        
        [CucuButton]
        public void DeserializeScene()
        {
            if (provider == null) return;
            
            var serializedComponents = provider.ReadScene(SceneName);

            if ((serializedComponents?.Length ?? 0) == 0) return;
            
            components = FindObjectsOfType<SerializableComponent>();
            
            foreach (var component in components)
            {
                if (component == null) continue;

                var serialized = serializedComponents
                    .FirstOrDefault(s => s.Guid == component.GuidEntity.Guid)?
                    .serializedData;

                if (string.IsNullOrWhiteSpace(serialized)) continue;
                
                component.Deserialize(serialized);
            }
        }
        
        [CucuButton]
        public void SerializeScene()
        {
            if (provider == null) return;

            components = FindObjectsOfType<SerializableComponent>();

            var serializedComponents = components
                .Where(c => c.NeedSerializing)
                .Select(c => new SerializedComponent(c.GuidEntity.Guid, c.Serialize()))
                .ToArray();
            
            provider.UpdateScene(SceneName, serializedComponents);
        }
        
        private void Awake()
        {
            DeserializeScene();
        }
    }
}
