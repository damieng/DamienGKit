using DamienG.System;
using NUnit.Framework;
using System;
using System.Linq;

namespace DamienG.Tests.System
{
    [TestFixture]
    public class EnumTTests
    {
        private enum NumberedEnum
        {
            One = 1,
            Two = 2,
            Zero = 0,
            Nine = 9,
            Five = 5
        };

        [Flags]
        private enum FlagEnum
        {
            BitOne = 1,
            BitTwo = 2,
            BitThree = 4,
            BitFour = 8,
            BitFive = 16,
            BitSix = 32,
            BitsTwoAndFour = 10
        };

        private const string ValidName = "Nine";
        private const int ValidInteger = 9;
        private const NumberedEnum ValidEnum = NumberedEnum.Nine;

        private const string InvalidName = "Ninety";
        private const int InvalidInteger = 800;

        [Test]
        public void CastOrNullCastsWhenValidInteger()
        {
            var actual = Enum<NumberedEnum>.CastOrNull(ValidInteger);

            Assert.AreEqual(ValidEnum, actual);
        }

        [Test]
        public void CastOrNullNullsWhenInvalidInteger()
        {
            var actual = Enum<NumberedEnum>.CastOrNull(InvalidInteger);

            Assert.IsNull(actual);
        }

        [Test]
        public void GetFlagsHandlesNoFlagsSet()
        {
            var actual = Enum<FlagEnum>.GetFlags(default(FlagEnum));

            CollectionAssert.IsEmpty(actual);
        }

        [Test]
        public void GetFlagsReturnsTheMultipleFlagsSet()
        {
            const FlagEnum multipleFlags = FlagEnum.BitOne | FlagEnum.BitFive | FlagEnum.BitThree;
            var actual = Enum<FlagEnum>.GetFlags(multipleFlags);

            CollectionAssert.AreEquivalent(new[] { FlagEnum.BitOne, FlagEnum.BitFive, FlagEnum.BitThree }, actual.ToArray());
        }

        [Test]
        public void GetFlagsReturnsTheMultipleFlagsSetIgnoringUndefined()
        {
            const FlagEnum multipleFlags = (FlagEnum) 255;
            var actual = Enum<FlagEnum>.GetFlags(multipleFlags);

            CollectionAssert.AreEquivalent(new[] { FlagEnum.BitOne, FlagEnum.BitTwo, FlagEnum.BitThree, FlagEnum.BitFour, FlagEnum.BitFive, FlagEnum.BitSix, FlagEnum.BitsTwoAndFour }, actual.ToArray());
        }

        [Test]
        public void GetFlagsReturnsTheMultipleFlagsSetIncludingCombinedMasks()
        {
            const FlagEnum combinedFlags = FlagEnum.BitsTwoAndFour;
            var actual = Enum<FlagEnum>.GetFlags(combinedFlags);

            CollectionAssert.AreEquivalent(new[] { FlagEnum.BitTwo, FlagEnum.BitFour, FlagEnum.BitsTwoAndFour }, actual.ToArray());
        }

        [Test]
        public void GetFlags_Returns_The_One_Flag_Set()
        {
            var actual = Enum<FlagEnum>.GetFlags(FlagEnum.BitOne);

            CollectionAssert.AreEquivalent(new[] { FlagEnum.BitOne }, actual.ToArray());
        }

        [Test]
        public void GetNameBehavesSame()
        {
            var expected = Enum.GetName(typeof (NumberedEnum), NumberedEnum.Five);
            var actual = Enum<NumberedEnum>.GetName(NumberedEnum.Five);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetNamesBehavesSame()
        {
            var expected = Enum.GetNames(typeof (NumberedEnum));
            var actual = Enum<NumberedEnum>.GetNames();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void GetValuesBehavesSame()
        {
            var expected = Enum.GetValues(typeof (NumberedEnum));
            var actual = Enum<NumberedEnum>.GetValues();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void IsDefinedWithInvalidIntegerBehavesSame()
        {
            var expected = Enum.IsDefined(typeof (NumberedEnum), InvalidInteger);
            var actual = Enum<NumberedEnum>.IsDefined(InvalidInteger);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsDefinedWithValidIntegerBehavesSame()
        {
            var expected = Enum.IsDefined(typeof (NumberedEnum), ValidInteger);
            var actual = Enum<NumberedEnum>.IsDefined(ValidInteger);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseOrNullInsensitiveReturnsCorrectValueWhenValidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName, true, out parsedValue);

            Assert.IsTrue(didParse);
            Assert.AreEqual(ValidEnum, parsedValue);
        }

        [Test]
        public void ParseOrNullInsensitiveReturnsCorrectValueWhenValidNameWrongCase()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName.ToUpper(), true, out parsedValue);

            Assert.IsTrue(didParse);
            Assert.AreEqual(ValidEnum, parsedValue);
        }

        [Test]
        public void ParseOrNullInsensitiveReturnsNullWhenInvalidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(InvalidName, true, out parsedValue);

            Assert.IsFalse(didParse);
        }

