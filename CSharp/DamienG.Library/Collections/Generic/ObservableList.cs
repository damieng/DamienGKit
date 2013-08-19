﻿// Copyright (c) Damien Guard.  All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Originally published at http://damieng.com/blog/2006/06/14/observing_change_events_on_a_listt

using System;
using System.Collections;
using System.Collections.Generic;

namespace DamienG.Collections.Generic
{
    /// <summary>
    /// A list that can be observed for change or clear events.
    /// </summary>
    /// <typeparam name="T">Type of items in the list.</typeparam>
    public class ObservableList<T> : IList<T>
    {
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
            return ((IEnumerable)internalList).GetEnumerator();
        }

        public event EventHandler<ListChangedEventArgs> ListChanged;
        public event EventHandler ListCleared;

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