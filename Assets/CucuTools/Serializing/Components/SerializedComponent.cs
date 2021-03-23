using System;

namespace CucuTools.Serializing.Components
{
    [Serializable]
    public class SerializedComponent
    {
        public Guid Guid => Guid.TryParse(guid, out var t) ? t : Guid.Empty;
        
        public string guid;
        public string raw;
        public byte[] bytes;

        public SerializedComponent(string guid, byte[] bytes)
        {
            this.guid = guid;
            this.bytes = bytes;
        }
        
        public SerializedComponent(Guid guid, byte[] bytes) : this(guid.ToString(), bytes)
        {
        }

        public SerializedComponent(string guid) : this(guid, new byte[0])
        {
        }
        
        public SerializedComponent(Guid guid) : this(guid.ToString())
        {
        }
    }
}