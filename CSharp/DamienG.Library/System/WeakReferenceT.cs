using System;
using System.Runtime.InteropServices;

namespace DamienG.System
{
    public sealed class EquatableWeakReference<T> : IEquatable<EquatableWeakReference<T>>, IDisposable where T : class
    {
        private readonly int hashCode;
        private GCHandle handle;

        public EquatableWeakReference(T target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
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

        public override bool Equals(object other)
        {
            if (other is EquatableWeakReference<T>)
                return Equals((EquatableWeakReference<T>) other);

            return false;
        }

        public override int GetHashCode()
        {
            return hashCode;
        }
    }
}