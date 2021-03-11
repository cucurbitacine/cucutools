using System;
using UnityEngine;

namespace CucuTools
{
    [Serializable]
    public class SerializedMesh : SerializableData
    {
        public string name;
        public Vector3[] vertices;
        public Vector3[] normals;
        public int[] triangles;
        public Vector2[] uv;
        public Vector4[] tangents;
        
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