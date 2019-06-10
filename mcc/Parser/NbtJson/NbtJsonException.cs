using System;

namespace mcc.Parser.NbtJson
{
    public class NbtJsonException : Exception
    {
        public NbtJsonException()
        {
        }

        public NbtJsonException(string message) : base(message)
        {
        }

        public NbtJsonException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
