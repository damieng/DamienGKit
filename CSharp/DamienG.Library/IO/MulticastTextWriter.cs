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
    /// A TextWriter that can multicast output to other TextWriters.
    /// </summary>
    public class MulticastTextWriter : TextWriter
    {
        readonly IList<TextWriter> textWriters;

        public MulticastTextWriter()
            : this(new List<TextWriter>())
        {
        }

        public MulticastTextWriter(IList<TextWriter> textWriters)
        {
            this.textWriters = textWriters;
        }

        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }

        public void Add(TextWriter textWriter)
        {
            lock (textWriters)
                textWriters.Add(textWriter);
        }

        public bool Remove(TextWriter textWriter)
        {
            lock (textWriters)
                return textWriters.Remove(textWriter);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            lock (textWriters)
                foreach (var textWriter in textWriters)
                    textWriter.Write(buffer, index, count);
        }
    }
}