using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace McFuncCompiler.Command.CustomCommands
{
    public class SetVariable : ICustomCommand
    {
        public bool CompilerOnly => false;
        
        private static string VarRegex = @"^([\w\d-_]*)\$([^\s=]+)$";

        public bool DoesApply(BuildEnvironment env, Command command)
        {
            return Regex.IsMatch(command.GetCommandName(), VarRegex);
        }

        public ApplyResult Apply(BuildEnvironment env, Command command)
        {
            if (command.Arguments.Count != 3)
                throw new Exception("Invalid usage of variable: Invalid argument count");

            Match match = Regex.Match(command.Arguments[0].Compile(env), VarRegex);
            string varScoreboard = match.Groups[1].Value;
            string varName = match.Groups[2].Value;
            string varOperand = command.Arguments[1].Compile(env);
;
            if (!int.TryParse(command.Arguments[2].Compile(env), out int varValue))
                throw new Exception("Invalid usage of variable: Value not an integer!");

            // If no scoreboard given, use the default global one specified in the environment constants.
            if (varScoreboard.Trim() == "")
                varScoreboard = env.Constants["globals_scoreboard"];

            var commands = new List<Command>();

            switch (varOperand)
            {
                case "=":
                    commands.Add(new Command("scoreboard", "players", "set", varName, varScoreboard, varValue.ToString()));
                    break;

                case "-=":
                    varValue = -varValue;
                    goto case "+=";
                case "+=":
                    if (varValue > 0)
                        commands.Add(new Command("scoreboard", "players", "add", varName, varScoreboard, Math.Abs(varValue).ToString()));
                    else if (varValue < 0)
                        commands.Add(new Command("scoreboard", "players", "remove", varName, varScoreboard, Math.Abs(varValue).ToString()));
                    
                    break;

                default:
                    throw new Exception("Invalid usage of variable: Unknown operand");
            }

            return new ApplyResult(true, commands);
        }
    }
}
