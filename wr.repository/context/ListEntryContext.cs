using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace wr.repository.context
{
    public class ListEntryContext<T> : IList<T>
        where T: class
    {
        #region properties
        private List<EntryContext<T>> _entries = new List<EntryContext<T>>();
        #endregion

        #region  constructor

        public ListEntryContext()
        {
        }

        public ListEntryContext(List<EntryContext<T>> entries)
        {
            if (entries == null)
            {
                throw  new ArgumentNullException();
            }

            _entries = entries;
        }
        #endregion
        
        #region implement IList<T> 
        public IEnumerator<T> GetEnumerator()
        {
             return _entries.Select(x => x.Item).ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            _entries.Add(item);
        }

        public void Clear()
        {
            _entries.Clear();
        }

        public bool Contains(T item)
        {
            return  _entries.Exists( i => i.Item == item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _entries.Select(x => x.Item).ToList().CopyTo(array,arrayIndex);
        }

        public bool Remove(T item)
        {
            return _entries.RemoveAll(x => x.Item == item) > 0;
        }

        public int Count { get => _entries.Count; }
        public bool IsReadOnly { get => false; }
        
        
        public int IndexOf(T item)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(int index, T item)
        {
           _entries.Insert(index,item);
        }

        public void RemoveAt(int index)
        {
            _entries.RemoveAt(index);
        }

        public T this[int index]
        {
            get => _entries[index];

            set { _entries[index] = value; }
        }

        #endregion
    }
}