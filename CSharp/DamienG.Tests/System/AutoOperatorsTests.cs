using System;
using DamienG.System;
using NUnit.Framework;

namespace DamienG.Tests.System
{
    [TestFixture]
    public class AutoOperatorsTests
    {
        [Test]
        public void CompareReturnsNegativeOneWhenFirstArgIsNullAndSecondIsNot()
        {
            var second = new InheritedTestClass<string>("Amiga");

            var comparison = AutoOperators.Compare(null, second);

            Assert.AreEqual(-1, comparison);
        }

        [Test]
        public void CompareReturnsPositiveOneWhenSecondArgIsNullAndFirstIsNot()
        {
            var first = new InheritedTestClass<string>("Amiga");

            var comparison = AutoOperators.Compare(first, null);

            Assert.AreEqual(1, comparison);
        }

        [Test]
        public void CompareReturnsZeroWhenBothArgsNull()
        {
            var comparison = AutoOperators.Compare(null, null);

            Assert.AreEqual(0, comparison);
        }

        [Test]
        public void CompareReturnsZeroWhenEqualValue()
        {
            var equal1 = new InheritedTestClass<string>("Amiga");
            var equal2 = new InheritedTestClass<string>("Amiga");

            var comparison = AutoOperators.Compare(equal1, equal2);

            Assert.AreEqual(0, comparison);
        }

        [Test]
        public void CompareReturnsZeroWhenSameReference()
        {
            var same = new InheritedTestClass<string>("Amiga");

            var comparison = AutoOperators.Compare(same, same);

            Assert.AreEqual(0, comparison);
        }

        [Test]
        public void EqualsReturnsFalseWhenNotEqual()
        {
            var notEqual1 = new InheritedTestClass<int>(2521);
            var notEqual2 = new InheritedTestClass<int>(2);

            Assert.IsFalse(notEqual1 == notEqual2);
        }

        [Test]
        public void EqualsReturnsTrueWhenEqual()
        {
            var equal1 = new InheritedTestClass<int>(2521);
            var equal2 = new InheritedTestClass<int>(2521);

            Assert.IsTrue(equal1 == equal2);
        }

        [Test]
        public void GreaterThanOrEqualReturnsFalseWhenLessThan()
        {
            var lower = new InheritedTestClass<int>(500);
            var higher = new InheritedTestClass<int>(2000);

            Assert.IsFalse(lower >= higher);
        }

        [Test]
        public void GreaterThanOrEqualReturnsTrueWhenEqual()
        {
            var equal1 = new InheritedTestClass<int>(520);
            var equal2 = new InheritedTestClass<int>(520);

            Assert.IsTrue(equal1 >= equal2);
        }

        [Test]
        public void GreaterThanOrEqualReturnsTrueWhenGreaterThan()
        {
            var lower = new InheritedTestClass<int>(720);
            var higher = new InheritedTestClass<int>(880);

            Assert.IsTrue(higher >= lower);
        }

        [Test]
        public void GreaterThanReturnsFalseWhenLessThan()
        {
            var lower = new InheritedTestClass<int>(252);
            var higher = new InheritedTestClass<int>(50809);

            Assert.IsFalse(lower > higher);
        }

        [Test]
        public void GreaterThanReturnsTrueWhenGreaterThan()
        {
            var lower = new InheritedTestClass<int>(252);
            var higher = new InheritedTestClass<int>(50809);

            Assert.IsTrue(higher > lower);
        }

        [Test]
        public void GreaterThanReturnsFalseWhenEqual()
        {
            var lower = new InheritedTestClass<int>(6508);
            var higher = new InheritedTestClass<int>(6508);

            Assert.IsFalse(lower > higher);
        }

        [Test]
        public void LessThanOrEqualReturnsFalseWhenGreaterThan()
        {
            var lower = new InheritedTestClass<int>(25);
            var higher = new InheritedTestClass<int>(5009);

            Assert.IsFalse(higher < lower);
        }

        [Test]
        public void LessThanOrEqualReturnsTrueWhenEqual()
        {
            var lower = new InheritedTestClass<int>(984);
            var higher = new InheritedTestClass<int>(984);

            Assert.IsTrue(lower <= higher);
        }

        [Test]
        public void LessThanOrEqualReturnsTrueWhenLessThan()
        {
            var lower = new InheritedTestClass<int>(123);
            var higher = new InheritedTestClass<int>(789);

            Assert.IsTrue(lower <= higher);
        }

        [Test]
        public void LessThanReturnsFalseWhenEqual()
        {
            var lower = new InheritedTestClass<int>(68000);
            var higher = new InheritedTestClass<int>(68000);

            Assert.IsFalse(higher < lower);
        }

        [Test]
        public void LessThanReturnsFalseWhenGreaterThan()
        {
            var lower = new InheritedTestClass<int>(16);
            var higher = new InheritedTestClass<int>(4096);

            Assert.IsFalse(higher < lower);
        }

        [Test]
        public void LessThanReturnsTrueWhenLessThan()
        {
            var lower = new InheritedTestClass<int>(4);
            var higher = new InheritedTestClass<int>(8);

            Assert.IsTrue(lower < higher);
        }

        [Test]
        public void NotEqualsReturnsFalseWhenEqual()
        {
            var equal1 = new InheritedTestClass<int>(2521);
            var equal2 = new InheritedTestClass<int>(2521);

            Assert.IsFalse(equal1 != equal2);
        }

        [Test]
        public void NotEqualsReturnsTrueWhenNotEqual()
        {
            var notEqual1 = new InheritedTestClass<int>(2521);
            var notEqual2 = new InheritedTestClass<int>(2);

            Assert.IsTrue(notEqual1 != notEqual2);
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