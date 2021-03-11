using UnityEngine;

namespace CucuTools
{
    public class SerializableMeshFilter : SerializableComponent<MeshFilter, SerializedMesh>
    {
        public override SerializedMesh ReadComponent()
        {
            return new SerializedMesh(Target.mesh);
        }

        public override bool WriteComponent(SerializedMesh serialized)
        {
            Target.mesh = serialized.Create();
            return true;
        }
    }
}