using System;
using DamienG.System;
using Xunit;

namespace DamienG.Tests.System
{
    public class AutoOperatorsTests
    {
        [Fact]
        public void CompareReturnsNegativeOneWhenFirstArgIsNullAndSecondIsNot()
        {
            var second = new InheritedTestClass<string>("Amiga");

            var comparison = AutoOperators.Compare(null, second);

            Assert.Equal(-1, comparison);
        }

        [Fact]
        public void CompareReturnsPositiveOneWhenSecondArgIsNullAndFirstIsNot()
        {
            var first = new InheritedTestClass<string>("Amiga");

            var comparison = AutoOperators.Compare(first, null);

            Assert.Equal(1, comparison);
        }

        [Fact]
        public void CompareReturnsZeroWhenBothArgsNull()
        {
            var comparison = AutoOperators.Compare(null, null);

            Assert.Equal(0, comparison);
        }

        [Fact]
        public void CompareReturnsZeroWhenEqualValue()
        {
            var equal1 = new InheritedTestClass<string>("Amiga");
            var equal2 = new InheritedTestClass<string>("Amiga");

            var comparison = AutoOperators.Compare(equal1, equal2);

            Assert.Equal(0, comparison);
        }

        [Fact]
        public void CompareReturnsZeroWhenSameReference()
        {
            var same = new InheritedTestClass<string>("Amiga");

            var comparison = AutoOperators.Compare(same, same);

            Assert.Equal(0, comparison);
        }

        [Fact]
        public void EqualsReturnsFalseWhenNotEqual()
        {
            var notEqual1 = new InheritedTestClass<int>(2521);
            var notEqual2 = new InheritedTestClass<int>(2);

            Assert.False(notEqual1 == notEqual2);
        }

        [Fact]
        public void EqualsReturnsTrueWhenEqual()
        {
            var equal1 = new InheritedTestClass<int>(2521);
            var equal2 = new InheritedTestClass<int>(2521);

            Assert.True(equal1 == equal2);
        }

        [Fact]
        public void GreaterThanOrEqualReturnsFalseWhenLessThan()
        {
            var lower = new InheritedTestClass<int>(500);
            var higher = new InheritedTestClass<int>(2000);

            Assert.False(lower >= higher);
        }

        [Fact]
        public void GreaterThanOrEqualReturnsTrueWhenEqual()
        {
            var equal1 = new InheritedTestClass<int>(520);
            var equal2 = new InheritedTestClass<int>(520);

            Assert.True(equal1 >= equal2);
        }

        [Fact]
        public void GreaterThanOrEqualReturnsTrueWhenGreaterThan()
        {
            var lower = new InheritedTestClass<int>(720);
            var higher = new InheritedTestClass<int>(880);

            Assert.True(higher >= lower);
        }

        [Fact]
        public void GreaterThanReturnsFalseWhenLessThan()
        {
            var lower = new InheritedTestClass<int>(252);
            var higher = new InheritedTestClass<int>(50809);

            Assert.False(lower > higher);
        }

        [Fact]
        public void GreaterThanReturnsTrueWhenGreaterThan()
        {
            var lower = new InheritedTestClass<int>(252);
            var higher = new InheritedTestClass<int>(50809);

            Assert.True(higher > lower);
        }

        [Fact]
        public void GreaterThanReturnsFalseWhenEqual()
        {
            var lower = new InheritedTestClass<int>(6508);
            var higher = new InheritedTestClass<int>(6508);

            Assert.False(lower > higher);
        }

        [Fact]
        public void LessThanOrEqualReturnsFalseWhenGreaterThan()
        {
            var lower = new InheritedTestClass<int>(25);
            var higher = new InheritedTestClass<int>(5009);

            Assert.False(higher < lower);
        }

        [Fact]
        public void LessThanOrEqualReturnsTrueWhenEqual()
        {
            var lower = new InheritedTestClass<int>(984);
            var higher = new InheritedTestClass<int>(984);

            Assert.True(lower <= higher);
        }

        [Fact]
        public void LessThanOrEqualReturnsTrueWhenLessThan()
        {
            var lower = new InheritedTestClass<int>(123);
            var higher = new InheritedTestClass<int>(789);

            Assert.True(lower <= higher);
        }

        [Fact]
        public void LessThanReturnsFalseWhenEqual()
        {
            var lower = new InheritedTestClass<int>(68000);
            var higher = new InheritedTestClass<int>(68000);

            Assert.False(higher < lower);
        }

        [Fact]
        public void LessThanReturnsFalseWhenGreaterThan()
        {
            var lower = new InheritedTestClass<int>(16);
            var higher = new InheritedTestClass<int>(4096);

            Assert.False(higher < lower);
        }

        [Fact]
        public void LessThanReturnsTrueWhenLessThan()
        {
            var lower = new InheritedTestClass<int>(4);
            var higher = new InheritedTestClass<int>(8);

            Assert.True(lower < higher);
        }

        [Fact]
        public void NotEqualsReturnsFalseWhenEqual()
        {
            var equal1 = new InheritedTestClass<int>(2521);
            var equal2 = new InheritedTestClass<int>(2521);

            Assert.False(equal1 != equal2);
        }

        [Fact]
        public void NotEqualsReturnsTrueWhenNotEqual()
        {
            var notEqual1 = new InheritedTestClass<int>(2521);
            var notEqual2 = new InheritedTestClass<int>(2);

            Assert.True(notEqual1 != notEqual2);
        }
    }

    public class InheritedTestClass<T> : AutoOperators where T : IComparable
    {
        private readonly T primitive;

        public InheritedTestClass(T primitive)
        {
            this.primitive = primitive;
        }

        public override int CompareTo(object obj)
        {
            var comparison = obj as InheritedTestClass<T>;
            if (comparison == null)
                return -1;

            return primitive.CompareTo(comparison.primitive);
        }

        public override int GetHashCode()
        {
            return primitive.GetHashCode();
        }
    }
}