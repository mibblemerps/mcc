using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFuncCompiler.Parser.NbtJson
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
            var builder = new StringBuilder();

            if (Type == typeof(object))
            {
                // Non-generic list (array)
                builder.Append("[");

                if (Type == typeof(byte)) builder.Append("B");
                if (Type == typeof(int)) builder.Append("I");
                if (Type == typeof(long)) builder.Append("L");

                builder.Append(";");
            }

            // Build list
            builder.Append("[" + string.Join(",", this) + "]");

            return builder.ToString();
        }
    }
}
