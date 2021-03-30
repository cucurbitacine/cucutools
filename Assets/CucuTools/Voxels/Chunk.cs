using System;
using UnityEngine;

namespace CucuTools.Voxels
{
    [Serializable]
    public class Chunk
    {
        public int id;
        public Point point;
        public int resolution;
        public float sizeChunk;
        public float sizeVoxel => sizeChunk / resolution;

        protected Voxel[,,] voxels => _voxels ?? (_voxels = new Voxel[resolution, resolution, resolution]);
        
        private Voxel[,,] _voxels;

        public Chunk(int id, Point point, int resolution, float sizeChunk)
        {
            this.id = id;
            this.point.x = point.x;
            this.point.y = point.y;
            this.point.z = point.z;
            this.resolution = resolution;
            this.sizeChunk = sizeChunk;
        }

        public Chunk(int id, int x, int y, int z, int res, float size) : this(id, new Point(x, y, z), res, size)
        {
        }

        public Voxel this[int x, int y, int z]
        {
            get
            {
                if (x < 0 || resolution <= x) return null;
                if (y < 0 || resolution <= y) return null;
                if (z < 0 || resolution <= z) return null;
                
                return voxels[x, y, z];
            }
            set
            {
                if (x < 0 || resolution <= x) return;
                if (y < 0 || resolution <= y) return;
                if (z < 0 || resolution <= z) return;

                if (value != null)
                {
                    value.point.x = x;
                    value.point.y = y;
                    value.point.z = z;
                }
                
                voxels[x, y, z] = value;
            }
        }

        public void Clear()
        {
            for (var x = 0; x < resolution; x++)
            {
                for (var y = 0; y < resolution; y++)
                {
                    for (var z = 0; z < resolution; z++)
                    {
                        this[x, y, z] = null;
                    }
                }
            }

            _voxels = null;
        }
    }
}