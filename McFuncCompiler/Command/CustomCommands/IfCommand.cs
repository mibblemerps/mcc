using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McFuncCompiler.Command.Tokens;
using McFuncCompiler.Parser.ParseFilters.If;

namespace McFuncCompiler.Command.CustomCommands
{
    public class IfCommand : ICustomCommand
    {
        public bool CompilerOnly => false;

        public bool DoesApply(BuildEnvironment env, Command command)
        {
            return command.Arguments[0].Tokens[0] is IfToken;
        }

        public ApplyResult Apply(BuildEnvironment env, Command command)
        {
            IfToken token = command.Arguments[0].Tokens[0] as IfToken;
            if (token == null)
                return new ApplyResult(false);

            // Build the execute command
            StringBuilder builder = new StringBuilder("execute");

            // Build conditions
            foreach (Condition condition in token.Conditions)
            {
                builder.Append(condition.Not ? " unless" : " if");

                if (condition is MatchesCondition matches)
                {
                    matches.LeftVariable.Scoreboard = matches.LeftVariable.Scoreboard ?? env.Constants["globals_scoreboard"];

                    builder.Append($" score {matches.LeftVariable.Name} {matches.LeftVariable.Scoreboard} matches {matches.Matches}");
                }
                else if (condition is OperationCondition operation)
                {
                    operation.LeftVariable.Scoreboard = operation.LeftVariable.Scoreboard ?? env.Constants["globals_scoreboard"];
                    operation.RightVariable.Scoreboard = operation.RightVariable.Scoreboard ?? env.Constants["globals_scoreboard"];

                    builder.Append($" score {operation.LeftVariable.Name} {operation.LeftVariable.Scoreboard} {operation.Operation} {operation.RightVariable.Name} {operation.RightVariable.Scoreboard}");
                }
            }

            Command replacement = new Command(builder.ToString().Split(' '));

            // Add run argument
            replacement.Arguments.Add(new Argument("run"));
            replacement.Arguments.AddRange(token.Command.Arguments);

            return new ApplyResult(true, new List<Command> {replacement});
        }
    }
}
