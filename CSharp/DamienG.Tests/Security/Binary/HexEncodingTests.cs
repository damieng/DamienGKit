using System;
using DamienG.System.Binary;
using Xunit;

namespace DamienG.Tests.Security.Binary
{
    public class HexEncodingTests
    {
        [Fact]
        public void EncodeReturnsEncodedString()
        {
            var unencoded = new byte[] { 0x10, 0x44, 0x00, 0xA3, 0xFF, 0xDE, 0x4E };
            var expected = "104400A3FFDE4E";

            var actual = new HexEncoding().Encode(unencoded);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecodeGivenUppercaseHexReturnsDecodedBytes()
        {
            var expected = new byte[] { 0x10, 0x44, 0x00, 0xA3, 0xFF, 0xDE, 0x4E };
            var text = "104400A3FFDE4E";

            var actual = new HexEncoding().Decode(text);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecodeGivenLowercaseHexReturnsDecodedBytes()
        {
            var expected = new byte[] { 0x00, 0x44, 0xA3, 0xDE, 0x4E, 0xFF };
            var text = "0044a3de4eff";

            var actual = new HexEncoding().Decode(text);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecodeGivenMixedcaseHexReturnsDecodedBytes()
        {
            var expected = new byte[] { 0x44, 0xA3, 0xDE, 0x4E, 0xFF, 0x00 };
            var text = "44a3De4eFF00";

            var actual = new HexEncoding().Decode(text);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecodeThrowsArgumentOutOfRangeIfUnevenLength()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new HexEncoding().Decode("ED13A"));
        }

        [Fact]
        public void DecodeThrowsArgumentOutOfRangeIfNotHex()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new HexEncoding().Decode("ED13G2"));
        }
    }
}