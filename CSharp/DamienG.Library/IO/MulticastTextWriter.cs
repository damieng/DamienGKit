using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DamienG.IO
{
    public class MulticastTextWriter : TextWriter
    {
        private readonly IList<TextWriter> textWriters;

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