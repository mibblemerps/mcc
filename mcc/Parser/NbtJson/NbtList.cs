using System;
using System.Collections.Generic;
using System.Text;

namespace mcc.Parser.NbtJson
{
    public class NbtList : List<object>
    {
        public Type Type { get; }

        public NbtList(Type type = null)
        {
            Type = type;
        }

        public NbtList(IEnumerable<object> collection, Type type = null) : base(collection)
        {
            Type = type;
        }

        public override string ToString()
        {
            var builder = new StringBuilder("[");

            if (Type != null)
            {
                // Non-generic list (array)
                if (Type == typeof(byte)) builder.Append("B");
                if (Type == typeof(int)) builder.Append("I");
                if (Type == typeof(long)) builder.Append("L");

                builder.Append(";");
            }

            // Build list
            builder.Append(string.Join(",", this) + "]");

            return builder.ToString();
        }
    }
}
