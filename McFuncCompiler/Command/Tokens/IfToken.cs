using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McFuncCompiler.Parser.ParseFilters.If;

namespace McFuncCompiler.Command.Tokens
{
    public class IfToken : IToken
    {
        public List<Condition> Conditions;
        public Command Command;

        public IfToken(List<Condition> conditions, Command command)
        {
            Conditions = conditions;
            Command = command;
        }

        public string Compile(BuildEnvironment env)
        {
            return "if";
        }
    }
}
