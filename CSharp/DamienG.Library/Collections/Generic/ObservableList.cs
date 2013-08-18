using System;
using System.Collections;
using System.Collections.Generic;

namespace DamienG.Collections.Generic
{
    public class ObservableList<T> : IList<T>
    {
        public delegate void ListChangedEventHandler(object sender, ListChangedEventArgs e);

        public delegate void ListClearedEventHandler(object sender, EventArgs e);

        private readonly IList<T> internalList;

        public ObservableList()
        {
            internalList = new List<T>();
        }

        public ObservableList(IList<T> list)
        {
            internalList = list;
        }

        public ObservableList(IEnumerable<T> collection)
        {
            internalList = new List<T>(collection);
        }

        public int IndexOf(T item)
        {
            return internalList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            internalList.Insert(index, item);
            OnListChanged(new ListChangedEventArgs(index, item));
        }

        public void RemoveAt(int index)
        {
            var item = internalList[index];
            internalList.Remove(item);
            OnListChanged(new ListChangedEventArgs(index, item));
        }

        public T this[int index]
        {
            get { return internalList[index]; }
            set
            {
                if (internalList[index].Equals(value)) return;
                
                internalList[index] = value;
                OnListChanged(new ListChangedEventArgs(index, value));
            }
        }

        public void Add(T item)
        {
            internalList.Add(item);
            OnListChanged(new ListChangedEventArgs(internalList.IndexOf(item), item));
        }

        public void Clear()
        {
            internalList.Clear();
            OnListCleared(new EventArgs());
        }

        public bool Contains(T item)
        {
            return internalList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            internalList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return internalList.Count; }
        }

        public bool IsReadOnly
        {
            get { return internalList.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            lock (this)
            {
                var index = internalList.IndexOf(item);
                if (internalList.Remove(item))
                {
                    OnListChanged(new ListChangedEventArgs(index, item));
                    return true;
                }
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) internalList).GetEnumerator();
        }

        public event ListChangedEventHandler ListChanged;
        public event ListClearedEventHandler ListCleared;

        protected virtual void OnListChanged(ListChangedEventArgs e)
        {
            if (ListChanged != null)
                ListChanged(this, e);
        }

        protected virtual void OnListCleared(EventArgs e)
        {
            if (ListCleared != null)
                ListCleared(this, e);
        }

        public class ListChangedEventArgs : EventArgs
        {
            private readonly int index;
            private readonly T item;

            public ListChangedEventArgs(int index, T item)
            {
                this.index = index;
                this.item = item;
            }

            public int Index
            {
                get { return index; }
            }

            public T Item
            {
                get { return item; }
            }
        }
    }
}