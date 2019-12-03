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
        public static int GetAge(this DateTime birthDate)
        {
            return GetAge(birthDate, DateTime.Now);
        }

        public static int GetAge(this DateTime birthDate, DateTime at)
        {
            if (at < birthDate)
                throw new ArgumentOutOfRangeException(nameof(at), "At date can not be before birthDate");

            var hadBirthday = birthDate.Month < at.Month
                || (birthDate.Month == at.Month && birthDate.Day <= at.Day);

            return at.Year - birthDate.Year - (hadBirthday ? 0 : 1);
        }
    }
}