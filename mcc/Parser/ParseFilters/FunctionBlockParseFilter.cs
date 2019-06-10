using System.Linq;
using mcc.Command;
using mcc.Command.Tokens;

namespace mcc.Parser.ParseFilters
{
    public class FunctionBlockParseFilter : IParseFilter
    {
        public Command.Command Filter(Command.Command command)
        {
            command = FilterForStart(command);
            command = FilterForEnd(command);

            return command;
        }

        public Argument FilterArgument(Argument argument)
        {
            return argument;
        }

        private Command.Command FilterForStart(Command.Command command)
        {
            string lastArg = command.Arguments.Last().GetAsText();
            if (lastArg == null)
                return command;

            if (lastArg != "(")
                return command;

            // Start of function block
            // Remove start function block syntax
            command.Arguments.RemoveAt(command.Arguments.Count - 1);

            // Add start function block argument
            var argument = new Argument(new OpenFunctionBlockToken());
            command.Arguments.Add(argument);

            return command;
        }

        private Command.Command FilterForEnd(Command.Command command)
        {
            // Requires exactly one argument (the closing bracket)
            if (command.Arguments.Count != 1)
                return command;

            string firstArg = command.Arguments.First().GetAsText();
            if (firstArg == null)
                return command;

            if (firstArg != ")")
                return command;

            // Closing of function block
            // Remove close function block syntax argument
            command.Arguments.RemoveAt(0);

            // Add close function block argument
            command.Arguments.Add(new Argument(new CloseFunctionBlockToken()));

            return command;
        }
    }
}
