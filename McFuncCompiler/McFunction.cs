using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McFuncCompiler.Command;
using McFuncCompiler.Command.CustomCommands;
using McFuncCompiler.Command.Tokens;
using McFuncCompiler.Util;

namespace McFuncCompiler
{
    public class McFunction
    {
        /// <summary>
        /// In-game ID of this function - including the namespace.
        /// </summary>
        public string Id { get; set; }

        public List<Command.Command> Commands = new List<Command.Command>();

        public List<McFunction> ChildFunctions = new List<McFunction>();

        private static List<ICustomCommand> CustomCommands = new List<ICustomCommand>
        {
            new DefineConstantCommand(),
            new SetVariableCommand(),
            new IfCommand(),
            new ImportCommand()
        };

        private const int FunctionBlockNestingLimit = 1000;

        /// <summary>
        /// Compiled cache.
        /// </summary>
        private string _compiled;

        public McFunction(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Compile MCFunction.
        /// </summary>
        /// <param name="env">Build environment</param>
        /// <param name="generateOutput">Should a text output be generated?</param>
        public void Compile(BuildEnvironment env, bool generateOutput = true)
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

            // Split function blocks into seperate McFunctions
            SplitFunctionBlocks(env);

            if (generateOutput)
            {
                // Compile to vanilla MCFunction file
                GenerateMcFunction(env);
            }
        }

        public string GenerateMcFunction(BuildEnvironment env)
        {
            if (_compiled != null) return _compiled;

            // Compile to vanilla mcfunction
            var builder = new StringBuilder();

            // Append header
            string header = env.Constants["compiled_header"];
            header = header.Replace("{date}", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            header = header.Replace("{file}", Id);
            builder.Append("# " + header + "\n");

            // Compile commands
            foreach (Command.Command command in Commands)
            {
                var argBuilder = new StringBuilder();
                foreach (Argument arg in command.Arguments)
                {
                    argBuilder.Append(arg.Compile(env) + " ");
                }

                builder.Append(argBuilder.ToString().Trim() + "\n");
            }

            _compiled = builder.ToString();

            return _compiled;
        }

        /// <summary>
        /// Search for function blocks and split them out into their own McFunctions.
        /// </summary>
        public void SplitFunctionBlocks(BuildEnvironment env)
        {
            // Iterate over commands, finding function blocks in the *root level*
            // We then make take those commands into their own McFunction and run this function again on the sub McFunction.
            var functionBlocks = new Dictionary<Argument, Range>();
            int nesting = 0;
            int i = 0;
            foreach (Command.Command command in Commands)
            {
                Argument arg = command.Arguments.LastOrDefault();
                if (arg == null) continue;

                if (arg.Tokens.FirstOrDefault() is OpenFunctionBlockToken)
                {
                    nesting++;

                    if (nesting > FunctionBlockNestingLimit) throw new Exception($"Exceeded function block nesting limit of {FunctionBlockNestingLimit}!");

                    if (nesting == 1)
                    {
                        // Root level function block.
                        functionBlocks.Add(arg, new Range(i, -1));
                    }
                }

                if (arg.Tokens.FirstOrDefault() is CloseFunctionBlockToken)
                {
                    nesting--;

                    if (nesting < 0) throw new Exception("Unexpected additional function block bracket!");

                    if (nesting == 0)
                    {
                        // Back to root nesting, close this function block
                        functionBlocks.Last().Value.Maximum = i;
                    }
                }

                i++;
            }

            if (nesting > 0) throw new Exception("Unclosed function block!");

            // Move commands into child McFunctions
            int functionId = 0;
            foreach (KeyValuePair<Argument, Range> range in functionBlocks)
            {
                string parent = Id.Substring(0, Math.Max(Id.LastIndexOf("/"), Id.LastIndexOf(":")) + 1);
                string name = Id.Substring(Math.Max(Id.LastIndexOf("/"), Id.LastIndexOf(":")) + 1);

                if (!parent.EndsWith("_subs/"))
                    parent += "_subs/";

                var mcFunction = new McFunction(parent + name + "_" + functionId++);
                ChildFunctions.Add(mcFunction);
                
                // Move commands
                mcFunction.Commands.AddRange(Commands.GetRange(range.Value.Minimum + 1, range.Value.Length - 1));
                Commands.RemoveRange(range.Value.Minimum + 1, range.Value.Length);

                // Replace argument with function call
                range.Key.Tokens.Clear();
                range.Key.Tokens.Add(new TextToken("function " + mcFunction.Id));

                Logger.Debug($"Function block \"{mcFunction.Id}\" created from commands {functionBlocks.Last().Value} in {Id}!");

                // Run split function blocks on child so they can do their own function blocks.
                mcFunction.SplitFunctionBlocks(env);
            }
        }

        /// <summary>
        /// Save this MCFunction and optionally it's children.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="saveChildren"></param>
        public void Save(BuildEnvironment env, bool saveChildren = true)
        {
            string output = env.GetPath(Id, "mcfunction", env.OutputPath);
            Directory.GetParent(output).Create();
            File.WriteAllText(output, GenerateMcFunction(env));

            Logger.Info("Saved to: " + output);

            if (saveChildren)
            {
                foreach (McFunction function in ChildFunctions)
                {
                    function.Save(env);
                }
            }
        }
    }
}
