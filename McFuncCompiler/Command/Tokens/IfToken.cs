using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McFuncCompiler.Command.CustomCommands;
using McFuncCompiler.Parser.ParseFilters.If;

namespace McFuncCompiler.Command.Tokens
{
    public class IfToken : IToken
    {
        public List<Condition> Conditions;

        public IfToken(List<Condition> conditions)
        {
            Conditions = conditions;
        }

        public string Compile(BuildEnvironment env)
        {
            // Build the execute command
            StringBuilder builder = new StringBuilder("execute");

            // Build conditions
            foreach (Condition condition in Conditions)
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

            return replacement.Compile(env);
        }
    }
}
