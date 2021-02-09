using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc cref="LerpableBehavior" />
    [AddComponentMenu(LerpMenuRoot + nameof(LerpableComplex))]
    public class LerpableComplex : LerpableBehavior, IList<LerpableBehavior>
    {
        public List<LerpableBehavior> Elements => elements ?? (elements = new List<LerpableBehavior>());
        
        [Header("Elements")]
        [SerializeField] private List<LerpableBehavior> elements;

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
        
        public IEnumerator<LerpableBehavior> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        public void Add(LerpableBehavior item)
        {
            Elements.Add(item);
        }

        public void Clear()
        {
            Elements.Clear();
        }

        public bool Contains(LerpableBehavior item)
        {
            return Elements.Contains(item);
        }

        public void CopyTo(LerpableBehavior[] array, int arrayIndex)
        {
            Elements.CopyTo(array, arrayIndex);
        }    

        public bool Remove(LerpableBehavior item)
        {
            return Elements.Remove(item);
        }
        
        public int IndexOf(LerpableBehavior item)
        {
            return Elements.IndexOf(item);
        }

        public void Insert(int index, LerpableBehavior item)
        {
            Elements.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Elements.RemoveAt(index);
        }

        public LerpableBehavior this[int index]
        {
            get => Elements[index];
            set => Elements[index] = value;
        }

        #endregion
    }
}