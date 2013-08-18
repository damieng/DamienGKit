using System;
using DamienG.IO;
using NUnit.Framework;

namespace DamienG.Tests.IO
{
    [TestFixture]
    public class ActionTextWriterTests
    {
        private const string SampleString = "Some bytes wander by mistake";

        [Test]
        public void WriteCharArrayGivenBufferAndEndRangePerformsActionWithPartialValue()
        {
            var startOffset = SampleString.Length/2;
            var expected = SampleString.Substring(startOffset);

            var actual = string.Empty;
            new ActionTextWriter(value => actual = value).Write(SampleString.ToCharArray(), startOffset, expected.Length);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WriteCharArrayGivenBufferAndFullRangePerformsActionWithFullValue()
        {
            var actual = string.Empty;
            new ActionTextWriter(value => actual = value).Write(SampleString.ToCharArray(), 0, SampleString.ToCharArray().Length);

            Assert.AreEqual(SampleString, actual);
        }

        [Test]
        public void WriteCharArrayGivenBufferAndMidRangePerformsActionWithPartialValue()
        {
            var quarterLength = SampleString.Length/4;
            var expected = SampleString.Substring(quarterLength, quarterLength);

            var actual = string.Empty;
            new ActionTextWriter(value => actual = value).Write(SampleString.ToCharArray(), quarterLength, quarterLength);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WriteCharArrayGivenBufferAndStartRangePerformsActionWithPartialValue()
        {
            var partialLength = SampleString.Length/2;
            var expected = SampleString.Substring(0, partialLength);

            var actual = string.Empty;
            new ActionTextWriter(value => actual = value).Write(SampleString.ToCharArray(), 0, partialLength);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void WriteCharArrayGivenLengthBeyondBoundaryThrowsArgumentOutOfRangeException()
        {
            new ActionTextWriter(k => k += k).Write(SampleString.ToCharArray(), 0, 500);
        }

        [Test]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void WriteCharArrayGivenNegativeIndexThrowsArgumentOutOfRangeException()
        {
            new ActionTextWriter(k => k += k).Write(SampleString.ToCharArray(), -1, 5);
        }

        [Test]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void WriteCharArrayGivenNegativeLengthThrowsArgumentOutOfRangeException()
        {
            new ActionTextWriter(k => k += k).Write(SampleString.ToCharArray(), 0, -5);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void WriteCharArrayGivenNullArrayThrowsArgumentNullException()
        {
            new ActionTextWriter(k => k += k).Write(null, 0, 0);
        }

        [Test]
        public void WriteLineGivenNullStringPerformsAction()
        {
            var actual = String.Empty;
            new ActionTextWriter(value => actual = value).WriteLine((string) null);

            Assert.AreEqual(Environment.NewLine, actual);
        }

        [Test]
        public void WriteLineGivenStringPerformsAction()
        {
            const string original = "A simple line";
            var actual = String.Empty;
            new ActionTextWriter(value => actual += value).WriteLine(original);

            Assert.AreEqual(original + Environment.NewLine, actual);
        }

        [Test]
        public void WriteGivenNullStringPerformsAction()
        {
            var actual = String.Empty;
            new ActionTextWriter(value => actual = value).Write((string) null);

            Assert.IsNull(actual);
        }

        [Test]
        public void WriteGivenStringPerformsAction()
        {
            const string expected = "ABC 123";
            var actual = String.Empty;
            new ActionTextWriter(value => actual = value).Write(expected);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WriteGivenStringPerformsActionManyTimes()
        {
            string[] originals = { "First", "Second", "Third" };
            var expected = string.Join(Environment.NewLine, originals);
            var actual = string.Empty;
            var actionTextWriter = new ActionTextWriter(value => actual += value);
            foreach (var original in originals)
                actionTextWriter.WriteLine(original);

            Assert.AreEqual(expected + Environment.NewLine, actual);
        }
    }
}