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

        public bool DoesApply(BuildEnvironment env, Command command)
        {
            return command.GetCommandName().StartsWith("$") && Regex.IsMatch(command.GetCommandName().Substring(1), @"^[\w\d-_]+$");
        }

        public ApplyResult Apply(BuildEnvironment env, Command command)
        {
            if (command.Arguments.Count != 3)
                throw new Exception("Invalid usage of variable: Invalid argument count");

            string varName = command.GetCommandName().Substring(1);
            string varOperand = command.Arguments[1].Compile(env);
            string varValue = command.Arguments[2].Compile(env);
;
            bool isValueInt = int.TryParse(varValue, out int varValueInt);

            var commands = new List<Command>();

            switch (varOperand)
            {
                case "=":
                    commands.Add(new Command("scoreboard", "players", "set", varName, env.Constants["globals_scoreboard"], varValue));
                    break;

                case "-=":
                    if (!isValueInt)
                        throw new Exception($"Invalid usage of variable: \"{varValue}\" is not a valid integer!");
                    varValueInt = -varValueInt;
                    goto case "+=";
                case "+=":
                    if (!isValueInt)
                        throw new Exception($"Invalid usage of variable: \"{varValue}\" is not a valid integer!");

                    if (varValueInt > 0)
                        commands.Add(new Command("scoreboard", "players", "add", varName, env.Constants["globals_scoreboard"], Math.Abs(varValueInt).ToString()));
                    else if (varValueInt < 0)
                        commands.Add(new Command("scoreboard", "players", "remove", varName, env.Constants["globals_scoreboard"], Math.Abs(varValueInt).ToString()));
                    
                    break;

                default:
                    throw new Exception("Invalid usage of variable: Unknown operand");
            }

            return new ApplyResult(true, commands);
        }
    }
}
