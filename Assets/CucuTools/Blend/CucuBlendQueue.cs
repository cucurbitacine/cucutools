using System;
using System.Linq;
using UnityEngine;

namespace CucuTools.Blend
{
    public class CucuBlendQueue : CucuBlend
    {
        public float LocalBlend
        {
            get => localBlend;
            private set => localBlend = value;
        }

        [Header("Queue")]
        [Range(0f, 1f)]
        [SerializeField] private float localBlend;
        
        [SerializeField] private BlendUnit[] units;
        
        private float leftBlend;
        private float rightBlend;
        private int left;
        private int right;

        public override void OnBlendChange()
        {
            base.OnBlendChange();

            Cucu.IndexesOfBorder(out left, out right, Blend, units);

            if (0 <= left && left < units.Length) leftBlend = units[left].blend;
            else leftBlend = 0f;
            if (0 <= right && right < units.Length) rightBlend = units[right].blend;
            else rightBlend = 1f;

            if (Math.Abs(leftBlend - rightBlend) < 0.001f) LocalBlend = 0f;
            else LocalBlend = (Blend - leftBlend) / (rightBlend - leftBlend);

            if (left >= 0)
                if (units[left].behaviour != null)
                    units[left].behaviour.Blend = LocalBlend;

            for (int i = 0; i < units.Length; i++)
            {
                if (units[i].behaviour != null)
                {
                    if (i < left) units[i].behaviour.Blend = 1f;
                    else if (left < i) units[i].behaviour.Blend = 0f;
                }
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            units = units.Where(u => u.behaviour != this).ToArray();
        }

        [Serializable]
        public class BlendUnit : IComparable<float>
        {
            [Range(0f, 1f)]
            public float blend;
            public CucuBlend behaviour;

            public int CompareTo(float other)
            {
                return blend.CompareTo(other);
            }
        }
    }
}