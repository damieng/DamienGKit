using System;
using System.Collections.Generic;
using System.Linq;

namespace DamienG.System
{
    /// <summary>
    /// Strongly typed version of Enum with Parsing and performance improvements.
    /// </summary>
    /// <typeparam name="T">Type of Enum</typeparam>
    public static class Enum<T> where T : struct
    {
        private static readonly IEnumerable<T> all = Enum.GetValues(typeof (T)).Cast<T>();
        private static readonly Dictionary<string, T> insensitiveNames = all.ToDictionary(k => Enum.GetName(typeof (T), k).ToUpperInvariant());
        private static readonly Dictionary<string, T> sensitiveNames = all.ToDictionary(k => Enum.GetName(typeof (T), k));
        private static readonly Dictionary<int, T> values = all.ToDictionary(k => Convert.ToInt32(k));
        private static readonly Dictionary<T, string> names = all.ToDictionary(k => k, v => v.ToString());

        public static bool IsDefined(T value)
        {
            return names.Keys.Contains(value);
        }

        public static bool IsDefined(string value)
        {
            return sensitiveNames.Keys.Contains(value);
        }

        public static bool IsDefined(int value)
        {
            return values.Keys.Contains(value);
        }

        public static IEnumerable<T> GetValues()
        {
            return all;
        }

        public static string[] GetNames()
        {
            return names.Values.ToArray();
        }

        public static string GetName(T value)
        {
            string name;
            names.TryGetValue(value, out name);
            return name;
        }

        public static T Parse(string value)
        {
            T parsed;
            if (!sensitiveNames.TryGetValue(value, out parsed))
                throw new ArgumentException("Value is not one of the named constants defined for the enumeration", "value");
            return parsed;
        }

        public static T Parse(string value, bool ignoreCase)
        {
            if (!ignoreCase)
                return Parse(value);

            T parsed;
            if (!insensitiveNames.TryGetValue(value.ToUpperInvariant(), out parsed))
                throw new ArgumentException("Value is not one of the named constants defined for the enumeration", "value");
            return parsed;
        }

        public static bool TryParse(string value, out T returnValue)
        {
            return sensitiveNames.TryGetValue(value, out returnValue);
        }

        public static bool TryParse(string value, bool ignoreCase, out T returnValue)
        {
            return ignoreCase
                       ? insensitiveNames.TryGetValue(value.ToUpperInvariant(), out returnValue)
                       : TryParse(value, out returnValue);
        }

        public static T? ParseOrNull(string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;

            T foundValue;
            if (sensitiveNames.TryGetValue(value, out foundValue))
                return foundValue;

            return null;
        }

        public static T? ParseOrNull(string value, bool ignoreCase)
        {
            if (!ignoreCase)
                return ParseOrNull(value);

            if (String.IsNullOrEmpty(value))
                return null;

            T foundValue;
            if (insensitiveNames.TryGetValue(value.ToUpperInvariant(), out foundValue))
                return foundValue;

            return null;
        }

        public static T? CastOrNull(int value)
        {
            T foundValue;
            if (values.TryGetValue(value, out foundValue))
                return foundValue;

            return null;
        }

        public static IEnumerable<T> GetFlags(T flagEnum)
        {
            var flagInt = Convert.ToInt32(flagEnum);
            return all.Where(e => (Convert.ToInt32(e) & flagInt) != 0);
        }

        public static T SetFlags(IEnumerable<T> flags)
        {
            var combined = flags.Aggregate(default(int), (current, flag) => current | Convert.ToInt32(flag));

            T result;
            return values.TryGetValue(combined, out result) ? result : default(T);
        }
    }
}