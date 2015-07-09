// Copyright (c) Damien Guard.  All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

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
        readonly bool encodeSpaces = true;
        readonly bool encodeTabs = true;
        const int maxLineLength = 76;

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

        public override byte[] Decode(string input)
        {
            var output = new List<Byte>();
            for (var textIndex = 0; textIndex < input.Length; textIndex++)
            {
                var t = input[textIndex];
                if (t == '=')
                {
                    textIndex++;
                    switch (input.Length - textIndex)
                    {
                        case 1:
                            throw new ArgumentOutOfRangeException("input", "Only one character found after = sign - is data truncated?");

                        case 0:
                            break;

                        default:
                            output.Add((byte)((Hex(input[textIndex++]) << 4) + Hex(input[textIndex])));
                            break;
                    }
                }
                else
                    output.Add((byte)t);
            }

            return output.ToArray();
        }

        bool ShouldEncode(byte b)
        {
            if (b == ' ')
                return encodeSpaces;
            if (b == '\t')
                return encodeTabs;

            return (b < 33 || b > 126 || b == '=');
        }

        int Hex(char a)
        {
            if (a >= '0' && a <= '9')
                return a - '0';

            if (a >= 'a' && a <= 'f')
                return a - 'a' + 10;

            if (a >= 'A' && a <= 'F')
                return a - 'A' + 10;

            throw new ArgumentOutOfRangeException("a", String.Format("Character {0} is not hexadecimal", a));
        }
    }
}