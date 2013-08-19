using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DamienG.IO
{
    /// <summary>
    /// A TextWriter that writes to the Debug window.
    /// </summary>
    public class DebugTextWriter : TextWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }

        public override void Write(char[] buffer, int index, int count)
        {
            Debug.Write(new String(buffer, index, count));
        }

        public override void Write(string value)
        {
            Debug.Write(value);
        }
    }
}