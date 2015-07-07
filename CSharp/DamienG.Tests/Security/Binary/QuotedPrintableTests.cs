using System;
using System.Linq;
using System.Text;
using System.IO;
using DamienG.System.Binary;
using Xunit;

namespace DamienG.Tests.Security.Binary
{
    public class QuotedPrintableTests
    {
        [Fact]
        public void EncodeGivenAsciiReturnsAsciiString()
        {
            var expected = "!ThisIsABasicTest^";

            var actual = new QuotedPrintable().Encode(Encoding.ASCII.GetBytes(expected));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EncodeEncodesTabsAndSpacesByDefault()
        {
            var text = "!This Is A Basic\tTest^";
            var expected = text.Replace(" ", "=20").Replace("\t", "=09");

            var actual = new QuotedPrintable().Encode(Encoding.ASCII.GetBytes(text));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EncodeDoesNotEncodeSpacesIfSpecified()
        {
            var text = "!This Is\tA Basic Test^";
            var expected = text.Replace("\t", "=09");

            var actual = new QuotedPrintable(DoNotEncode.Space).Encode(Encoding.ASCII.GetBytes(text));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EncodeDoesNotEncodeTabsIfSpecified()
        {
            var text = "!This Is A Basic\tTest^";
            var expected = text.Replace(" ", "=20");

            var actual = new QuotedPrintable(DoNotEncode.Tab).Encode(Encoding.ASCII.GetBytes(text));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EncodeDoesNotCreateLinesOver76CharsInLength()
        {
            var text = String.Join("-", Enumerable.Repeat("A quick brown fox ", 100));

            var actual = new QuotedPrintable(DoNotEncode.Space).Encode(Encoding.ASCII.GetBytes(text));

            using (var reader = new StringReader(actual))
                Assert.True(reader.ReadLine().Length <= 76);
        }

        [Fact]
        public void EncodeDoesNotEncodeTabsOrSpacesIfSpecified()
        {
            var text = "!This Is A Basic\tTest^";
            var expected = text.Replace(" ", "=20").Replace("\t", "=09");

            var actual = new QuotedPrintable(DoNotEncode.Tab & DoNotEncode.Space).Encode(Encoding.ASCII.GetBytes(text));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EncodePutsEqualsAtEndIfLastCharacterIsUnencodedSpace()
        {
            var text = "Line Ends With ";

            var actual = new QuotedPrintable(DoNotEncode.Space).Encode(Encoding.ASCII.GetBytes(text));

            Assert.Equal(text + "=", actual);
        }

        [Fact]
        public void EncodeDoesNotPutsEqualsAtEndIfLastCharacterEncoded()
        {
            var text = "Line Ends With ";
            var expected = text.Replace(" ", "=20");

            var actual = new QuotedPrintable().Encode(Encoding.ASCII.GetBytes(text));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EncodePutsEqualsAtEndIfLastCharacterIsUnencodedTab()
        {
            var text = "LineEndsWith\t";

            var actual = new QuotedPrintable(DoNotEncode.Tab).Encode(Encoding.ASCII.GetBytes(text));

            Assert.Equal(text + "=", actual);
        }

        [Fact]
        public void EncodeEncodesEquals()
        {
            var text = "This=Is=A=Test";
            var expected = text.Replace("=", "=3D");

            var actual = new QuotedPrintable().Encode(Encoding.ASCII.GetBytes(text));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EncodeGivenBinaryEncodesBinary()
        {
            var unencoded = new byte[] { 0x10, 0x44, 0x00, 0xA3, 0xFF, 0xDE, 0x4E };
            var expected = "=10D=00=A3=FF=DEN";

            var actual = new QuotedPrintable().Encode(unencoded);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecodeGivenAsciiReturnsAsciiBytes()
        {
            var text = "This is a cool test";
            var expected = Encoding.ASCII.GetBytes(text);

            var actual = new QuotedPrintable().Decode(text);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecodeGivenMixReturnsDecodedBytes()
        {
            var text = "^Aa =20\t=09!";
            var expected = new byte[] { 0x5E, 0x41, 0x61, 0x20, 0x20, 0x09, 0x09, 0x21 };

            var actual = new QuotedPrintable().Decode(text);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecodeIgnoresTrailingEquals()
        {
            var text = " A =";
            var expected = new byte[] { 0x20, 0x41, 0x20 };

            var actual = new QuotedPrintable().Decode(text);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecodeThrowsArgumentOutOfRangeIfQuotedEndsBeforeCompletion()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new QuotedPrintable().Decode("Dam=2"));
        }

        [Fact]
        public void DecodeThrowsArgumentOutOfRangeIfNotHex()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new QuotedPrintable().Decode("Dam=4G"));
        }
    }
}