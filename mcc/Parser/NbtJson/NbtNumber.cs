using System;

namespace mcc.Parser.NbtJson
{
    public class NbtNumber<T>
    {
        public T Value { get; }

        public Type Type { get; }

        public NbtNumber(T value)
        {
            Value = value;
            Type = typeof(T);
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
