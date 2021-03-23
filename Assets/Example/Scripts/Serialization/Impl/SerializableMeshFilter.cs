using System;
using CucuTools.Serializing.Components;
using CucuTools.Serializing.Datas;
using UnityEngine;

namespace Example.Scripts.Serialization.Impl
{
    public class SerializableMeshFilter : JsonSerializableComponent<MeshFilter, SerializedMesh>
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
    
    [Serializable]
    public class SerializedMesh : SerializedData
    {
        public string name;
        public Vector3[] vertices;
        public Vector3[] normals;
        public int[] triangles;
        public Vector2[] uv;
        public Vector4[] tangents;

        #region SerializedData

        public SerializedMesh()
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
        
        public SerializedMesh(Mesh mesh)
        {
            name = mesh.name;
            vertices = mesh.vertices;
            normals = mesh.normals;
            triangles = mesh.triangles;
            uv = mesh.uv;
            tangents = mesh.tangents;
        }

        public Mesh Create()
        {
            var mesh = new Mesh
            {
                name = name,
                vertices = vertices,
                normals = normals,
                triangles = triangles,
                uv = uv,
                tangents = tangents
            };
            
            return mesh;
        }
    }
}