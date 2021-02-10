using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc cref="LerpBehavior" />
    [AddComponentMenu(LerpMenuRoot + nameof(LerpComplex))]
    public class LerpComplex : LerpBehavior, IList<LerpBehavior>
    {
        public List<LerpBehavior> Elements => elements ?? (elements = new List<LerpBehavior>());
        
        [Header("Elements")]
        [SerializeField] private List<LerpBehavior> elements;

        /// <inheritdoc />
        protected override bool UpdateBehaviour()
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
        
        public IEnumerator<LerpBehavior> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        public void Add(LerpBehavior item)
        {
            Elements.Add(item);
        }

        public void Clear()
        {
            Elements.Clear();
        }

        public bool Contains(LerpBehavior item)
        {
            return Elements.Contains(item);
        }

        public void CopyTo(LerpBehavior[] array, int arrayIndex)
        {
            Elements.CopyTo(array, arrayIndex);
        }    

        public bool Remove(LerpBehavior item)
        {
            return Elements.Remove(item);
        }
        
        public int IndexOf(LerpBehavior item)
        {
            return Elements.IndexOf(item);
        }

        public void Insert(int index, LerpBehavior item)
        {
            Elements.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Elements.RemoveAt(index);
        }

        public LerpBehavior this[int index]
        {
            get => Elements[index];
            set => Elements[index] = value;
        }

        #endregion
    }
}