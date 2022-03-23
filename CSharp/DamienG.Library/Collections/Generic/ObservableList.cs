// Copyright (c) Damien Guard.  All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Originally published at http://damieng.com/blog/2006/06/14/observing_change_events_on_a_listt

using System;
using System.Collections;
using System.Collections.Generic;

namespace DamienG.Collections.Generic
{
    public class ObservableList<T> : IList<T>
    {
        readonly object _lock = new object();
        readonly IList<T> internalList;

        public ObservableList()
        {
            internalList = new List<T>();
        }

        public ObservableList(IList<T> list)
        {
            internalList = list ?? throw new ArgumentNullException(nameof(list));
        }

        public ObservableList(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

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
            internalList.RemoveAt(index);
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
            lock (_lock)
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
            return ((IEnumerable)internalList).GetEnumerator();
        }

        public event EventHandler<ListChangedEventArgs> ListChanged = delegate { };
        public event EventHandler ListCleared = delegate { };

        protected virtual void OnListChanged(ListChangedEventArgs e)
        {
            ListChanged(this, e);
        }

        protected virtual void OnListCleared(EventArgs e)
        {
            ListCleared(this, e);
        }

        public class ListChangedEventArgs : EventArgs
        {
            internal ListChangedEventArgs(int index, T item)
            {                
                Index = index;
                Item = item;
            }

            public int Index { get; }

            public T Item { get; }
        }
    }
}