using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McFuncCompiler.Command;
using McFuncCompiler.Command.CompilerCommands;

namespace McFuncCompiler
{
    public class McFunction
    {
        public List<Command.Command> Commands = new List<Command.Command>();

        private static List<ICompilerCommand> CompilerCommands = new List<ICompilerCommand>
        {
            new DefineConstant()
        };

        public string Compile(BuildEnvironment env)
        {
            // Execute compiler commands
            var toRemove = new List<Command.Command>();
            foreach (Command.Command command in Commands)
            {
                foreach (ICompilerCommand compilerCommand in CompilerCommands)
                {
                    if (compilerCommand.DoesApply(env, command))
                    {
                        compilerCommand.Apply(env, command);
                        toRemove.Add(command);
                    }
                }
            }

            // Remove compiler commands (they are not meant for the output)
            foreach (Command.Command command in toRemove)
                Commands.Remove(command);
            toRemove.Clear();

            // Compile to vanilla mcfunction
            var builder = new StringBuilder();
            foreach (Command.Command command in Commands)
            {
                var argBuilder = new StringBuilder(); 
                foreach (Argument arg in command.Arguments)
                {
                    argBuilder.Append(arg.Compile(env) + " ");
                }

                builder.Append(argBuilder.ToString().Trim() + "\n");
            }

            return builder.ToString();
        }
    }
}
