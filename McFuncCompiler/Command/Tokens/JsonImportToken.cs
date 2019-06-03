using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFuncCompiler.Command.Tokens
{
    public class JsonImportToken : IToken
    {
        public string Path { get; set; }

        public JsonImportToken(string path)
        {
            Path = path;
        }

        public string Compile(BuildEnvironment env)
        {
            Path = Path.Replace('\\', '/').TrimStart('/').TrimEnd();

            if (!Path.Substring(Math.Max(Path.IndexOf('/'), 0)).Contains('.'))
            {
                // No file extension, assume .json
                Path += ".json";
            }

            return File.ReadAllText(env.Path + "/" + Path);
        }
    }
}
