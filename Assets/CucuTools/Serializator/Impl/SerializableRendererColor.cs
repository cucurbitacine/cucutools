using System;
using UnityEngine;

namespace CucuTools
{
    public class SerializableRendererColor : SerializableComponent<Renderer, SerializedColor>
    {
        [SerializeField] private Color color;
        
        public override SerializedColor ReadComponent()
        {
            if (Application.isPlaying)
            {
                //color = Target.material.color;
            }
            
            return new SerializedColor(color);
        }

        public override bool WriteComponent(SerializedColor serialized)
        {
            color = serialized.colorHex.ToColor();
            
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
    public class SerializedColor : SerializableData
    {
        public string colorHex;

        public SerializedColor(Color color)
        {
            colorHex = color.ToHex();
        }
    }
}