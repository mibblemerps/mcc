using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFuncCompiler.Parser.NbtJson
{
    public class NbtCompoundTag : Dictionary<string, object>
    {
        public long GetLong(string key)
        {
            return (long) this[key];
        }

        public int GetInt(string key)
        {
            return (int) this[key];
        }

        public short GetShort(string key)
        {
            return (short) this[key];
        }

        public byte GetByte(string key)
        {
            return (byte) this[key];
        }

        public float GetFloat(string key)
        {
            return (float) this[key];
        }

        public double GetDouble(string key)
        {
            return (double) this[key];
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

                // Append key
                builder.Append(child.Key + ":");

                // Append value
                builder.Append(child.Value);
            }

            builder.Append("}");

            return builder.ToString();
        }
    }
}
