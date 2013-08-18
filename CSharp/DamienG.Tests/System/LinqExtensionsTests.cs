using System;
using System.Collections.Generic;
using System.Linq;
using DamienG.System;
using NUnit.Framework;

namespace DamienG.Tests.System
{
    [TestFixture]
    public class LinqExtensionsTests
    {
        private class Simple
        {
            public int Id;
            public string Name;
        }

        private readonly List<Simple> unsorted = new List<Simple> {
            new Simple { Id = 234, Name = "And" },
            new Simple { Id = 100, Name = "Seven" },
            new Simple { Id = 910, Name = "Score" },
            new Simple { Id = 123, Name = "Four" },
            new Simple { Id = 1001, Name = "Years" }
        };

        [Test]
        public void ContainsOrDefaultContainsValueAndReturnsIt()
        {
            var expected = unsorted[2];

            var actual = unsorted.ContainsOrDefault(expected);

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void ContainsOrDefaultDoesNotContainReturnsDefault()
        {
            var actual = unsorted.ContainsOrDefault(new Simple { Id = 999, Name = "Unknown" });

            Assert.IsNull(actual);
        }

        [Test]
        public void PageFirstReturnsFirstPage()
        {
            var expected = unsorted.Take(2).ToList();
            var actual = unsorted.Page(1, 2).ToList();
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void PageLastReturnsSubsetIfRangeExceeded()
        {
            var actual = unsorted.Page(1, 20).ToList();
            CollectionAssert.AreEqual(unsorted, actual);
        }

        [Test]
        public void PagePastEndOfRangeReturnsNoResults()
        {
            var actual = unsorted.Page(10, 10).ToList();
            CollectionAssert.IsEmpty(actual);
        }

        [Test]
        public void PageSecondReturnsSecondPage()
        {
            var expected = unsorted.Skip(2).Take(2).ToList();
            var actual = unsorted.Page(2, 2).ToList();
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void PageZeroPageIndexThrowsArgumentException()
        {
            unsorted.Page(0, 12);
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void PageZeroPageSizeThrowsArgumentException()
        {
            unsorted.Page(1, 0).ToList();
        }

        [Test]
        public void SortBySequenceByProperty()
        {
            var sequence = new[] { 123, 910, 234, 100, 1001 };

            var sorted = unsorted.OrderBySequence(u => u.Id, sequence);

            var expected = new[] { "Four", "Score", "And", "Seven", "Years" };
            CollectionAssert.AreEqual(expected, sorted.Select(s => s.Name).ToArray());
        }
    }
}