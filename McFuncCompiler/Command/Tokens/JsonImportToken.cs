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
        public string Id { get; set; }

        public JsonImportToken(string id)
        {
            Id = id;
        }

        public string Compile(BuildEnvironment env)
        {
            return File.ReadAllText(env.GetPath(Id, "json"));
        }
    }
}
