// Copyright (c) Damien Guard.  All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

namespace DamienG.System.Binary
{
    public abstract class BinaryTextEncoding
    {
        public abstract string Encode(byte[] bytes);

        public abstract byte[] Decode(string input);
    }
}