// Copyright (c) Damien Guard.  All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

using System;

namespace DamienG.System.Binary
{
    public class HexEncoding : BinaryTextEncoding
    {
        public override string Encode(byte[] bytes)
        {
            var output = new char[bytes.Length * 2];
            var outputIndex = 0;
            for (var byteIndex = 0; byteIndex < bytes.Length; byteIndex++)
            {
                var hex = bytes[byteIndex].ToString("X2");
                output[outputIndex++] = hex[0];
                output[outputIndex++] = hex[1];
            }
            return new string(output);
        }

        public override byte[] Decode(string text)
        {
            if (text.Length % 2 == 1)
                throw new ArgumentOutOfRangeException("text", "Must be an even number of hex digits");

            var output = new byte[text.Length / 2];
            var textIndex = 0;
            for (var outputIndex = 0; outputIndex < output.Length; outputIndex++)
            {
                var b = (byte)((Hex(text[textIndex++]) << 4) + Hex(text[textIndex++]));
                output[outputIndex] = b;
            }
            return output;
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