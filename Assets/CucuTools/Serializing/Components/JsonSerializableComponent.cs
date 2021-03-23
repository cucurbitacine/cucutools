using System.Text;
using CucuTools.Serializing.Datas;
using UnityEngine;

namespace CucuTools.Serializing.Components
{
    public abstract class JsonSerializableComponent<TComponent, TSerialized> : SerializableComponent<TComponent, TSerialized>
        where TComponent : Component
        where TSerialized : SerializedData, new()
    {
        protected virtual Encoding Encoding => Encoding.Default;
        
        protected override sealed bool TrySerializing(TSerialized t, out byte[] bytes)
        {
            bytes = null;

            try
            {
                bytes = Encoding.GetBytes(JsonSerialize<TSerialized>(t));
            }
            catch
            {
                return false;
            }

            return bytes != null;
        }
        
        protected override sealed bool TryDeserializing(byte[] bytes, out TSerialized t)
        {
            t = default;
            
            try
            {
                t = JsonDeserialize<TSerialized>(Encoding.GetString(bytes));
            }
            catch
            {
                return false;
            }

            return true;
        }

        protected virtual string JsonSerialize<T>(T data)
        {
            return JsonUtility.ToJson(data);
        }
        
        protected virtual T JsonDeserialize<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }
    }
}