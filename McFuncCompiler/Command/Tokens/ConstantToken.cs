using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFuncCompiler.Command.Tokens
{
    public class ConstantToken : IToken
    {
        public string Constant { get; set; }

        public ConstantToken(string constant)
        {
            Constant = constant;
        }

        public string Compile(BuildEnvironment env)
        {
            if (!env.Constants.ContainsKey(Constant))
            {
                return $"<Unknown constant: {Constant}>";
            }

            return env.Constants[Constant];
        }
    }
}
