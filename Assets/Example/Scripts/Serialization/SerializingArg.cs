using System;
using CucuTools.ArgumentInjector;
using CucuTools.Serializing.Components;

namespace Example.Scripts.Serialization
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