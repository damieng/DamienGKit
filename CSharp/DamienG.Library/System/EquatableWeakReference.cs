// Copyright (c) Damien Guard.  All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Originally published at http://damieng.com/blog/2006/08/28/equatable_weak_references

using System;
using System.Runtime.InteropServices;

namespace DamienG.System
{
    /// <summary>
    /// Strongly-typed version of WeakReference that supports IEquatable too.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class EquatableWeakReference<T> : IEquatable<EquatableWeakReference<T>>, IDisposable where T : class
    {
        readonly int hashCode;
        GCHandle handle;

        public EquatableWeakReference(T target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            hashCode = target.GetHashCode();
            handle = GCHandle.Alloc(target, GCHandleType.Weak);
        }

        public bool IsAlive
        {
            get { return (handle.Target != null); }
        }

        public T Target
        {
            get { return handle.Target as T; }
        }

        public void Dispose()
        {
            handle.Free();
            GC.SuppressFinalize(this);
        }

        public bool Equals(EquatableWeakReference<T> other)
        {
            return ReferenceEquals(other.Target, Target);
        }

        ~EquatableWeakReference()
        {
            Dispose();
        }

        public override bool Equals(object obj)
        {
            if (obj is EquatableWeakReference<T>)
                return Equals((EquatableWeakReference<T>) obj);

            return false;
        }

        public override int GetHashCode()
        {
            return hashCode;
        }
    }
}