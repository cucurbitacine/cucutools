using System;
using UnityEngine;

namespace CucuTools.Voxels
{
    [Serializable]
    public class Voxel
    {
        public int x => point.x;
        public int y => point.y;
        public int z => point.z;
        
        public int id;
        public Point point;
        
        public Voxel(int id, int x, int y, int z)
        {
            this.id = id;
            this.point.x = x;
            this.point.y = y;
            this.point.z = z;
        }

        public Voxel(int id, Point p) : this(id, p.x, p.y, p.z)
        {
        }

        public Voxel(int id) : this(id, Point.zero)
        {
        }
        
        public static void BuildPlane(Vector3 pos, Vector3 frd, Vector3 up, Vector2 size,
            ref int iVert, ref Vector3[] vert, ref Vector3[] norm, ref Vector2[] uv,
            ref int iTris, ref int[] tris)
        {
            frd = frd.normalized;
            up = up.normalized;

            var right = Vector3.Cross(up, frd);

            vert[iVert + 0] = pos + (-up * size.y - right * size.x) / 2;
            vert[iVert + 1] = pos + ( up * size.y - right * size.x) / 2;
            vert[iVert + 2] = pos + ( up * size.y + right * size.x) / 2;
            vert[iVert + 3] = pos + (-up * size.y + right * size.x) / 2;

            norm[iVert + 0] = frd;
            norm[iVert + 1] = frd;
            norm[iVert + 2] = frd;
            norm[iVert + 3] = frd;

            uv[iVert + 0] = Vector2.zero;
            uv[iVert + 1] = Vector2.up;
            uv[iVert + 2] = Vector2.one;
            uv[iVert + 3] = Vector2.right;

            tris[iTris + 0] = iVert + 0;
            tris[iTris + 1] = iVert + 2;
            tris[iTris + 2] = iVert + 1;

            tris[iTris + 3] = iVert + 0;
            tris[iTris + 4] = iVert + 3;
            tris[iTris + 5] = iVert + 2;
            
            iVert += 4;
            iTris += 6;
        }
    }
}
