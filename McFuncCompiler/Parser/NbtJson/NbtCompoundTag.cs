using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFuncCompiler.Parser.NbtJson
{
    public class NbtCompoundTag : Dictionary<string, object>
    {
        public NbtString GetString(string key)
        {
            return (NbtString) this[key];
        }

        public NbtNumber<long> GetLong(string key)
        {
            return (NbtNumber<long>) this[key];
        }

        public NbtNumber<int> GetInt(string key)
        {
            return (NbtNumber<int>)this[key];
        }

        public NbtNumber<short> GetShort(string key)
        {
            return (NbtNumber<short>)this[key];
        }

        public NbtNumber<byte> GetByte(string key)
        {
            return (NbtNumber<byte>)this[key];
        }

        public NbtNumber<float> GetFloat(string key)
        {
            return (NbtNumber<float>)this[key];
        }

        public NbtNumber<double> GetDouble(string key)
        {
            return (NbtNumber<double>)this[key];
        }

        public NbtCompoundTag GetTag(string key)
        {
            return (NbtCompoundTag) this[key];
        }

        public override string ToString()
        {
            var builder = new StringBuilder("{");

            bool first = true;

            foreach (KeyValuePair<string, object> child in this)
            {
                if (!first) // Append seperator
                    builder.Append(",");
                first = false;

                // Append key - convert to an NbtString as this will output a safe quoted string if necessary
                builder.Append(new NbtString(child.Key) + ":");

                // Append value
                builder.Append(child.Value);
            }

            builder.Append("}");

            return builder.ToString();
        }
    }
}
