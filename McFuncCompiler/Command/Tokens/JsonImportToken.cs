using System;
using System.IO;
using McFuncCompiler.Parser.NbtJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

            if (path.EndsWith(".nbt"))
            {
                // Treat as NBT JSON
                // Parse and recompile to put onto one line and ensure formatting is correct
                try
                {
                    object nbt = NbtJsonParser.Parse(file);
                    string compiledNbt = nbt.ToString();

                    Logger.Debug($"Sucessfully parsed {Id} as NBT.");

                    return compiledNbt;
                }
                catch (Exception e)
                {
                    Logger.Warning($"Resource {Id} isn't valid NBT! " + e.Message);
                }
            }
            else if (path.EndsWith(".json"))
            {
                // Treat as real JSON
                // Parse and recompile to put onto one line and ensure formatting is correct
                try
                {
                    object deserialized = JsonConvert.DeserializeObject(file);

                    string json = JsonConvert.SerializeObject(deserialized, Formatting.None);

                    Logger.Debug($"Sucessfully parsed {Id} as JSON.");

                    return json;
                }
                catch (Exception e)
                {
                    Logger.Warning($"Resource {Id} isn't valid JSON! " + e.Message);
                }
            }

            return file;
        }
    }
}