        [Test]
        public void ParseOrNullReturnsCorrectValueWhenValidName()
        {
            var actual = Enum<NumberedEnum>.ParseOrNull(ValidName);

            Assert.AreEqual(ValidEnum, actual);
        }

        [Test]
        public void ParseOrNullReturnsNullValueWhenInvalidName()
        {
            var actual = Enum<NumberedEnum>.ParseOrNull(InvalidName);

            Assert.IsNull(actual);
        }

        [Test]
        public void ParseOrNullReturnsNullValueWhenValidNameWrongCase()
        {
            var actual = Enum<NumberedEnum>.ParseOrNull(ValidName.ToUpper());

            Assert.IsNull(actual);
        }

        [Test]
        public void ParseOrNullSensitiveReturnsCorrectValueWhenValidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName, false, out parsedValue);

            Assert.IsTrue(didParse);
            Assert.AreEqual(ValidEnum, parsedValue);
        }

        [Test]
        public void ParseOrNullSensitiveReturnsFalseWhenValidNameWrongCase()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName.ToUpper(), false, out parsedValue);

            Assert.IsFalse(didParse);
        }

        [Test]
        public void ParseOrNullSensitiveReturnsNullWhenInvalidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(InvalidName, false, out parsedValue);

            Assert.IsFalse(didParse);
        }

        [Test]
        public void ParseReturnsValidValueWhenValidName()
        {
            var actual = Enum<NumberedEnum>.Parse(ValidName);

            Assert.AreEqual(ValidEnum, actual);
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void ParseThrowsWhenInvalidName()
        {
            Enum<NumberedEnum>.Parse(InvalidName);
        }

        [Test]
        public void SetFlagsShouldDefaultWhenGivenNoValues()
        {
            const FlagEnum expected = default(FlagEnum);
            var actual = Enum<FlagEnum>.SetFlags(Enumerable.Empty<FlagEnum>());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SetFlagsShouldReturnCombinationFlagFromIndividualMasks()
        {
            const FlagEnum expected = FlagEnum.BitsTwoAndFour;
            var actual = Enum<FlagEnum>.SetFlags(new[] { FlagEnum.BitFour, FlagEnum.BitTwo });

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TryParseInsensitiveReturnsFalseWhenInvalidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(InvalidName, true, out parsedValue);

            Assert.IsFalse(didParse);
        }

        [Test]
        public void TryParseInsensitiveReturnsTrueAndCorrectOutputValueWhenValidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName, true, out parsedValue);

            Assert.IsTrue(didParse);
            Assert.AreEqual(ValidEnum, parsedValue);
        }

        [Test]
        public void TryParseInsensitiveReturnsTrueAndCorrectOutputValueWhenValidNameWrongCase()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName.ToUpper(), true, out parsedValue);

            Assert.IsTrue(didParse);
            Assert.AreEqual(ValidEnum, parsedValue);
        }

        [Test]
        public void TryParseReturnsFalseWhenInvalidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(InvalidName, out parsedValue);

            Assert.IsFalse(didParse);
        }

        [Test]
        public void TryParseReturnsFalseWhenValidNameWrongCase()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName.ToUpper(), false, out parsedValue);

            Assert.IsFalse(didParse);
        }

        [Test]
        public void TryParseReturnsTrueAndCorrectOutputValueWhenValidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName, out parsedValue);

            Assert.IsTrue(didParse);
            Assert.AreEqual(ValidEnum, parsedValue);
        }
    }
}