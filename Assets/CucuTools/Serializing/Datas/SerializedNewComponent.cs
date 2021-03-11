using System;
using CucuTools.Serializing.Components;

namespace CucuTools.Serializing.Datas
{
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
            serializedComponent = new SerializedComponent(component.GuidEntity.Guid, component.Serialize());
        }
    }
}