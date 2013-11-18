using System;
using DamienG.System.Binary;
using NUnit.Framework;

namespace DamienG.Tests.System.Binary
{
    [TestFixture]
    public class HexEncodingTests
    {
        [Test]
        public void EncodeReturnsEncodedString()
        {
            var unencoded = new byte[] { 0x10, 0x44, 0x00, 0xA3, 0xFF, 0xDE, 0x4E };
            var expected = "104400A3FFDE4E";

            var actual = new HexEncoding().Encode(unencoded);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DecodeGivenUppercaseHexReturnsDecodedBytes()
        {
            var expected = new byte[] { 0x10, 0x44, 0x00, 0xA3, 0xFF, 0xDE, 0x4E };
            var text = "104400A3FFDE4E";

            var actual = new HexEncoding().Decode(text);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DecodeGivenLowercaseHexReturnsDecodedBytes()
        {
            var expected = new byte[] { 0x00, 0x44, 0xA3, 0xDE, 0x4E, 0xFF };
            var text = "0044a3de4eff";

            var actual = new HexEncoding().Decode(text);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DecodeGivenMixedcaseHexReturnsDecodedBytes()
        {
            var expected = new byte[] { 0x44, 0xA3, 0xDE, 0x4E, 0xFF, 0x00 };
            var text = "44a3De4eFF00";

            var actual = new HexEncoding().Decode(text);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DecodeThrowsArgumentOutOfRangeIfUnevenLength()
        {
            new HexEncoding().Decode("ED13A");
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DecodeThrowsArgumentOutOfRangeIfNotHex()
        {
            new HexEncoding().Decode("ED13G2");
        }
    }
}