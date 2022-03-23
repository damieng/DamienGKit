// Copyright (c) Damien Guard.  All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Originally published at http://damieng.com/blog/2008/07/30/linq-to-sql-log-to-debug-window-file-memory-or-multiple-writers

using System.Collections.Generic;
using System.IO;

namespace DamienG.IO
{
    /// <summary>
    /// A <see cref="StreamReader"/> that tracks the line number and character position as it is read.
    /// </summary>
    public class LineTrackingStreamReader : StreamReader
    {
        /// <summary>
        /// Create a new instance of <seealso cref="LineTrackingStreamReader"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to track line reading on.</param>
        public LineTrackingStreamReader(Stream stream)
            : base(stream)
        {
            LineNumber = 0;
            CharacterPosition = 0;
        }

        /// <summary>
        /// The current character position within the line.
        /// </summary>
        public int CharacterPosition { get; private set; }

        /// <summary>
        /// The current line number.
        /// </summary>
        public int LineNumber { get; private set; }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override int ReadBlock(char[] buffer, int index, int count)
        {
            var readBlock = base.ReadBlock(buffer, index, count);
            if (LineNumber == 0)
                LineNumber++;
            TrackPosition(buffer, index, count);
            return readBlock;
        }

        /// <inheritdoc/>
        public override int Read(char[] buffer, int index, int count)
        {
            var read = base.Read(buffer, index, count);
            if (LineNumber == 0)
                LineNumber++;
            TrackPosition(buffer, index, count);
            return read;
        }

        /// <inheritdoc/>
        void TrackPosition(IList<char> buffer, int index, int count)
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