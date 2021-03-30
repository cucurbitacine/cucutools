using System;
using UnityEngine;

namespace CucuTools.Voxels
{
    [Serializable]
    public struct Point
    {
        public static Point zero => zeroPoint;
        private static readonly Point zeroPoint = new Point(0, 0, 0);
        
        public int x;
        public int y;
        public int z;

        public Point(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator Vector3(Point p) => new Vector3(p.x, p.y, p.z);
        public static explicit operator Point(Vector3 v) => new Point((int) v.x, (int) v.y, (int) v.z);
    }
}