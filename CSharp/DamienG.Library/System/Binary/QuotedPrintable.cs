using System;
using System.Collections.Generic;
using System.Text;

namespace DamienG.System.Binary
{
    [Flags]
    public enum DoNotEncode
    {
        Tab = 0x01,
        Space = 0x02
    }

    public class QuotedPrintable : BinaryTextEncoding
    {
        private readonly bool encodeSpaces = true;
        private readonly bool encodeTabs = true;
        private const int maxLineLength = 76;

        public QuotedPrintable()
        {
        }

        public QuotedPrintable(DoNotEncode doNotEncode)
        {
            encodeTabs = !doNotEncode.HasFlag(DoNotEncode.Tab);
            encodeSpaces = !doNotEncode.HasFlag(DoNotEncode.Space);
        }

        public override string Encode(byte[] bytes)
        {
            var sb = new StringBuilder();

            var column = 0;
            string nextOutput = null;

            Action checkForEndOfLine = () =>
            {
                if (column + nextOutput.Length >= maxLineLength)
                {
                    sb.Append("=\n");
                    column = 0;
                }
            };

            foreach (var b in bytes)
            {
                nextOutput = ShouldEncode(b) ? "=" + b.ToString("X2") : Char.ToString((char)b);

                checkForEndOfLine();
                sb.Append(nextOutput);
                column += nextOutput.Length;
            }

            if (nextOutput.Length == 1 && nextOutput[0] <= 32)
                sb.Append("=");

            return sb.ToString();
        }

        public override byte[] Decode(string text)
        {
            var output = new List<Byte>();
            for (var textIndex = 0; textIndex < text.Length; textIndex++)
            {
                var t = text[textIndex];
                if (t == '=')
                {
                    textIndex++;
                    switch (text.Length - textIndex)
                    {
                        case 1:
                            throw new ArgumentOutOfRangeException("text", "Only one character found after = sign - is data truncated?");

                        case 0:
                            break;

                        default:
                            output.Add((byte)((Hex(text[textIndex++]) << 4) + Hex(text[textIndex])));
                            break;
                    }
                }
                else
                    output.Add((byte)t);
            }

            return output.ToArray();
        }

        private bool ShouldEncode(byte b)
        {
            if (b == ' ')
                return encodeSpaces;
            if (b == '\t')
                return encodeTabs;

            return (b < 33 || b > 126 || b == '=');
        }

        private int Hex(char a)
        {
            if (a >= '0' && a <= '9')
                return a - '0';

            if (a >= 'a' && a <= 'f')
                return a - 'a' + 10;

            if (a >= 'A' && a <= 'F')
                return a - 'A' + 10;

            throw new ArgumentOutOfRangeException("text", String.Format("Character {0} is not hexadecimal", a));
        }
    }
}