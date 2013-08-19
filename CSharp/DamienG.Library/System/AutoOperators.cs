using System;

namespace DamienG.System
{
    /// <summary>
    /// A base class that automatically provides all operator overloads based on your class
    /// only implementing CompareTo.
    /// </summary>
    public abstract class AutoOperators : IComparable
    {
        public abstract int CompareTo(object obj);

        public static bool operator <(AutoOperators obj1, AutoOperators obj2)
        {
            return Compare(obj1, obj2) < 0;
        }

        public static bool operator >(AutoOperators obj1, AutoOperators obj2)
        {
            return Compare(obj1, obj2) > 0;
        }

        public static bool operator ==(AutoOperators obj1, AutoOperators obj2)
        {
            return Compare(obj1, obj2) == 0;
        }

        public static bool operator !=(AutoOperators obj1, AutoOperators obj2)
        {
            return Compare(obj1, obj2) != 0;
        }

        public static bool operator <=(AutoOperators obj1, AutoOperators obj2)
        {
            return Compare(obj1, obj2) <= 0;
        }

        public static bool operator >=(AutoOperators obj1, AutoOperators obj2)
        {
            return Compare(obj1, obj2) >= 0;
        }

        public static int Compare(AutoOperators obj1, AutoOperators obj2)
        {
            if (ReferenceEquals(obj1, obj2))
                return 0;
            if ((object) obj1 == null)
                return -1;
            if ((object) obj2 == null)
                return 1;
            return obj1.CompareTo(obj2);
        }

        public abstract override int GetHashCode();

        public override bool Equals(object obj)
        {
            if (!(obj is AutoOperators))
                return false;
            return this == (AutoOperators) obj;
        }
    }
}