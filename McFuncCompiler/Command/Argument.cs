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

        public Argument()
        {
        }

        public Argument(string arg)
        {
            Tokens.Add(new TextToken(arg));
        }

        public string Compile(BuildEnvironment env)
        {
            StringBuilder builder = new StringBuilder();
            foreach (IToken token in Tokens)
                builder.Append(token.Compile(env));

            return builder.ToString();
        }

        /// <summary>
        /// Get this argument a simple text form, if possible.<br />
        /// For arguments with only text tokens, this will have the same output as Compile.
        /// </summary>
        public string GetAsText()
        {
            // All tokens must be simple text tokens
            if (Tokens.Any(token => !(token is TextToken)))
                return null;

            return Compile(null);
        }
    }
}
