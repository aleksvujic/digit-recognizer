﻿using System;
using System.IO;
using DigitRecognizer.Core.Utilities;

namespace DigitRecognizer.Core.InputOutput
{
    /// <summary>
    /// 
    /// </summary>
    public class NnBinaryAdapter : INnBinaryAdapter
    {
        /// <summary>
        /// 
        /// </summary>
        private const string NnFileExtension = ".nn";
        
        /// <summary>
        /// The internal <see cref="System.IO.FileStream"/> object.
        /// </summary>
        protected readonly FileStream FileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="NnBinaryAdapter"/> class.
        /// </summary>
        /// <param name="filename">The name of the file, used for opening a file stream.</param>
        /// <param name="fileMode">The file acces mode of the adapter.</param>
        public NnBinaryAdapter(string filename, FileMode fileMode)
        {
            Contracts.StringNotNullOrEmpty(filename, nameof(filename));
            Contracts.FileHasExtension(filename, nameof(filename));
            Contracts.FileExtensionValid(filename, NnFileExtension, nameof(filename));
            Contracts.FileExists(filename, nameof(filename));

            FileStream = File.Open(filename, fileMode, FileAccess.ReadWrite);
        }

        /// <summary>
        /// Indicates if the disposing operation has been completed or not.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Releases all resources used by the <see cref="NnBinaryAdapter"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the <see cref="NnBinaryAdapter"/>.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                FileStream.Dispose();
            }

            _disposed = true;
        }
    }
}
