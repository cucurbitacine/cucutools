using CucuTools.Common;
using CucuTools.Serializing.Datas;
using UnityEngine;

namespace CucuTools.Serializing.Components
{
    public abstract class SerializableComponent : GuidBehavour
    {
        public abstract bool NeedSerializing { get; }
        public abstract byte[] Serialize();
        public abstract void Deserialize(byte[] bytes);

        protected virtual void OnDrawGizmos()
        {
        }
    }

    public abstract class SerializableComponent<TComponent> : SerializableComponent
        where TComponent : Component
    {
        public override bool NeedSerializing => needSerializing;

        public TComponent Target
        {
            get => target;
            protected set => target = value;
        }

        protected virtual void ValidateComponent()
        {
            if (Target == null) Target = GetComponent<TComponent>();
        }
        
        [SerializeField] protected bool needSerializing = true;
        [SerializeField] private TComponent target;
        
        protected override void OnValidate()
        {
            base.OnValidate();

            ValidateComponent();
        }
    }

    public abstract class SerializableComponent<TComponent, TSerialized> : SerializableComponent<TComponent>
        where TComponent : Component
        where TSerialized : SerializedData, new()
    {
        public sealed override byte[] Serialize()
        {
            ValidateComponent();

            return TrySerializing(ReadComponent(), out var t) ? t : null;
        }

        public sealed override void Deserialize(byte[] bytes)
        {
            if (TryDeserializing(bytes, out var t)) WriteComponent(t);
        }

        public abstract TSerialized ReadComponent();
        public abstract bool WriteComponent(TSerialized serialized);

        protected virtual bool TrySerializing(TSerialized t, out byte[] bytes)
        {
            bytes = null;

            try
            {
                bytes = t.Serialize();
            }
            catch
            {
                return false;
            }

            return true;
        }

        protected virtual bool TryDeserializing(byte[] bytes, out TSerialized t)
        {
            t = new TSerialized();

            try
            {
                t.Deserialize(bytes);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}