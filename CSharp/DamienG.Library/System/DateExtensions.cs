// Copyright (c) Damien Guard.  All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Originally published at http://damieng.com/blog/2006/10/20/dotnettipsuptimeagerounding

using System;

namespace DamienG.System
{
    /// <summary>
    /// Various extension for Date related classes.
    /// </summary>
    public static class DateExtensions
    {
        /// <summary>
        /// Calculate the age for somebody given their birthday.
        /// </summary>
        /// <param name="birthDate">Date of birth.</param>
        /// <returns>Age in years.</returns>
        /// <remarks>As always time zones can mean this is off if today is their birthday.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">If not yet born.</exception>
        public static int GetAge(this DateTime birthDate)
        {
            return GetAge(birthDate, DateTime.Now);
        }

        /// <summary>
        /// Calculate the age for somebody given their birthday from a specific date.
        /// </summary>
        /// <param name="birthDate">Date of birth.</param>
        /// <param name="at">When to consider from - often <see cref="DateTime.Now"/></param>
        /// <returns>Age in years.</returns>
        /// <remarks>As always time zones can mean this is off if today is their birthday.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">If not yet born.</exception>
        public static int GetAge(this DateTime birthDate, DateTime at)
        {
            if (at < birthDate)
                throw new ArgumentOutOfRangeException(nameof(at), "At date must not be before birthDate");

            var hadBirthday = birthDate.Month < at.Month
                || (birthDate.Month == at.Month && birthDate.Day <= at.Day);

            return at.Year - birthDate.Year - (hadBirthday ? 0 : 1);
        }
    }
}