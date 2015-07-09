using DamienG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DamienG.Tests.System
{
    public class EnumTTests
    {
        enum NumberedEnum
        {
            One = 1,
            Two = 2,
            Zero = 0,
            Nine = 9,
            Five = 5
        };

        [Flags]
        enum FlagEnum
        {
            BitOne = 1,
            BitTwo = 2,
            BitThree = 4,
            BitFour = 8,
            BitFive = 16,
            BitSix = 32,
            BitsTwoAndFour = 10
        };

        const string ValidName = "Nine";
        const int ValidInteger = 9;
        const NumberedEnum ValidEnum = NumberedEnum.Nine;

        const string InvalidName = "Ninety";
        const int InvalidInteger = 800;

        [Fact]
        public void CastOrNullCastsWhenValidInteger()
        {
            var actual = Enum<NumberedEnum>.CastOrNull(ValidInteger);

            Assert.Equal(ValidEnum, actual);
        }

        [Fact]
        public void CastOrNullNullsWhenInvalidInteger()
        {
            var actual = Enum<NumberedEnum>.CastOrNull(InvalidInteger);

            Assert.Null(actual);
        }

        [Fact]
        public void GetFlagsHandlesNoFlagsSet()
        {
            var actual = Enum<FlagEnum>.GetFlags(default(FlagEnum));

            Assert.Empty(actual);
        }

        [Fact]
        public void GetFlagsReturnsTheMultipleFlagsSet()
        {
            const FlagEnum multipleFlags = FlagEnum.BitOne | FlagEnum.BitFive | FlagEnum.BitThree;
            var actual = Enum<FlagEnum>.GetFlags(multipleFlags);

            Assert.Equal(new[] { FlagEnum.BitOne, FlagEnum.BitThree, FlagEnum.BitFive }, actual.ToArray());
        }

        [Fact]
        public void GetFlagsReturnsTheMultipleFlagsSetIgnoringUndefined()
        {
            const FlagEnum multipleFlags = (FlagEnum) 255;
            var actual = Enum<FlagEnum>.GetFlags(multipleFlags);

            Assert.Equal(new[] { FlagEnum.BitOne, FlagEnum.BitTwo, FlagEnum.BitThree, FlagEnum.BitFour, FlagEnum.BitsTwoAndFour, FlagEnum.BitFive, FlagEnum.BitSix }, actual.ToArray());
        }

        [Fact]
        public void GetFlagsReturnsTheMultipleFlagsSetIncludingCombinedMasks()
        {
            const FlagEnum combinedFlags = FlagEnum.BitsTwoAndFour;
            var actual = Enum<FlagEnum>.GetFlags(combinedFlags);

            Assert.Equal(new[] { FlagEnum.BitTwo, FlagEnum.BitFour, FlagEnum.BitsTwoAndFour }, actual.ToArray());
        }

        [Fact]
        public void GetFlags_Returns_The_One_Flag_Set()
        {
            var actual = Enum<FlagEnum>.GetFlags(FlagEnum.BitOne);

            Assert.Equal(new[] { FlagEnum.BitOne }, actual.ToArray());
        }

        [Fact]
        public void GetNameBehavesSame()
        {
            var expected = Enum.GetName(typeof (NumberedEnum), NumberedEnum.Five);
            var actual = Enum<NumberedEnum>.GetName(NumberedEnum.Five);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNamesBehavesSame()
        {
            var expected = Enum.GetNames(typeof (NumberedEnum));
            var actual = Enum<NumberedEnum>.GetNames();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetValuesBehavesSame()
        {
            var expected = Enum.GetValues(typeof (NumberedEnum));
            var actual = Enum<NumberedEnum>.GetValues().ToArray();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsDefinedWithInvalidIntegerBehavesSame()
        {
            var expected = Enum.IsDefined(typeof (NumberedEnum), InvalidInteger);
            var actual = Enum<NumberedEnum>.IsDefined(InvalidInteger);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsDefinedWithValidIntegerBehavesSame()
        {
            var expected = Enum.IsDefined(typeof (NumberedEnum), ValidInteger);
            var actual = Enum<NumberedEnum>.IsDefined(ValidInteger);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ParseOrNullInsensitiveReturnsCorrectValueWhenValidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName, true, out parsedValue);

            Assert.True(didParse);
            Assert.Equal(ValidEnum, parsedValue);
        }

        [Fact]
        public void ParseOrNullInsensitiveReturnsCorrectValueWhenValidNameWrongCase()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName.ToUpper(), true, out parsedValue);

            Assert.True(didParse);
            Assert.Equal(ValidEnum, parsedValue);
        }

        [Fact]
        public void ParseOrNullInsensitiveReturnsNullWhenInvalidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(InvalidName, true, out parsedValue);

            Assert.False(didParse);
        }

        [Fact]
        public void ParseOrNullReturnsCorrectValueWhenValidName()
        {
            var actual = Enum<NumberedEnum>.ParseOrNull(ValidName);

            Assert.Equal(ValidEnum, actual);
        }

        [Fact]
        public void ParseOrNullReturnsNullValueWhenInvalidName()
        {
            var actual = Enum<NumberedEnum>.ParseOrNull(InvalidName);

            Assert.Null(actual);
        }

        [Fact]
        public void ParseOrNullReturnsNullValueWhenValidNameWrongCase()
        {
            var actual = Enum<NumberedEnum>.ParseOrNull(ValidName.ToUpper());

            Assert.Null(actual);
        }

        [Fact]
        public void ParseOrNullSensitiveReturnsCorrectValueWhenValidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName, false, out parsedValue);

            Assert.True(didParse);
            Assert.Equal(ValidEnum, parsedValue);
        }

        [Fact]
        public void ParseOrNullSensitiveReturnsFalseWhenValidNameWrongCase()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName.ToUpper(), false, out parsedValue);

            Assert.False(didParse);
        }

        [Fact]
        public void ParseOrNullSensitiveReturnsNullWhenInvalidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(InvalidName, false, out parsedValue);

            Assert.False(didParse);
        }

        [Fact]
        public void ParseReturnsValidValueWhenValidName()
        {
            var actual = Enum<NumberedEnum>.Parse(ValidName);

            Assert.Equal(ValidEnum, actual);
        }

        [Fact]
        public void ParseThrowsWhenInvalidName()
        {
            Assert.Throws<ArgumentException>(() => Enum<NumberedEnum>.Parse(InvalidName));
        }

        [Fact]
        public void SetFlagsShouldDefaultWhenGivenNoValues()
        {
            const FlagEnum expected = default(FlagEnum);
            var actual = Enum<FlagEnum>.SetFlags(Enumerable.Empty<FlagEnum>());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetFlagsShouldReturnCombinationFlagFromIndividualMasks()
        {
            const FlagEnum expected = FlagEnum.BitsTwoAndFour;
            var actual = Enum<FlagEnum>.SetFlags(new[] { FlagEnum.BitFour, FlagEnum.BitTwo });

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TryParseInsensitiveReturnsFalseWhenInvalidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(InvalidName, true, out parsedValue);

            Assert.False(didParse);
        }

        [Fact]
        public void TryParseInsensitiveReturnsTrueAndCorrectOutputValueWhenValidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName, true, out parsedValue);

            Assert.True(didParse);
            Assert.Equal(ValidEnum, parsedValue);
        }

        [Fact]
        public void TryParseInsensitiveReturnsTrueAndCorrectOutputValueWhenValidNameWrongCase()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName.ToUpper(), true, out parsedValue);

            Assert.True(didParse);
            Assert.Equal(ValidEnum, parsedValue);
        }

        [Fact]
        public void TryParseReturnsFalseWhenInvalidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(InvalidName, out parsedValue);

            Assert.False(didParse);
        }

        [Fact]
        public void TryParseReturnsFalseWhenValidNameWrongCase()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName.ToUpper(), false, out parsedValue);

            Assert.False(didParse);
        }

        [Fact]
        public void TryParseReturnsTrueAndCorrectOutputValueWhenValidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName, out parsedValue);

            Assert.True(didParse);
            Assert.Equal(ValidEnum, parsedValue);
        }
    }
}