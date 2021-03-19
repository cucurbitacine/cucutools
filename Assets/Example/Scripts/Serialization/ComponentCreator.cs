using System;
using CucuTools.Attributes;
using CucuTools.Serializing.Components;
using CucuTools.Serializing.Datas;
using UnityEngine;

namespace Example.Scripts.Serialization
{
    public class ComponentCreator : MonoBehaviour
    {
        public GameObject origin;
        public string json;
        public GameObject copy;

        [CucuButton]
        public void Create()
        {
            var newGameObject = new SerializedNewGameObject(origin);
            json = JsonUtility.ToJson(newGameObject);
            newGameObject = JsonUtility.FromJson<SerializedNewGameObject>(json);

            if (copy != null) DestroyImmediate(copy);
            copy = newGameObject.gameObject.Create();

            foreach (var newComponent in newGameObject.components)
            {
                copy.CreateComponent(newComponent.typeComponent);
                var comp = (SerializableComponent) copy.CreateComponent(newComponent.typeSerializableComponent);
                comp.GuidEntity.UpdateGuid(newComponent.serializedComponent.guid);
                comp.Deserialize(newComponent.serializedComponent.serializedData);
            }
        }
    }

    public static class ComponentCreatorExt
    {
        public static Component CreateComponent(this GameObject gameObject, string fullName)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var type = assembly.GetType(fullName);
                
                if (type != null)
                {
                    return gameObject.gameObject.AddComponent(type);
                }
            }

            return null;
        }
    }
}
