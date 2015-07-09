using System;
using DamienG.IO;
using Xunit;

namespace DamienG.Tests.IO
{
    public class ActionTextWriterTests
    {
        const string SampleString = "Some bytes wander by mistake";

        [Fact]
        public void WriteCharArrayGivenBufferAndEndRangePerformsActionWithPartialValue()
        {
            var startOffset = SampleString.Length/2;
            var expected = SampleString.Substring(startOffset);

            var actual = string.Empty;
            new ActionTextWriter(value => actual = value).Write(SampleString.ToCharArray(), startOffset, expected.Length);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WriteCharArrayGivenBufferAndFullRangePerformsActionWithFullValue()
        {
            var actual = string.Empty;
            new ActionTextWriter(value => actual = value).Write(SampleString.ToCharArray(), 0, SampleString.ToCharArray().Length);

            Assert.Equal(SampleString, actual);
        }

        [Fact]
        public void WriteCharArrayGivenBufferAndMidRangePerformsActionWithPartialValue()
        {
            var quarterLength = SampleString.Length/4;
            var expected = SampleString.Substring(quarterLength, quarterLength);

            var actual = string.Empty;
            new ActionTextWriter(value => actual = value).Write(SampleString.ToCharArray(), quarterLength, quarterLength);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WriteCharArrayGivenBufferAndStartRangePerformsActionWithPartialValue()
        {
            var partialLength = SampleString.Length/2;
            var expected = SampleString.Substring(0, partialLength);

            var actual = string.Empty;
            new ActionTextWriter(value => actual = value).Write(SampleString.ToCharArray(), 0, partialLength);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WriteCharArrayGivenLengthBeyondBoundaryThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ActionTextWriter(k => k += k).Write(SampleString.ToCharArray(), 0, 500));
        }

        [Fact]
        public void WriteCharArrayGivenNegativeIndexThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ActionTextWriter(k => k += k).Write(SampleString.ToCharArray(), -1, 5));
        }

        [Fact]
        public void WriteCharArrayGivenNegativeLengthThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ActionTextWriter(k => k += k).Write(SampleString.ToCharArray(), 0, -5));
        }

        [Fact]
        public void WriteCharArrayGivenNullArrayThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ActionTextWriter(k => k += k).Write(null, 0, 0));
        }

        [Fact]
        public void WriteLineGivenNullStringPerformsAction()
        {
            var actual = String.Empty;
            new ActionTextWriter(value => actual = value).WriteLine((string) null);

            Assert.Equal(Environment.NewLine, actual);
        }

        [Fact]
        public void WriteLineGivenStringPerformsAction()
        {
            const string original = "A simple line";
            var actual = String.Empty;
            new ActionTextWriter(value => actual += value).WriteLine(original);

            Assert.Equal(original + Environment.NewLine, actual);
        }

        [Fact]
        public void WriteGivenNullStringPerformsAction()
        {
            var actual = String.Empty;
            new ActionTextWriter(value => actual = value).Write((string) null);

            Assert.Null(actual);
        }

        [Fact]
        public void WriteGivenStringPerformsAction()
        {
            const string expected = "ABC 123";
            var actual = String.Empty;
            new ActionTextWriter(value => actual = value).Write(expected);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WriteGivenStringPerformsActionManyTimes()
        {
            string[] originals = { "First", "Second", "Third" };
            var expected = string.Join(Environment.NewLine, originals);
            var actual = string.Empty;
            var actionTextWriter = new ActionTextWriter(value => actual += value);
            foreach (var original in originals)
                actionTextWriter.WriteLine(original);

            Assert.Equal(expected + Environment.NewLine, actual);
        }
    }
}