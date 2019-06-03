using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFuncCompiler.Command.Tokens
{
    public class DefineConstantToken : IToken
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public DefineConstantToken(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Compile(BuildEnvironment env)
        {
            env.Constants[Name] = Value;
            return "";
        }
    }
}
