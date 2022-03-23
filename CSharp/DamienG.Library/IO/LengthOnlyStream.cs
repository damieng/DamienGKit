// Copyright (c) Damien Guard.  All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Originally published at https://damieng.com/blog/2022/03/22/estimating-json-size/

using System;
using System.IO;

namespace DamienG.IO
{
    /// <summary>
    /// A fake <see cref="Stream"/> that can be used to count the length of content without storing it.
    /// </summary>
    /// <remarks>
    /// This is useful for determining the size before actually streaming
    /// it somewhere like memory or disk where it could be expensive.
    /// </remarks>
    public class LengthOnlyStream : Stream
    {
        long length;

        /// <inheritdoc/>
        public override bool CanRead => false;

        /// <inheritdoc/>
        public override bool CanSeek => false;

        /// <inheritdoc/>
        public override bool CanWrite => true;

        /// <inheritdoc/>
        public override long Length => length;

        /// <inheritdoc/>
        public override long Position { get; set; }

        /// <inheritdoc/>
        public override void Flush() { }

        /// <summary>
        /// Can not read the stream as it does not exist. Will instead throw a <exception cref="NotImplementedException">.
        /// </summary>
        /// <param name="buffer">Ignored.</param>
        /// <param name="offset">Ignored.</param>
        /// <param name="count">Ignored.</param>
        /// <returns>Always throws an exception, do not call this method.</returns>
        /// <exception cref="NotImplementedException">Always.</exception>
        public override int Read(byte[] buffer, int offset, int count) => throw new NotImplementedException();

        /// <summary>
        /// Resets the stream - specifically the length back to zero.
        /// </summary>
        public void Reset() => length = 0;

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin) => 0;

        /// <inheritdoc/>
        public override void SetLength(long value) => length = value;

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count) => length += count - offset;
    }
}
