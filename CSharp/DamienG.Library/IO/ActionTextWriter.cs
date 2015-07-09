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
    /// A TextWriter that can delegate the writing to any Action that takes a string.
    /// </summary>
    public class ActionTextWriter : TextWriter
    {
        readonly Action<string> action;

        public ActionTextWriter(Action<string> action)
        {
            this.action = action;
        }

        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }

        public override void Write(char[] buffer, int index, int count)
        {
            Write(new string(buffer, index, count));
        }

        public override void Write(string value)
        {
            action(value);
        }
    }
}