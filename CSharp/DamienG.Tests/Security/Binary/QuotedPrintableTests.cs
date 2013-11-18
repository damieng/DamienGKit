using System;
using System.Linq;
using System.Text;
using System.IO;
using DamienG.System.Binary;
using NUnit.Framework;

namespace DamienG.Tests.System.Binary
{
    [TestFixture]
    public class QuotedPrintableTests
    {
        [Test]
        public void EncodeGivenAsciiReturnsAsciiString()
        {
            var expected = "!ThisIsABasicTest^";

            var actual = new QuotedPrintable().Encode(Encoding.ASCII.GetBytes(expected));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EncodeEncodesTabsAndSpacesByDefault()
        {
            var text = "!This Is A Basic\tTest^";
            var expected = text.Replace(" ", "=20").Replace("\t", "=09");

            var actual = new QuotedPrintable().Encode(Encoding.ASCII.GetBytes(text));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EncodeDoesNotEncodeSpacesIfSpecified()
        {
            var text = "!This Is\tA Basic Test^";
            var expected = text.Replace("\t", "=09");

            var actual = new QuotedPrintable(DoNotEncode.Space).Encode(Encoding.ASCII.GetBytes(text));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EncodeDoesNotEncodeTabsIfSpecified()
        {
            var text = "!This Is A Basic\tTest^";
            var expected = text.Replace(" ", "=20");

            var actual = new QuotedPrintable(DoNotEncode.Tab).Encode(Encoding.ASCII.GetBytes(text));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EncodeDoesNotCreateLinesOver76CharsInLength()
        {
            var text = String.Join("-", Enumerable.Repeat("A quick brown fox ", 100));

            var actual = new QuotedPrintable(DoNotEncode.Space).Encode(Encoding.ASCII.GetBytes(text));

            using (var reader = new StringReader(actual))
                Assert.That(reader.ReadLine().Length <= 76);
        }

        [Test]
        public void EncodeDoesNotEncodeTabsOrSpacesIfSpecified()
        {
            var text = "!This Is A Basic\tTest^";
            var expected = text.Replace(" ", "=20").Replace("\t", "=09");

            var actual = new QuotedPrintable(DoNotEncode.Tab & DoNotEncode.Space).Encode(Encoding.ASCII.GetBytes(text));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EncodePutsEqualsAtEndIfLastCharacterIsUnencodedSpace()
        {
            var text = "Line Ends With ";

            var actual = new QuotedPrintable(DoNotEncode.Space).Encode(Encoding.ASCII.GetBytes(text));

            Assert.AreEqual(text + "=", actual);
        }

        [Test]
        public void EncodeDoesNotPutsEqualsAtEndIfLastCharacterEncoded()
        {
            var text = "Line Ends With ";
            var expected = text.Replace(" ", "=20");

            var actual = new QuotedPrintable().Encode(Encoding.ASCII.GetBytes(text));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EncodePutsEqualsAtEndIfLastCharacterIsUnencodedTab()
        {
            var text = "LineEndsWith\t";

            var actual = new QuotedPrintable(DoNotEncode.Tab).Encode(Encoding.ASCII.GetBytes(text));

            Assert.AreEqual(text + "=", actual);
        }

        [Test]
        public void EncodeEncodesEquals()
        {
            var text = "This=Is=A=Test";
            var expected = text.Replace("=", "=3D");

            var actual = new QuotedPrintable().Encode(Encoding.ASCII.GetBytes(text));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EncodeGivenBinaryEncodesBinary()
        {
            var unencoded = new byte[] { 0x10, 0x44, 0x00, 0xA3, 0xFF, 0xDE, 0x4E };
            var expected = "=10D=00=A3=FF=DEN";

            var actual = new QuotedPrintable().Encode(unencoded);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DecodeGivenAsciiReturnsAsciiBytes()
        {
            var text = "This is a cool test";
            var expected = Encoding.ASCII.GetBytes(text);

            var actual = new QuotedPrintable().Decode(text);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DecodeGivenMixReturnsDecodedBytes()
        {
            var text = "^Aa =20\t=09!";
            var expected = new [] { 0x5E, 0x41, 0x61, 0x20, 0x20, 0x09, 0x09, 0x21 };

            var actual = new QuotedPrintable().Decode(text);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DecodeIgnoresTrailingEquals()
        {
            var text = " A =";
            var expected = new [] { 0x20, 0x41, 0x20 };

            var actual = new QuotedPrintable().Decode(text);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DecodeThrowsArgumentOutOfRangeIfQuotedEndsBeforeCompletion()
        {
            new QuotedPrintable().Decode("Dam=2");
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DecodeThrowsArgumentOutOfRangeIfNotHex()
        {
            new QuotedPrintable().Decode("Dam=4G");
        }
    }
}