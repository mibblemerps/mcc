using System;
using System.IO;
using McFuncCompiler.Parser.NbtJson;

namespace McFuncCompiler.Command.Tokens
{
    public class JsonImportToken : IToken
    {
        public string Id { get; set; }

        public JsonImportToken(string id)
        {
            Id = id;
        }

        public string Compile(BuildEnvironment env)
        {
            string path = env.GetPath(Id, "json");
            string file = File.ReadAllText(path);

            if (path.EndsWith(".json") || path.EndsWith(".nbt"))
            {
                // Treat as NBT JSON
                // Parse and recompile to put onto one line and ensure formatting is correct
                //try
                {
                    object nbt = NbtJsonParser.Parse(file);
                    string compiledNbt = nbt.ToString();

                    Logger.Debug($"Sucessfully parsed {Id} as NBT.");

                    return compiledNbt;
                }
//                catch (Exception e)
//                {
//                    Logger.Warning($"Resource {Id} isn't valid NBT! " + e.Message);
//                }
            }
            
            return file;
        }
    }
}
