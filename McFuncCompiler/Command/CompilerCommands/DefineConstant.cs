using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFuncCompiler.Command.CompilerCommands
{
    /// <summary>
    /// Command for defining constants in user code.
    /// </summary>
    public class DefineConstant : ICompilerCommand
    {
        public bool DoesApply(BuildEnvironment env, Command command)
        {
            return command.GetCommandName() == "define";
        }

        public void Apply(BuildEnvironment env, Command command)
        {
            if (command.Arguments.Count < 2 || command.Arguments.Count > 3)
                throw new Exception("Incorrect usage of 'define'!"); // todo: change exception

            string name = command.Arguments[1].Compile(env);
            string value = "true";

            if (command.Arguments.Count == 3)
                value = command.Arguments[2].Compile(env);

            env.Constants[name] = value;
        }
    }
}
