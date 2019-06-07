using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFuncCompiler.Parser.NbtJson
{
    public class NbtNumber<T>
    {
        public T Value { get; }

        public NbtNumber(T value)
        {
            Value = value;
        }

        public override string ToString()
        {
            string suffix = "";

            if (typeof(T) == typeof(long)) suffix = "l";
            if (typeof(T) == typeof(short)) suffix = "s";
            if (typeof(T) == typeof(byte)) suffix = "b";
            if (typeof(T) == typeof(float)) suffix = "f";
            if (typeof(T) == typeof(double)) suffix = "d";

            return Value.ToString() + suffix;
        }
    }
}
