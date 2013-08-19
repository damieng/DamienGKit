using System.Collections.Generic;
using System.IO;

namespace DamienG.IO
{
    /// <summary>
    /// A StreamReader that adds line number and character position tracking.
    /// </summary>
    public class LineTrackingStreamReader : StreamReader
    {
        public LineTrackingStreamReader(Stream stream)
            : base(stream)
        {
            LineNumber = 0;
            CharacterPosition = 0;
        }

        public int CharacterPosition { get; private set; }

        public int LineNumber { get; private set; }

        public override string ReadLine()
        {
            var readLine = base.ReadLine();
            if (readLine != null)
            {
                LineNumber++;
                CharacterPosition = readLine.Length;
            }
            return readLine;
        }

        public override int Read()
        {
            var read = base.Read();
            if (read != -1)
            {
                if (LineNumber == 0)
                    LineNumber++;
                var c = (char) read;
                if (c == '\n')
                {
                    LineNumber++;
                    CharacterPosition = 0;
                }
                else
                    CharacterPosition++;
            }
            return read;
        }

        public override int ReadBlock(char[] buffer, int index, int count)
        {
            var readBlock = base.ReadBlock(buffer, index, count);
            if (LineNumber == 0)
                LineNumber++;
            TrackPosition(buffer, index, count);
            return readBlock;
        }

        public override int Read(char[] buffer, int index, int count)
        {
            var read = base.Read(buffer, index, count);
            if (LineNumber == 0)
                LineNumber++;
            TrackPosition(buffer, index, count);
            return read;
        }

        private void TrackPosition(IList<char> buffer, int index, int count)
        {
            for (var i = index; i < count; i++)
                if (buffer[i] == '\n')
                {
                    LineNumber++;
                    CharacterPosition = 0;
                }
                else
                    CharacterPosition++;
        }
    }
}