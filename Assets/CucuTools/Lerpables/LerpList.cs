using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CucuTools.Lerpables
{
    public abstract class LerpList<TResult, TElement> : LerpBehavior<TResult>, IList<LerpPoint<TElement>>
    {
        public abstract List<LerpPoint<TElement>> Elements { get; protected set; }

        public List<LerpPoint<TElement>> SortedElements => _sortedElements ?? (_sortedElements = Elements?.OrderBy(p => p).ToList());

        private List<LerpPoint<TElement>> _sortedElements;
        
        public void Add(float t, TElement element)
        {
            Add(new LerpPoint<TElement>(t, element));
        }

        public bool Remove(TElement element)
        {
            var remove = Elements.FirstOrDefault(f => f.Value.Equals(element));

            return remove != null && Remove(remove);
        }
        
        public void Sort()
        {
            if (Elements == null) return;

            Elements = Elements.OrderBy(p => p).ToList();
        }
        
        #region IList<LerpPoint<TElement>>

        public int Count => Elements.Count;

        public bool IsReadOnly => false;
        
        public IEnumerator<LerpPoint<TElement>> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        public void Add(LerpPoint<TElement> item)
        {
            Elements.Add(item);
        }

        public void Clear()
        {
            Elements.Clear();
        }

        public bool Contains(LerpPoint<TElement> item)
        {
            return Elements.Contains(item);
        }

        public void CopyTo(LerpPoint<TElement>[] array, int arrayIndex)
        {
            Elements.CopyTo(array, arrayIndex);
        }

        public bool Remove(LerpPoint<TElement> item)
        {
            return Elements.Remove(item);
        }

        public int IndexOf(LerpPoint<TElement> item)
        {
            return Elements.IndexOf(item);
        }

        public void Insert(int index, LerpPoint<TElement> item)
        {
            Elements.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Elements.RemoveAt(index);
        }

        public LerpPoint<TElement> this[int index]
        {
            get => Elements[index];
            set => Elements[index] = value;
        }

        #endregion

        private void Awake()
        {
            _sortedElements = null;
        }
    }

    public abstract class LerpableList<TElement> : LerpList<TElement, TElement>
    {
    }
    
}