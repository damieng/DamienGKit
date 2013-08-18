using System.IO;
using System.Text;
using DamienG.IO;
using NUnit.Framework;

namespace DamienG.Tests.IO
{
    [TestFixture]
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

        [Test]
        public void InitializedLineAndPositionAreZero()
        {
            var stream = MakeMemoryStream("abcdefghijklmnopqrstuvwxyz");
            var tracking = new LineTrackingStreamReader(stream);

            Assert.AreEqual(0, tracking.LineNumber);
            Assert.AreEqual(0, tracking.CharacterPosition);
        }

        [Test]
        public void ReadLineSetsInitialLineAndPosition()
        {
            const string expectedString = "12345";
            var stream = MakeMemoryStream(expectedString);
            var tracking = new LineTrackingStreamReader(stream);

            var actualString = tracking.ReadLine();

            Assert.AreEqual(1, tracking.LineNumber);
            Assert.AreEqual(5, tracking.CharacterPosition);
            Assert.AreEqual(expectedString, actualString);
        }

        [Test]
        public void ReadSetsInitialLineAndPosition()
        {
            var stream = MakeMemoryStream("1");
            var tracking = new LineTrackingStreamReader(stream);
            tracking.Read();

            Assert.AreEqual(1, tracking.LineNumber);
            Assert.AreEqual(1, tracking.CharacterPosition);
        }
    }
}