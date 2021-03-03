using System;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    [Serializable]
    public class SerializedComponent
    {
        public Guid Guid => Guid.TryParse(guid, out var t) ? t : Guid.Empty;
        
        public string guid;
        public string serializedData;

        public SerializedComponent(string guid, string serializedData)
        {
            this.guid = guid;
            this.serializedData = serializedData;
        }
        
        public SerializedComponent(Guid guid, string serializedData) : this(guid.ToString(), serializedData)
        {
        }

        public SerializedComponent(string guid) : this(guid, "")
        {
        }
        
        public SerializedComponent(Guid guid) : this(guid.ToString())
        {
        }
    }

    [Serializable]
    public class SerializedGameObject
    {
        public string name;
        public string tag;
        public int layer;
        public bool activeSelf;

        public SerializedGameObject(GameObject gameObject)
        {
            name = gameObject.name;
            tag = gameObject.tag;
            layer = gameObject.layer;
            activeSelf = gameObject.activeSelf;
        }
    }
    
    [Serializable]
    public class SerializedNewComponent
    {
        public string typeSerializableComponent;
        public string typeComponent;
        public SerializedComponent serializedComponent;

        public SerializedNewComponent(SerializableComponent component)
        {
            var type = component.GetType();
            typeSerializableComponent = type.FullName;
            typeComponent = type.GetProperty("Target")?.GetMethod?.ReturnType?.FullName;
        }
    }

    [Serializable]
    public class SerializedNewGameObject
    {
        public SerializedGameObject gameObject;
        public SerializedNewComponent[] components;

        public SerializedNewGameObject(GameObject gameObject)
        {
            this.gameObject = new SerializedGameObject(gameObject);

            components = gameObject
                .GetComponents<SerializableComponent>()
                .Select(s => new SerializedNewComponent(s))
                .ToArray();
        }
    }
}