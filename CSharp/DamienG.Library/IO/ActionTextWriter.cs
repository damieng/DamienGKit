// Copyright (c) Damien Guard.  All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Originally published at http://damieng.com/blog/2008/07/30/linq-to-sql-log-to-debug-window-file-memory-or-multiple-writers

using System;
using System.IO;
using System.Text;

namespace DamienG.IO
{
    /// <summary>
    /// A <see cref="TextWriter"/> that delegates the writing to an <see cref="Action"/> that takes a string.
    /// </summary>
    public class ActionTextWriter : TextWriter
    {
        readonly Action<string> action;

        /// <summary>
        /// Create a new instance of <see cref="ActionTextWriter"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{String}"/> to call every time text should be written.</param>
        public ActionTextWriter(Action<string> action)
        {
            this.action = action;
        }

        /// <inheritdoc/>
        public override Encoding Encoding => Encoding.Default;

        /// <inheritdoc/>
        public override void Write(char[] buffer, int index, int count) => Write(new string(buffer, index, count));

        /// <inheritdoc/>
        public override void Write(string value) => action(value);
    }
}