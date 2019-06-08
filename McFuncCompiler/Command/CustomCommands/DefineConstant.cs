using System;
using System.Text;

namespace McFuncCompiler.Command.CustomCommands
{
    /// <summary>
    /// Command for defining constants in user code.
    /// </summary>
    public class DefineConstant : ICustomCommand
    {
        public bool CompilerOnly => true;

        public bool DoesApply(BuildEnvironment env, Command command)
        {
            return command.GetCommandName() == "define";
        }

        public ApplyResult Apply(BuildEnvironment env, Command command)
        {
            if (command.Arguments.Count < 2)
                throw new Exception("Incorrect usage of 'define'!"); // todo: change exception

            string name = command.Arguments[1].Compile(env);
            string value = "true";

            if (command.Arguments.Count >= 3)
            {
                var builder = new StringBuilder();
                for (var i = 2; i < command.Arguments.Count; i++)
                {
                    builder.Append(command.Arguments[i].Compile(env) + " ");
                }

                value = builder.ToString();
            }

            env.Constants[name] = value;

            return new ApplyResult(true);
        }
    }
}
