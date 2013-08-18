using System;
using DamienG.System;
using NUnit.Framework;

namespace DamienG.Tests.System
{
    [TestFixture]
    public class DateExtensionsTests
    {
        [Test]
        public void GetAgeWithTodaySpecifiedForBirthdayTomorrowSameMonth()
        {
            var today = new DateTime(2011, 3, 6);
            var birthday = new DateTime(1994, 3, 7);

            var age = birthday.GetAge(today);

            Assert.AreEqual(16, age);
        }

        [Test]
        public void GetAgeWithTodaySpecifiedForBirthdayTomorrowNextMonth()
        {
            var today = new DateTime(2011, 8, 31);
            var birthday = new DateTime(2000, 9, 1);
            var age = birthday.GetAge(today);

            Assert.AreEqual(10, age);
        }

        [Test]
        public void GetAgeWithTodaySpecifiedForBirthdayTomorrowNextYear()
        {
            var today = new DateTime(2011, 1, 1);
            var birthday = new DateTime(2000, 12, 31);
            var age = birthday.GetAge(today);

            Assert.AreEqual(10, age);
        }

        [Test]
        public void GetAgeWithTodaySpecifiedForBirthdayYesterdaySameMonth()
        {
            var birthday = new DateTime(1968, 3, 5);
            var today = new DateTime(2000, 3, 5);
            var age = birthday.GetAge(today);

            Assert.AreEqual(32, age);
        }

        [Test]
        public void GetAgeWithTodaySpecifiedForBirthdayYesterdayPriorMonth()
        {
            var birthday = new DateTime(1990, 9, 30);
            var today = new DateTime(2011, 10, 1);
            var age = birthday.GetAge(today);

            Assert.AreEqual(21, age);
        }

        [Test]
        public void GetAgeWithTodaySpecifiedForBirthdayYesterdayPriorYear()
        {
            var today = new DateTime(2020, 12, 31);
            var birthday = new DateTime(1994, 1, 1);
            var age = birthday.GetAge(today);

            Assert.AreEqual(26, age);
        }

        [Test]
        public void GetAgeWithTodaySpecifiedForBirthdayToday()
        {
            var birthday = new DateTime(1970, 3, 5);
            var today = new DateTime(2000, 3, 5);
            var age = birthday.GetAge(today);

            Assert.AreEqual(30, age);
        }

        [Test]
        public void GetAgeNowForBirthdayToday()
        {
            var birthday = new DateTime(1970, 3, 5);
            var today = new DateTime(2000, 3, 5);
            var age = birthday.GetAge(today);

            Assert.AreEqual(30, age);
        }

        [Test]
        public void GetAgeNowForBirthdayYesterday()
        {
            var birthday = new DateTime(1970, 3, 4);
            var today = new DateTime(2000, 3, 5);
            var age = birthday.GetAge(today);

            Assert.AreEqual(30, age);
        }

        [Test]
        public void GetAgeNowForBirthdayTomorrow()
        {
            var birthday = new DateTime(1970, 3, 6);
            var today = new DateTime(2000, 3, 5);
            var age = birthday.GetAge(today);

            Assert.AreEqual(29, age);
        }
    }
}