﻿using System.Text;

namespace mcc.Parser.NbtJson
{
    /// <summary>
    /// Converts an object (for example one generated by <see cref="NbtJsonParser"/>) into Minecraft's weird NBT JSON.
    /// </summary>
    public class NbtJsonCompiler
    {
        private object _nbt;

        NbtJsonCompiler(object nbt)
        {
            _nbt = nbt;
        }

        public string Compile(object nbt)
        {
            StringBuilder builder = new StringBuilder();
            


            return builder.ToString();
        }
    }
}
