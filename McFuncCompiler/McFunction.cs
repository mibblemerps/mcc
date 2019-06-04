using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McFuncCompiler.Command;
using McFuncCompiler.Command.CustomCommands;
using McFuncCompiler.Util;

namespace McFuncCompiler
{
    public class McFunction
    {
        /// <summary>
        /// In-game path of this function - including the namespace.
        /// </summary>
        public string Path { get; set; }

        public List<Command.Command> Commands = new List<Command.Command>();

        public List<McFunction> ChildFunctions = new List<McFunction>();

        /// <summary>
        /// Compiled result is stored here.
        /// </summary>
        public string Compiled { get; private set; }

        private static List<ICustomCommand> CustomCommands = new List<ICustomCommand>
        {
            new DefineConstant(),
            new SetVariable()
        };

        private const int FunctionBlockNestingLimit = 1000;

        public McFunction(string path)
        {
            Path = path;
        }

        public void Compile(BuildEnvironment env)
        {
            if (Compiled != null) throw new Exception("Already compiled. To recompile, create a new McFunction with the parser.");

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

            // Split function blocks into seperate McFunctions
            SplitFunctionBlocks(env);

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

            Compiled = builder.ToString();
        }

        /// <summary>
        /// Search for function blocks and split them out into their own McFunctions.
        /// </summary>
        public void SplitFunctionBlocks(BuildEnvironment env)
        {
            // Iterate over commands, finding function blocks in the *root level*
            // We then make take those commands into their own McFunction and run this function again on the sub McFunction.
            var functionBlocks = new List<Range>();
            int nesting = 0;
            int i = 0;
            foreach (Command.Command command in Commands)
            {
                Argument arg = command.Arguments.LastOrDefault();
                if (arg == null) continue;
                string argText = arg.GetAsText();

                if (argText == "(")
                {
                    nesting++;

                    if (nesting > FunctionBlockNestingLimit) throw new Exception($"Exceeded function block nesting limit of {FunctionBlockNestingLimit}!");

                    if (nesting == 1)
                    {
                        // Root level function block.
                        functionBlocks.Add(new Range(i, -1));
                    }
                }
                if (argText == ")")
                {
                    nesting--;

                    if (nesting < 0) throw new Exception("Unexpected additional function block bracket!");

                    if (nesting == 0)
                    {
                        // Back to root nesting, close this function block
                        functionBlocks.Last().Maximum = i;
                    }
                }

                i++;
            }

            if (nesting > 0) throw new Exception("Unclosed function block!");

            // Move commands into child McFunctions
            int functionId = 0;
            foreach (Range range in functionBlocks)
            {
                var mcFunction = new McFunction(Path + "__" + functionId++);
                ChildFunctions.Add(mcFunction);
                
                // Move commands
                mcFunction.Commands.AddRange(Commands.GetRange(range.Minimum + 1, range.Length - 1));
                Commands.RemoveRange(range.Minimum, range.Length + 1);
                Commands.Insert(range.Minimum, new Command.Command("function", mcFunction.Path));

                Logger.Debug($"Function block \"{mcFunction.Path}\" created from commands {functionBlocks.Last()} in {Path}!");

                // Run split function blocks on child so they can do their own function blocks.
                mcFunction.SplitFunctionBlocks(env);
            }
        }
    }
}
