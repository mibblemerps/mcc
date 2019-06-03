using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McFuncCompiler.Command.Tokens;

namespace McFuncCompiler.Command
{
    public class Argument
    {
        public List<IToken> Tokens = new List<IToken>();

        public string Compile(BuildEnvironment env)
        {
            StringBuilder builder = new StringBuilder();
            foreach (IToken token in Tokens)
                builder.Append(token.Compile(env));

            return builder.ToString();
        }
    }
}
