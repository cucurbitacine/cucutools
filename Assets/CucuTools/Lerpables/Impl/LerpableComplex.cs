using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc cref="LerpableEntity" />
    [AddComponentMenu(LerpMenuRoot + nameof(LerpableComplex))]
    public class LerpableComplex : LerpableEntity, IList<LerpableEntity>
    {
        public List<LerpableEntity> Elements => elements ?? (elements = new List<LerpableEntity>());
        
        [Header("Elements")]
        [SerializeField] private List<LerpableEntity> elements;

        /// <inheritdoc />
        protected override bool UpdateEntityInternal()
        {
            if (Elements == null) return false;
            if (Elements.Count == 0) return false;

            foreach (var element in Elements)
            {
                element?.Lerp(LerpValue);
            }

            return true;
        }

        #region IList<LerpableEntity>

        public int Count => Elements?.Count ?? 0;
        public bool IsReadOnly => false;
        
        public IEnumerator<LerpableEntity> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        public void Add(LerpableEntity item)
        {
            Elements.Add(item);
        }

        public void Clear()
        {
            Elements.Clear();
        }

        public bool Contains(LerpableEntity item)
        {
            return Elements.Contains(item);
        }

        public void CopyTo(LerpableEntity[] array, int arrayIndex)
        {
            Elements.CopyTo(array, arrayIndex);
        }    

        public bool Remove(LerpableEntity item)
        {
            return Elements.Remove(item);
        }
        
        public int IndexOf(LerpableEntity item)
        {
            return Elements.IndexOf(item);
        }

        public void Insert(int index, LerpableEntity item)
        {
            Elements.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Elements.RemoveAt(index);
        }

        public LerpableEntity this[int index]
        {
            get => Elements[index];
            set => Elements[index] = value;
        }

        #endregion
    }
}