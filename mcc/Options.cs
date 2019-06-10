using System.Collections.Generic;
using CommandLine;
using Newtonsoft.Json;

namespace mcc
{
    public class Options
    {
        public class CompileOptions
        {
            [Option('v', "verbose", Default = false, Required = false)]
            public bool Verbose { get; set; }

            [Option('c', "constants", Required = false, HelpText = "A list of constants to be included into the build environment. Either as JSON or colon seperated values.")]
            public string Constants { get; set; }

            public Dictionary<string, string> ToConstantsDictonary()
            {
                if (Constants == null)
                    return null;

                if (Constants.StartsWith("{"))
                {
                    // Treat as JSON
                    return JsonConvert.DeserializeObject<Dictionary<string, string>>(Constants);
                }

                // Treat as comma seperated values
                Dictionary<string, string> constants = new Dictionary<string, string>();
                string[] values = Constants.Split(',');
                foreach (string value in values)
                    constants.Add(value.Substring(0, value.IndexOf(':')), value.Substring(value.IndexOf(':') + 1));
                return constants;
            }
        }

        [Verb("compile", HelpText = "Compile a datapack")]
        public class CompileDatapack : CompileOptions
        {
            [Value(0, Required = true, MetaName = "Source Path", HelpText = "Path to *root* of the datapack to be compiled.")]
            public string SourceDirectory { get; set; }

            [Value(1, Required = true, MetaName = "Output Path", HelpText = "Path to save the compiled datapack to. This path will become the root of the compiled datapack.")]
            public string OutputDirectory { get; set; }
        }

        [Verb("compilefile", HelpText = "Compile a single mcfunction file")]
        public class CompileFile : CompileOptions
        {
            [Value(0, Required = true, MetaName = "Source", HelpText = "Path to source mcfunction file.")]
            public string Source { get; set; }

            [Value(1, Required = true, MetaName = "Output", HelpText = "Path to save compiled mcfunction file to.")]
            public string Output { get; set; }
        }
    }
}
