using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFuncCompiler.Command.Tokens
{
    public class CloseFunctionBlockToken : IToken
    {
        public string Compile(BuildEnvironment env)
        {
            return "";
        }
    }
}
