﻿using System;
using System.Collections.Generic;
using System.IO;
using DigitRecognizer.Core.Extensions;
using DigitRecognizer.Core.Utilities;

namespace DigitRecognizer.Core.InputOutput
{
    /// <summary>
    /// Provides methods for working with an in-memory stream of data.
    /// </summary>
    public class MemoryStreamReader : IMemoryStreamReader<byte>, IDisposable
    {
        /// <summary>
        /// The offset (if it exsists) in bytes, that need to be skipped at the beginning of the stream. 
        /// </summary>
        private readonly int _offset;
        
        /// <summary>
        /// The in-memory stream of data.
        /// </summary>
        private readonly MemoryStream _memoryStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStreamReader"/> class, from the specified file.
        /// </summary>
        /// <param name="filePath">The file path used for instantiating a stream.</param>
        /// <param name="offset">The magic number.</param>
        public MemoryStreamReader(string filePath, int offset) : this()
        {
            Contracts.ValueGreaterThanZero(offset, nameof(offset));

            _memoryStream = StreamExtensions.GetMemoryStreamFromFile(filePath);

            _offset = offset;

            if (_memoryStream.Position != 0)
            {
                Reset();
            }
        }

        /// <summary>
        /// Initialies a new instance of the <see cref="MemoryStreamReader"/> class.
        /// </summary>
        public MemoryStreamReader()
        {
            _memoryStream = new MemoryStream();
            _offset = 0;
        }

        /// <summary>
        /// Reads the specified ammount of bytes from the stream.
        /// </summary>
        /// <param name="count">The number of elements to read from the stream.</param>
        /// <returns>A byte array.</returns>
        public byte[] Read(int count)
        {
            Contracts.ValueGreaterThanZero(count, nameof(count));
            CheckForOverflow();

            var result = new byte[count];

            for (var i = 0; i < count; i++)
            {
                var byteVal = (byte) _memoryStream.ReadByte();

                result[i] = byteVal;
            }

            return result;
        }

        /// <summary>
        /// Reads the specified ammount of bytes from the stream, of the specified length.
        /// </summary>
        /// <param name="count">The number of elements to read from the stream.</param>
        /// <param name="length">The length of each element.</param>
        /// <returns>A list of byte arrays.</returns>
        public byte[][] Read(int count, int length)
        {
            Contracts.ValueGreaterThanZero(count, nameof(count));
            Contracts.ValueGreaterThanZero(length, nameof(length));
            CheckForOverflow();

            var result = new List<byte[]>();

            for (var i = 0; i < count; i++)
            {
                var byteArray = _memoryStream.ReadBytes(length);
                
                result.Add(byteArray);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Resets the in-memory stream to the starting position for further reading.
        /// </summary>
        public void Reset()
        {
            _memoryStream.SetPosition(_offset);
        }

        /// <summary>
        /// Checks if the current position of the stream has reached the end of the stream, and resets it if true.
        /// </summary>
        private void CheckForOverflow()
        {
            if (_memoryStream.Position == _memoryStream.Length)
            {
                Reset();
            }
        }

        /// <summary>
        /// Indicates if the disposing operation has been completed or not.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Releases all resources used by the <see cref="MemoryStreamReader"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the <see cref="MemoryStreamReader"/>.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _memoryStream?.Dispose();
            }

            _disposed = true;
        }
    }
}