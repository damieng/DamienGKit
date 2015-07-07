using System.IO;
using System.Text;
using DamienG.IO;
using Xunit;

namespace DamienG.Tests.IO
{
    public class LineTrackingStreamReaderTests
    {
        private static Stream MakeMemoryStream(string contents)
        {
            var memoryStream = new MemoryStream(contents.Length);
            var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
            streamWriter.Write(contents);
            streamWriter.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        [Fact]
        public void InitializedLineAndPositionAreZero()
        {
            var stream = MakeMemoryStream("abcdefghijklmnopqrstuvwxyz");
            var tracking = new LineTrackingStreamReader(stream);

            Assert.Equal(0, tracking.LineNumber);
            Assert.Equal(0, tracking.CharacterPosition);
        }

        [Fact]
        public void ReadLineSetsInitialLineAndPosition()
        {
            const string expectedString = "12345";
            var stream = MakeMemoryStream(expectedString);
            var tracking = new LineTrackingStreamReader(stream);

            var actualString = tracking.ReadLine();

            Assert.Equal(1, tracking.LineNumber);
            Assert.Equal(5, tracking.CharacterPosition);
            Assert.Equal(expectedString, actualString);
        }

        [Fact]
        public void ReadSetsInitialLineAndPosition()
        {
            var stream = MakeMemoryStream("1");
            var tracking = new LineTrackingStreamReader(stream);
            tracking.Read();

            Assert.Equal(1, tracking.LineNumber);
            Assert.Equal(1, tracking.CharacterPosition);
        }
    }
}