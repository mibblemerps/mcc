using System.Collections.Generic;
using mcc.Command;
using mcc.Command.Tokens;

namespace mcc.Parser.ParseFilters
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
