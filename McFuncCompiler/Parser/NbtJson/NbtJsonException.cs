using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFuncCompiler.Parser.NbtJson
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
