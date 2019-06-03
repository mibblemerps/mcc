using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFuncCompiler.Command.Tokens
{
    public class TextToken : IToken
    {
        public string Text { get; set; }

        public TextToken(string text)
        {
            Text = text;
        }

        public string Compile(BuildEnvironment env)
        {
            return Text;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
