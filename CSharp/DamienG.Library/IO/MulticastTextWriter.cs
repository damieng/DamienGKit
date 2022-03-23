// Copyright (c) Damien Guard.  All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Originally published at http://damieng.com/blog/2008/07/30/linq-to-sql-log-to-debug-window-file-memory-or-multiple-writers

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DamienG.IO
{
    /// <summary>
    /// A <see cref="TextWriter"/> that can multicast output to multiple <see cref="TextWriter"/> instances.
    /// </summary>
    public class MulticastTextWriter : TextWriter
    {
        readonly IList<TextWriter> textWriters;

        /// <summary>
        /// Create a new instance of <see cref="MulticastTextWriter"/>.
        /// </summary>
        public MulticastTextWriter()
            : this(new List<TextWriter>())
        {
        }

        /// <summary>
        /// Create a new instance of <see cref="MulticastTextWriter"/>.
        /// </summary>
        /// <param name="textWriters">The <see cref="TextWriter"/> instances to use.</param>
        public MulticastTextWriter(IList<TextWriter> textWriters)
        {
            this.textWriters = new List<TextWriter>(textWriters);
        }

        /// <inheritdoc/>
        public override Encoding Encoding => Encoding.Default;

        /// <summary>
        /// Add another <see cref="TextWriter"/> to the multiplexor.
        /// </summary>
        /// <param name="textWriter">The <see cref="TextWriter"/> to add.</param>
        public void Add(TextWriter textWriter)
        {
            lock (textWriters)
                textWriters.Add(textWriter);
        }

        /// <summary>
        /// Remove an existing <see cref="TextWriter"/> from the multiplexor.
        /// </summary>
        /// <param name="textWriter">The <see cref="TextWriter"/> to remove.</param>
        public bool Remove(TextWriter textWriter)
        {
            lock (textWriters)
                return textWriters.Remove(textWriter);
        }

        /// <inheritdoc/>
        public override void Write(char[] buffer, int index, int count)
        {
            lock (textWriters)
                foreach (var textWriter in textWriters)
                    textWriter.Write(buffer, index, count);
        }
    }
}