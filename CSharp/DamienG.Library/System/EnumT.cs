// Copyright (c) Damien Guard.  All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Originally published at http://damieng.com/blog/2010/10/17/enums-better-syntax-improved-performance-and-tryparse-in-net-3-5

using System;
using System.Collections.Generic;
using System.Linq;

namespace DamienG.System
{
    /// <summary>
    /// Strongly typed version of Enum with Parsing and performance improvements.
    /// </summary>
    /// <typeparam name="T">Type of Enum</typeparam>
    public static class Enum<T> where T : struct, IConvertible
    {
        static readonly IEnumerable<T> all = Enum.GetValues(typeof (T)).Cast<T>();
        static readonly Dictionary<string, T> insensitiveNames = all.ToDictionary(k => Enum.GetName(typeof (T), k).ToUpperInvariant());
        static readonly Dictionary<string, T> sensitiveNames = all.ToDictionary(k => Enum.GetName(typeof (T), k));
        static readonly Dictionary<int, T> values = all.ToDictionary(k => Convert.ToInt32(k));
        static readonly Dictionary<T, string> names = all.ToDictionary(k => k, v => v.ToString());

        public static bool IsDefined(T value) => names.Keys.Contains(value);

        public static bool IsDefined(string value) => sensitiveNames.Keys.Contains(value);

        public static bool IsDefined(int value) => values.Keys.Contains(value);

        public static IEnumerable<T> GetValues() => all;

        public static string[] GetNames() => names.Values.ToArray();

        public static string GetName(T value)
        {
            names.TryGetValue(value, out string name);
            return name;
        }

        public static T Parse(string value)
        {
            if (!sensitiveNames.TryGetValue(value, out T parsed))
                throw new ArgumentException("Value is not one of the named constants defined for the enumeration", nameof(value));
            return parsed;
        }

        public static T Parse(string value, bool ignoreCase)
        {
            if (!ignoreCase)
                return Parse(value);

            if (!insensitiveNames.TryGetValue(value.ToUpperInvariant(), out T parsed))
                throw new ArgumentException("Value is not one of the named constants defined for the enumeration", nameof(value));
            return parsed;
        }

        public static bool TryParse(string value, out T returnValue) => sensitiveNames.TryGetValue(value, out returnValue);

        public static bool TryParse(string value, bool ignoreCase, out T returnValue)
        {
            return ignoreCase
                       ? insensitiveNames.TryGetValue(value.ToUpperInvariant(), out returnValue)
                       : TryParse(value, out returnValue);
        }

        public static T? ParseOrNull(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            if (sensitiveNames.TryGetValue(value, out T foundValue))
                return foundValue;

            return null;
        }

        public static T? ParseOrNull(string value, bool ignoreCase)
        {
            if (!ignoreCase)
                return ParseOrNull(value);

            if (string.IsNullOrEmpty(value))
                return null;

            if (insensitiveNames.TryGetValue(value.ToUpperInvariant(), out T foundValue))
                return foundValue;

            return null;
        }

        public static T? CastOrNull(int value)
        {
            if (values.TryGetValue(value, out T foundValue))
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
            return values.TryGetValue(combined, out T result) ? result : default;
        }
    }
}