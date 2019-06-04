using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using McFuncCompiler.Command;
using McFuncCompiler.Command.Tokens;
using McFuncCompiler.ParseAddons;

namespace McFuncCompiler
{
    public class Parser
    {
        protected McFunction McFunction;
        protected BuildEnvironment Environment;

        protected List<IParseAddon> ParseAddons;

        public Parser(BuildEnvironment env)
        {
            Environment = env;
        }

        public McFunction Parse(string id, string code = null)
        {
            if (code == null)
            {
                // Load file from path in build environment
                string path = Environment.GetPath(id, "mcfunction");
                Logger.Debug($"Loading mcfunction file from \"{path}\"...");
                code = File.ReadAllText(path);
            }

            ParseAddons = new List<IParseAddon>
            {
                new JsonImportParseAddon(),
                new ConstantParseAddon()
            };

            McFunction = new McFunction(id);

            // Iterate over lines in file
            foreach (string line in code.Split('\n'))
            {
                // Break command up into arguments
                var parts = CommandParser.Parse(line);

                if (parts == null)
                    continue;

                var cmd = new Command.Command();

                foreach (string part in parts)
                {
                    Argument argument = new Argument();

                    StringBuilder builder = new StringBuilder();
                    bool escaped = false;

                    foreach (char c in part + (char) 0)
                    {
                        // Skip parsing on escaped characters
                        if (escaped && c != 0)
                        {
                            builder.Append(c);
                            escaped = false;
                            continue;
                        }

                        // Check for escape character
                        if (c == '\\')
                        {
                            escaped = true;
                            continue;
                        }

                        // Allow any parse addons to do their work.
                        bool handled = false;
                        foreach (IParseAddon parseAddon in ParseAddons)
                        {
                            ParseAddonResponse resp = parseAddon.OnCharacter(cmd, argument, builder, c);
                            if (resp == ParseAddonResponse.Handled || resp == ParseAddonResponse.HandledClearBuffer)
                                handled = true;

                            if (resp == ParseAddonResponse.HandledClearBuffer && builder.Length > 0)
                            {
                                // Parse addon has requested we flush the current buffer
                                argument.Tokens.Add(new TextToken(builder.ToString()));
                                builder.Clear();
                            }
                        }
                        
                        if (c != 0 && !handled)
                        {
                            // No parse addons handled anything, treat as plaintext and append to buffer as normal.
                            builder.Append(c);
                        }
                    }

                    if (builder.Length > 0)
                    {
                        // There is stuff left in the buffer, output it to a text token
                        argument.Tokens.Add(new TextToken(builder.ToString()));
                        builder.Clear();
                    }

                    // Finished parsing argument
                    cmd.Arguments.Add(argument);
                }

                McFunction.Commands.Add(cmd);
            }

            return McFunction;
        }
    }
}
