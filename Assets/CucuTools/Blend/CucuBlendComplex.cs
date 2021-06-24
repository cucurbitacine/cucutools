using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.Blend
{
    public class CucuBlendComplex : CucuBlend, IList<CucuBlend>
    {
        public List<CucuBlend> Blends => blends ?? (blends = new List<CucuBlend>());

        [Header("Blends")]
        [SerializeField] private List<CucuBlend> blends;

        public override void OnBlendChange()
        {
            base.OnBlendChange();

            foreach (var cucuBlend in Blends)
                if (cucuBlend != null)
                    cucuBlend.Blend = Blend;
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            Blends.RemoveAll(b => b == this);
        }

        #region IList<CucuBlend>

        public int Count => Blends.Count;
        public bool IsReadOnly => false;

        public IEnumerator<CucuBlend> GetEnumerator()
        {
            return Blends.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(CucuBlend item)
        {
            Blends.Add(item);
        }

        public void Clear()
        {
            Blends.Clear();
        }

        public bool Contains(CucuBlend item)
        {
            return Blends.Contains(item);
        }

        public void CopyTo(CucuBlend[] array, int arrayIndex)
        {
            Blends.CopyTo(array, arrayIndex);
        }

        public bool Remove(CucuBlend item)
        {
            return Blends.Remove(item);
        }

        public int IndexOf(CucuBlend item)
        {
            return Blends.IndexOf(item);
        }

        public void Insert(int index, CucuBlend item)
        {
            Blends.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Blends.RemoveAt(index);
        }

        public CucuBlend this[int index]
        {
            get => Blends[index];
            set => Blends[index] = value;
        }

        #endregion
    }
}