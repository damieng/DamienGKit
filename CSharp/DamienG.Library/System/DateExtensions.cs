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
                throw new ArgumentOutOfRangeException("at", "At date can not be before birthDate");

            var hadBirthday = birthDate.Month < at.Month
                || (birthDate.Month == at.Month && birthDate.Day <= at.Day);

            return at.Year - birthDate.Year - (hadBirthday ? 0 : 1);
        }
    }
}