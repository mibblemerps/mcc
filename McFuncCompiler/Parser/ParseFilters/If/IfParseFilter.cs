using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using McFuncCompiler.Command;
using McFuncCompiler.Command.Tokens;

namespace McFuncCompiler.Parser.ParseFilters.If
{
    // if $var1 = 10.. (
    // if not $var1 = 10.. (
    // if $var1 = $var2 (
    // if $var1 > $var2 (

    public class IfParseFilter : IParseFilter
    {
        private static readonly IReadOnlyList<string> Operators = new List<string>
        {
            "<", "<=", "=", ">", ">="
        };

        public Command.Command Filter(Command.Command command)
        {
            if (command.GetCommandName() != "if")
                return command;

            // Split if statement into individual conditions by seperating them at "&&"
            List<List<Argument>> ifConditionArguments = new List<List<Argument>> { new List<Argument>() };
            int pieceIndex = 0;
            foreach (Argument argument in command.Arguments)
            {
                string argText = argument.GetAsText();

                if (argText == "&&")
                {
                    ifConditionArguments.Add(new List<Argument>());
                    pieceIndex++;
                }
                else
                {
                    ifConditionArguments[pieceIndex].Add(new Argument(argText));
                }
            }

            List<Condition> conditions = new List<Condition>();

            foreach (var arguments in ifConditionArguments)
            {
                // Matches condition
                Condition condition = MatchesCondition.Parse(arguments);

                // Operation condition
                if (condition == null)
                    condition = OperationCondition.Parse(arguments);

                if (condition == null)
                    throw new Exception("Invalid if statement condition");
                else
                    conditions.Add(condition);
            }

            // Get all the left over arguments; these will make up the command that will be ran if the conditions are met
            int startIndex = conditions.Last().LastIndex + 1;
            List<Argument> leftOver = new List<Argument>(command.Arguments.GetRange(startIndex, command.Arguments.Count - startIndex));
            Command.Command successCommand = new Command.Command(leftOver);
            
            // Replace existing command with an if token
            command.Arguments.Clear();
            command.Arguments.Add(new Argument(new IfToken(conditions, successCommand)));

            return command;
        }

        public Argument FilterArgument(Argument argument)
        {
            return argument;
        }
    }
}
