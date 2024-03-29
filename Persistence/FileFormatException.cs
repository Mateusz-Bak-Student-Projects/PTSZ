﻿using System;

namespace Persistence
{
    public class FileFormatException : Exception
    {
        public FileFormatException() { }

        public FileFormatException(string message) : base(message) { }

        public FileFormatException(string message, Exception innerException) : base(message, innerException) { }
    }
}
