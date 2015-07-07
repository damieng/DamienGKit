using System;
using System.Collections.Generic;
using System.Linq;
using DamienG.System;
using Xunit;

namespace DamienG.Tests.System
{
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

        [Fact]
        public void ContainsOrDefaultContainsValueAndReturnsIt()
        {
            var expected = unsorted[2];

            var actual = unsorted.ContainsOrDefault(expected);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void ContainsOrDefaultDoesNotContainReturnsDefault()
        {
            var actual = unsorted.ContainsOrDefault(new Simple { Id = 999, Name = "Unknown" });

            Assert.Null(actual);
        }

        [Fact]
        public void PageFirstReturnsFirstPage()
        {
            var expected = unsorted.Take(2).ToList();
            var actual = unsorted.Page(1, 2).ToList();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PageLastReturnsSubsetIfRangeExceeded()
        {
            var actual = unsorted.Page(1, 20).ToList();
            Assert.Equal(unsorted, actual);
        }

        [Fact]
        public void PagePastEndOfRangeReturnsNoResults()
        {
            var actual = unsorted.Page(10, 10).ToList();
            Assert.Empty(actual);
        }

        [Fact]
        public void PageSecondReturnsSecondPage()
        {
            var expected = unsorted.Skip(2).Take(2).ToList();
            var actual = unsorted.Page(2, 2).ToList();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PageZeroPageIndexThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => unsorted.Page(0, 12));
        }

        [Fact]
        public void PageZeroPageSizeThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => unsorted.Page(1, 0).ToList());
        }

        [Fact]
        public void SortBySequenceByProperty()
        {
            var sequence = new[] { 123, 910, 234, 100, 1001 };

            var sorted = unsorted.OrderBySequence(u => u.Id, sequence);

            var expected = new[] { "Four", "Score", "And", "Seven", "Years" };
            Assert.Equal(expected, sorted.Select(s => s.Name).ToArray());
        }
    }
}