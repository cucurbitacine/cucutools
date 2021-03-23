using System;
using CucuTools.Serializing.Components;
using CucuTools.Serializing.Datas;
using UnityEngine;

namespace Example.Scripts.Serialization.Impl
{
    public class SerializableTransform : JsonSerializableComponent<Transform, SerializedTransform>
    {
        public override SerializedTransform ReadComponent()
        {
            return new SerializedTransform(Target);
        }

        public override bool WriteComponent(SerializedTransform dto)
        {
            Target.localPosition = dto.localPosition;
            Target.localRotation = dto.localRotation;
            Target.localScale = dto.localScale;

            return true;
        }
    }

    [Serializable]
    public class SerializedTransform : SerializedData
    {
        public Vector3 localPosition;
        public Quaternion localRotation;
        public Vector3 localScale;
        
        #region SerializedData

        public SerializedTransform() : this(Vector3.zero, Quaternion.identity, Vector3.one)
        {
        }
        
        public override int SizeOf()
        {
            throw new NotImplementedException();
        }

        public override byte[] Serialize()
        {
            throw new NotImplementedException();
        }

        public override void Deserialize(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        #endregion
        
        public SerializedTransform(Vector3 pos, Quaternion rot, Vector3 sca)
        {
            localPosition = pos;
            localRotation = rot;
            localScale = sca;
        }

        public SerializedTransform(Vector3 pos, Quaternion rot) : this(pos, rot, Vector3.one)
        {
        }
        
        public SerializedTransform(Vector3 pos, Vector3 sca) : this(pos, Quaternion.identity, sca)
        {
        }
        
        public SerializedTransform(Quaternion rot, Vector3 sca) : this(Vector3.zero, rot, sca)
        {
        }
        
        public SerializedTransform(Quaternion rot) : this(Vector3.zero, rot, Vector3.one)
        {
        }
        
        public SerializedTransform(Vector3 pos) : this(pos, Quaternion.identity, Vector3.one)
        {
        }

        public SerializedTransform(Transform tr) : this(tr.localPosition, tr.localRotation, tr.localScale)
        {
        }
    }
}