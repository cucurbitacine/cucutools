using System;
using UnityEngine;

namespace CucuTools
{
    public class SerializableMeshRenderer : SerializableComponent<MeshRenderer, SerializedMeshRenderer>
    {
        [SerializeField] private Color color;
        
        public override SerializedMeshRenderer ReadComponent()
        {
            if (Application.isPlaying)
            {
                //color = Target.material.color;
            }
            
            return new SerializedMeshRenderer(Target);
        }

        public override bool WriteComponent(SerializedMeshRenderer serialized)
        {
            color = serialized.colorHexs[0].ToColor();
            
            if (Application.isPlaying)
            {
                //Target.material.color = color;
            }
            
            return true;
        }
        
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (Target == null) return;

            var tr = Target.transform;

            var sharedMesh = tr.GetComponent<MeshFilter>()?.sharedMesh;
            
            if (sharedMesh == null) return;
            
            Gizmos.color = color;
            Gizmos.DrawWireMesh(sharedMesh, tr.position, tr.rotation, tr.lossyScale);
        }
    }

    [Serializable]
    public class SerializedMeshRenderer : SerializableData
    {
        public string[] colorHexs;
        public string[] materialNames;

        public SerializedMeshRenderer(MeshRenderer meshRenderer)
        {
            var materials = meshRenderer.sharedMaterials;
            
            colorHexs = new string[materials.Length];
            materialNames = new string[materials.Length];
            
            for (int i = 0; i < materials.Length; i++)
            {
                colorHexs[i] = materials[i].color.ToHex();
                materialNames[i] = materials[i].name;
            }
        }
    }
}