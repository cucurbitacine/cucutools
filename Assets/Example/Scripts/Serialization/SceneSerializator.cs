using System.Collections;
using System.Linq;
using CucuTools.Attributes;
using CucuTools.Serializing;
using CucuTools.Serializing.Components;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Example.Scripts.Serialization
{
    public class SceneSerializator : MonoBehaviour
    {
        public string SceneDataName => SceneManager.GetActiveScene().name;

        public UnityEvent OnDeserialized => onDeserialized ?? (onDeserialized = new UnityEvent());
        public UnityEvent OnSerialized=> onSerialized ?? (onSerialized = new UnityEvent());
        
        public SerializedSceneProvider provider;

        [Header("Events")]
        [SerializeField] private UnityEvent onDeserialized;
        [SerializeField] private UnityEvent onSerialized;
        
        private SerializableComponent[] components;

        private Coroutine _deserializing;
        private Coroutine _serializing;

        [CucuButton(colorHex: "A00050")]
        public void DeserializeScene()
        {
            if (provider == null) return;

            if (_deserializing != null) StopCoroutine(_deserializing);
            _deserializing = StartCoroutine(Deserializing());
        }

        [CucuButton(colorHex: "00A050")]
        public void SerializeScene()
        {
            if (provider == null) return;

            if (_serializing != null) StopCoroutine(_serializing);
            _serializing = StartCoroutine(Serializing());
        }

        private IEnumerator Deserializing()
        {
            var reading = provider.ReadScene(SceneDataName);

            while (!reading.IsCompleted)
            {
                yield return null;
            }

            var serializedComponents = reading.Result;

            if ((serializedComponents?.Length ?? 0) == 0) yield break;
            
            components = FindObjectsOfType<SerializableComponent>();
            
            foreach (var component in components)
            {
                if (component == null) continue;

                var serialized = serializedComponents
                    .FirstOrDefault(s => s.Guid == component.GuidEntity.Guid)?
                    .bytes;

                if (serialized == null || serialized.Length == 0) continue;
                
                component.Deserialize(serialized);
            }
            
            OnDeserialized.Invoke();
        }

        private IEnumerator Serializing()
        {
            components = FindObjectsOfType<SerializableComponent>();

            var serializedComponents = components
                .Where(c => c.NeedSerializing)
                .Select(c => new SerializedComponent(c.GuidEntity.Guid, c.Serialize()))
                .ToArray();
            
            var updating = provider.UpdateScene(SceneDataName, serializedComponents);

            while (!updating.IsCompleted)
            {
                yield return null;
            }
            
            OnSerialized.Invoke();
        }
        
        private void Awake()
        {
            DeserializeScene();
        }
    }
}
