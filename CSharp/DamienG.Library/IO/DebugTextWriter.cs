// Copyright (c) Damien Guard.  All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Originally published at http://damieng.com/blog/2008/07/30/linq-to-sql-log-to-debug-window-file-memory-or-multiple-writers

using System.Diagnostics;
using System.IO;
using System.Text;

namespace DamienG.IO
{
    /// <summary>
    /// A <see cref="TextWriter"/> that writes to the <see cref="Debug"/> window.
    /// </summary>
    public class DebugTextWriter : TextWriter
    {
        /// <inheritdoc/>
        public override Encoding Encoding => Encoding.Default;

        /// <inheritdoc/>
        public override void Write(char[] buffer, int index, int count) => Debug.Write(new string(buffer, index, count));

        /// <inheritdoc/>
        public override void Write(string value) => Debug.Write(value);
    }
}