using System;

namespace Courby.Exception
{
    public class CourbyException : System.Exception
    {
        public CourbyException() : this("") { }
        public CourbyException(System.Exception innerException) : this(innerException.Message, innerException) { }
        public CourbyException(string message) : this (message, new System.Exception(message)) { }
        public CourbyException(string message, System.Exception innerException) : base(message, innerException) { }

    }
}
