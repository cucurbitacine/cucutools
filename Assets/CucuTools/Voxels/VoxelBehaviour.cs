using System.Collections.Generic;
using CucuTools.Attributes;
using UnityEngine;

namespace CucuTools.Voxels
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class VoxelBehaviour : MonoBehaviour
    {
        public Chunk Chunk => _chunk ?? (_chunk = new Chunk(0, Point.zero, 10, 1f));
        [SerializeField] private Chunk _chunk;

        protected MeshFilter filter => _filter ?? (_filter = GetComponent<MeshFilter>());
        protected MeshRenderer renderer => _renderer ?? (_renderer = GetComponent<MeshRenderer>());

        [SerializeField] private MeshFilter _filter;
        [SerializeField] private MeshRenderer _renderer;
        
        [CucuButton()]
        public void Show()
        {
            Clear();

            for (int i = 0; i < 32; i++)
            {
                var x = Random.Range(0, Chunk.resolution);
                var y = Random.Range(0, Chunk.resolution);
                var z = Random.Range(0, Chunk.resolution);
                
                Chunk[x, y, z] = new Voxel(0);
            }
            
            UpdateVoxel();
        }
        
        public void UpdateVoxel()
        {
            if (_filter == null) _filter = GetComponent<MeshFilter>();
            var mesh = _filter.mesh;

            if (mesh == null) mesh = new Mesh();
            mesh.name = gameObject.name;
            
            //

            var voxels = new List<Voxel>();

            for (var x = 0; x < Chunk.resolution; x++)
            {
                for (var y = 0; y < Chunk.resolution; y++)
                {
                    for (var z = 0; z < Chunk.resolution; z++)
                    {
                        if (Chunk[x, y, z] != null) voxels.Add(Chunk[x, y, z]);
                    }
                }
            }

            var vertPerVoxel = 4 * 6; // 4 vert on 6 sides
            var trisPerVoxel = 2 * 6; // 2 tris on 6 sides

            var vertices = new Vector3[voxels.Count * vertPerVoxel];
            var normals = new Vector3[vertices.Length];
            var uv = new Vector2[vertices.Length];
            var triangles = new int[voxels.Count * trisPerVoxel * 3];

            var indVert = 0;
            var indTris = 0;
            for (var ind = 0; ind < voxels.Count; ind++)
            {
                var voxel = voxels[ind];
                
                var size = Chunk.sizeVoxel;
                var center = (Vector3)voxel.point * size + Vector3.one * size / 2;

                Voxel.BuildPlane(center + Vector3.forward * size / 2, Vector3.forward, Vector3.up, Vector2.one * size,
                    ref indVert, ref vertices, ref normals, ref uv,
                    ref indTris, ref triangles);

                Voxel.BuildPlane(center + Vector3.back * size / 2, Vector3.back, Vector3.up, Vector2.one * size,
                    ref indVert, ref vertices, ref normals, ref uv,
                    ref indTris, ref triangles);
                
                Voxel.BuildPlane(center + Vector3.right * size / 2, Vector3.right, Vector3.up, Vector2.one * size,
                    ref indVert, ref vertices, ref normals, ref uv,
                    ref indTris, ref triangles);
                
                Voxel.BuildPlane(center + Vector3.left * size / 2, Vector3.left, Vector3.up, Vector2.one * size,
                    ref indVert, ref vertices, ref normals, ref uv,
                    ref indTris, ref triangles);
                
                Voxel.BuildPlane(center + Vector3.up * size / 2, Vector3.up, Vector3.forward, Vector2.one * size,
                    ref indVert, ref vertices, ref normals, ref uv,
                    ref indTris, ref triangles);
                
                Voxel.BuildPlane(center + Vector3.down * size / 2, Vector3.down, Vector3.forward, Vector2.one * size,
                    ref indVert, ref vertices, ref normals, ref uv,
                    ref indTris, ref triangles);
            }
            
            //
            
            mesh.Clear();
            
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uv;
            mesh.triangles = triangles;

            filter.mesh = mesh;

            var collider = GetComponent<MeshCollider>();

            if (collider != null)
            {
                collider.sharedMesh = mesh;
            }
        }

        [CucuButton()]
        public void Clear()
        {
            Chunk.Clear();
        }
        
        [CucuButton()]
        public void Divide()
        {
            var voxels = new List<Voxel>();
            
            for (var x = 0; x < Chunk.resolution; x++)
            {
                for (var y = 0; y < Chunk.resolution; y++)
                {
                    for (var z = 0; z < Chunk.resolution; z++)
                    {
                        if (Chunk[x, y, z] != null)
                        {
                            if (x > Chunk.resolution / 2)
                            {
                                voxels.Add(Chunk[x, y, z]);
                                Chunk[x, y, z] = null;
                            }
                        }
                    }
                }
            }

            var divided = Instantiate(gameObject);
            var voxel = divided.GetComponent<VoxelBehaviour>();
            voxel.Clear();
            
            foreach (var v in voxels)
            {
                voxel.Chunk[v.x, v.y, v.z] = v;
            }
            
            voxel.UpdateVoxel();
            
            
            UpdateVoxel();
        }
    }
}