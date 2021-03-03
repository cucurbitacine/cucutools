using UnityEngine;

namespace CucuTools
{
    public abstract class SerializableComponent : GuidBehavour
    {
        public abstract bool NeedSerializing { get; }
        public abstract string Serialize();
        public abstract bool Deserialize(string serialized);

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
        where TSerialized : SerializableData
    {
        public sealed override string Serialize()
        {
            ValidateComponent();
            
            return Serializing(ReadComponent());
        }

        public sealed override bool Deserialize(string serialized)
        {
            return TryDeserializing(serialized, out var t) && WriteComponent(t);
        }

        public abstract TSerialized ReadComponent();
        public abstract bool WriteComponent(TSerialized serialized);

        protected virtual string Serializing(TSerialized t)
        {
            return JsonUtility.ToJson(t);
        }
        
        protected virtual bool TryDeserializing(string raw, out TSerialized t)
        {
            t = default;

            if (string.IsNullOrWhiteSpace(raw)) return false;
            
            try
            {
                t = JsonUtility.FromJson<TSerialized>(raw);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}