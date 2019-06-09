using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McFuncCompiler.Command;
using McFuncCompiler.Command.Tokens;

namespace McFuncCompiler.Parser.ParseFilters
{
    public class DefineConstantParseFilter : IParseFilter
    {
        public Command.Command Filter(Command.Command command)
        {
            if (command.GetCommandName() != "define" || command.Arguments.Count < 3)
                return command;
            
            string constName = command.Arguments[1].GetAsText();

            // Constant value
            var argument = new Argument();
            for (var i = 2; i < command.Arguments.Count; i++)
            {
                foreach (var token in command.Arguments[i].Tokens)
                    argument.Tokens.Add(token);
                argument.Tokens.Add(new TextToken(" "));
            }
            
            var defineConstToken = new DefineConstantToken(constName, argument);
            return new Command.Command(new List<Argument> {new Argument(defineConstToken) });
        }

        public Argument FilterArgument(Argument argument)
        {
            return argument;
        }
    }
}
