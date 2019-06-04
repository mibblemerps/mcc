using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McFuncCompiler.Command;
using McFuncCompiler.Command.CustomCommands;

namespace McFuncCompiler
{
    public class McFunction
    {
        public List<Command.Command> Commands = new List<Command.Command>();

        private static List<ICustomCommand> CustomCommands = new List<ICustomCommand>
        {
            new DefineConstant(),
            new SetVariable()
        };

        public string Compile(BuildEnvironment env)
        {
            // Apply custom commands
            var toRemove = new List<Command.Command>();

            int i = 0;
            while (i < Commands.Count)
            {
                Command.Command command = Commands[i];

                foreach (ICustomCommand customCommand in CustomCommands)
                {
                    if (customCommand.DoesApply(env, command))
                    {
                        ApplyResult result = customCommand.Apply(env, command);

                        // Strip command from output if requested
                        if (result.StripFromOutput)
                            toRemove.Add(command);

                        // Add replacement commands
                        Commands.InsertRange(i + 1, result.AddCommands);
                    }
                }

                i++;
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
