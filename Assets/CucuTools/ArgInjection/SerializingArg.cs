using System;

namespace CucuTools
{
    [Serializable]
    public class SerializingArg : CucuArg
    {
        public SerializedComponent serializedComponent;

        public SerializingArg()
        {
        }
        
        public SerializingArg(SerializedComponent serializedComponent)
        {
            this.serializedComponent = serializedComponent;
        }
    }
}