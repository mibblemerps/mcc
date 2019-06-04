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
            return Regex.IsMatch(command.GetCommandName(), Variable.VarRegex);
        }

        public ApplyResult Apply(BuildEnvironment env, Command command)
        {
            if (command.Arguments.Count != 3)
                throw new Exception("Invalid usage of variable: Invalid argument count");

            Variable variable = Variable.Parse(command.Arguments[0].Compile(env));
            string varOperand = command.Arguments[1].Compile(env);

            ValueType valueType = ValueType.Integer;
            string varValueStr = command.Arguments[2].Compile(env);

            // Check if we're trying to set a null value.
            if (varValueStr.ToLower() == "null")
            {
                valueType = ValueType.Null;
            }
            else if (int.TryParse(varValueStr, out int varValueInt))
            {
                valueType = ValueType.Integer;
                variable.Value = varValueInt;
            }
            else if (Regex.IsMatch(varValueStr, Variable.VarRegex))
            {
                valueType = ValueType.Variable;

                // todo: implement variable assignment
                throw new NotImplementedException("Assigning variables to other variables is not complete yet!");
            }

            // If no scoreboard given, use the default global one specified in the environment constants.
            if (variable.Scoreboard == null)
                variable.Scoreboard = env.Constants["globals_scoreboard"];

            var commands = new List<Command>();

            switch (varOperand)
            {
                case "=":
                    if (valueType == ValueType.Integer)
                        commands.Add(new Command("scoreboard", "players", "set", variable.Name, variable.Scoreboard, variable.Value.ToString()));
                    else if (valueType == ValueType.Null)
                        commands.Add(new Command("scoreboard", "players", "reset", variable.Name, variable.Scoreboard));

                    break;

                case "-=":
                    if (valueType != ValueType.Integer) throw new Exception("Invalid usage of variable: Integer expected!");

                    variable.Value = -variable.Value;
                    goto case "+="; // fallthrough

                case "+=":
                    if (valueType != ValueType.Integer) throw new Exception("Invalid usage of variable: Integer expected!");

                    if (variable.Value > 0)
                        commands.Add(new Command("scoreboard", "players", "add", variable.Name, variable.Scoreboard, Math.Abs(variable.Value.Value).ToString()));
                    else if (variable.Value < 0)
                        commands.Add(new Command("scoreboard", "players", "remove", variable.Name, variable.Scoreboard, Math.Abs(variable.Value.Value).ToString()));
                    
                    break;

                default:
                    throw new Exception("Invalid usage of variable: Unknown operand");
            }

            return new ApplyResult(true, commands);
        }

        enum ValueType
        {
            Integer,
            Null,
            Variable
        }
    }
}
