using System;

namespace CucuTools.Serializing.Components
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
}