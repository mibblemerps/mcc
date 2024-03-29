﻿using System.Collections.Generic;
using System.IO;
using mcc.Command;
using mcc.Command.Tokens;
using mcc.Parser.ParseFilters;
using mcc.Parser.ParseFilters.If;

namespace mcc.Parser
{
    public class Parser
    {
        protected McFunction McFunction;
        protected BuildEnvironment Environment;
        
        protected List<IParseFilter> ParseFilters = new List<IParseFilter>
        {
            new ConstantParseFilter(),
            new JsonImportParseFilter(),
            new FunctionBlockParseFilter(),
            new IfParseFilter(),
            new DefineConstantParseFilter()
        };

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
                code = File.ReadAllText(path);
            }

            McFunction = new McFunction(id);

            // Iterate over lines in file
            foreach (string line in code.Split('\n'))
            {
                // Break command up into arguments
                var parts = CommandParser.Parse(line);

                if (parts == null)
                    continue;

                // Parse command into arguments
                var cmd = new Command.Command();
                foreach (string part in parts)
                {
                    Argument argument = new Argument();
                    argument.Tokens.Add(new TextToken(part));

                    // Run argument through filters
                    foreach (IParseFilter filter in ParseFilters)
                        argument = filter.FilterArgument(argument);

                    if (argument != null)
                        cmd.Arguments.Add(argument);
                }

                // Run command through filters
                foreach (IParseFilter filter in ParseFilters)
                    cmd = filter.Filter(cmd);

                McFunction.Commands.Add(cmd);
            }

            return McFunction;
        }
    }
}
